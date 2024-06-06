-- Test Scripts for Database Verification Important 
-- Note: Execute Each Script Separately to Prevent Errors; Avoid Running All at Once Take Care, Bro


Use Car_Rental;

-- Insert sample fuel types into the FuelTypes table
INSERT INTO FuelTypes (ID, FuelType) VALUES
(1, 'Gasoline (Petrol)'),
(2, 'Diesel'),
(3, 'Electric'),
(4, 'Hybrid');

---------------------------------------------------------------------------------------------------------------------

-- Insert sample vehicle categories into the VehiclesCategory table
INSERT INTO VehiclesCategory (CategoryName) VALUES
('4x4'),
('Sedan'),
('SUV'),
('Sports Car'),
('Minivan');

---------------------------------------------------------------------------------------------------------------------


-- Insert Vehicles

-- Insert a Vehicles
INSERT INTO Vehicles (Make, Model, Year, mileage, FuelTypeID, PlateNumber, CarCategoryID, RentalPricePerDay, ISAvilableForRent)
VALUES ('Toyota', 'Camry', '2021-01-01', 10000, 1, 'ABC123', 1, 50.00, 1),
	   ('Tesla', 'Model 3', '2023-01-01', 0, 4, 'TESLA001', 2, 80.00, 1);

-- Insert a Vehicle by specifying FuelType and CarCategory
DECLARE @FuelTypeID INT, @CarCategoryID INT;

-- Get the FuelTypeID for 'Gasoline' fuel type
SELECT @FuelTypeID = ID FROM FuelTypes WHERE FuelType LIKE '%Gasoline%';

-- Get the CarCategoryID for 'Sports' category
SELECT @CarCategoryID = CategoryID FROM VehiclesCategory WHERE CategoryName LIKE '%Sports%';

-- Insert the vehicle into the Vehicles table
INSERT INTO Vehicles (Make, Model, Year, Mileage, FuelTypeID, PlateNumber, CarCategoryID, RentalPricePerDay, ISAvilableForRent)
VALUES ('Honda', 'Civic', '2020-01-01', 8000, @FuelTypeID, 'XYZ789', @CarCategoryID, 45.00, 1);

---------------------------------------------------------------------------------------------------------------------


-- Example to insert customers

-- Insert the first customer
INSERT INTO Customers (CustomerName, National_ID_Number, Driver_License_Number, Phone)
VALUES ('Mahmoud Mohamed', '29803031401528', '56789012345678', '01019760452');

-- Insert the second customer
INSERT INTO Customers (CustomerName, National_ID_Number, Driver_License_Number, Phone)
VALUES ('Mohamed Salah', '30003031491528', '43210987654321', '01129416608');

---------------------------------------------------------------------------------------------------------------------

-- Example for Creating a New Rental Booking and Recording the Associated Transaction:

-- Start a transaction
BEGIN TRANSACTION;

-- Declare variables for input data
DECLARE 
@CustomerID INT,
@VehicleID INT, 
@RentalDurationDays TINYINT,
@RentalStartDate DATE, 
@PickupLocation NVARCHAR(100), 
@DropOffLocation NVARCHAR(100), 
@RentalPricePerDay SMALLMONEY, 
@InitialCheckNotes NVARCHAR(500);

-- Assign values to the variables

-- Assume that the customer who is going to rent the car already exists in the database, 
-- so we don't need to include him as a new record
SELECT @CustomerID = (SELECT CustomerID FROM Customers WHERE CustomerName = 'Mahmoud Mohamed');

-- Retrieve the VehicleID from the Vehicles table
SELECT @VehicleID = (SELECT TOP 1 VehicleID FROM Vehicles 
WHERE Make = 'Toyota' AND Year = '2021-01-01' AND Model = 'Camry' AND ISAvilableForRent = 1);

-- Retrieve the RentalPricePerDay from the Vehicles table Based On VehicleID
SELECT @RentalPricePerDay = (SELECT RentalPricePerDay FROM Vehicles WHERE VehicleID = @VehicleID);

-- Set other variables
SELECT @RentalDurationDays = 7; -- Replace with the actual Rental Duration Days
SELECT @RentalStartDate = GETDATE(); -- Use the current date as the start date 
SELECT @PickupLocation = 'Test Pickup Location'; -- Replace with the actual Pickup Location
SELECT @DropOffLocation = 'Test DropOff Location'; -- Replace with the actual DropOff Location
SELECT @InitialCheckNotes = 'Test Check Notes'; -- Replace with the actual Check Notes

-- Insert the rental booking record
INSERT INTO RentalBooking (CustomerID, VehicleID, RentalDurationDays, RentalStartDate, PickupLocation, DropOffLocation, RentalPricePerDay, InitialCheckNotes)
VALUES (
    @CustomerID,
    @VehicleID,
    @RentalDurationDays,
    @RentalStartDate,
    @PickupLocation,
    @DropOffLocation,
    @RentalPricePerDay,
    @InitialCheckNotes
);

-- Retrieve the 'BookingID' of the inserted RentalBooking record
DECLARE @BookingID INT;
SELECT @BookingID = SCOPE_IDENTITY();

---- Retrieve 'InitialTotalDueAmount' From the inserted RentalBooking record
DECLARE @PaidInitialTotalDueAmount SMALLMONEY;
SELECT @PaidInitialTotalDueAmount = (SELECT InitialTotalDueAmount FROM RentalBooking WHERE BookingID = @BookingID );


-- Insert a record into the RentalTransaction table to track the initial payment for a rental booking.

-- Define variables for the input data.
DECLARE @PaymentDetails NVARCHAR(100) = 'Payment received for initial booking';
DECLARE @TransactionDate DATETIME = GETDATE();
DECLARE @ReturnID INT = NULL; -- We will update this when the customer returns the car.
DECLARE @ActualTotalDueAmount SMALLMONEY = NULL; -- We will update this when the customer returns the car.
DECLARE @TotalRemaining SMALLMONEY = NULL; -- We will update this when the customer returns the car.
DECLARE @TotalRefundedAmount SMALLMONEY = NULL; -- We will update this when the customer returns the car.
DECLARE @UpdatedTransactionDate DATETIME = NULL; -- We will update this when the customer returns the car.

-- Insert the record into the RentalTransaction table.
INSERT INTO RentalTransaction (
    BookingID,
    PaidInitialTotalDueAmount,
    PaymentDetails,
    TransactionDate,
    ReturnID,
    ActualTotalDueAmount,
    TotalRemaining,
    TotalRefundedAmount,
    UpdatedTransactionDate
)
VALUES (
    @BookingID, -- Booking ID for the associated rental.
    @PaidInitialTotalDueAmount, -- Amount paid initially.
    @PaymentDetails, -- Details of the payment.
    @TransactionDate, -- Date and time of the transaction.
    @ReturnID, -- ID of the return record (null for now, to be updated).
    @ActualTotalDueAmount, -- Total amount due (null for now, to be updated).
    @TotalRemaining, -- Total remaining amount (null for now, to be updated).
    @TotalRefundedAmount, -- Total refunded amount (null for now, to be updated).
    @UpdatedTransactionDate -- Date when the transaction is updated (null for now, to be updated).
);



-- Check if the insertion was successful
IF @@ERROR = 0
BEGIN
    -- If successful, commit the transaction
	-- Note: Don't Forget To Set 'ISAvilableForRent' Vehicles Column To 0
	UPDATE Vehicles
	SET ISAvilableForRent = 0
	WHERE VehicleID = @VehicleID;

    COMMIT;
    PRINT 'Transaction committed successfully.';
END
ELSE
BEGIN
    -- If unsuccessful, roll back the transaction
    ROLLBACK;
    PRINT 'Transaction rolled back due to an error.';
END;

---------------------------------------------------------------------------------------------------------------------

-- Example for Recording a Vehicle Return and Updating Rental Transaction

-- Start a database transaction
BEGIN TRANSACTION;

-- Declare variables for input data
DECLARE 
@BookingID INT,  -- ID of the booking for the vehicle return
@ActualReturnDate DATETIME,  -- Actual date and time of the vehicle return
@ActualRentalDays TINYINT,  -- Number of days the vehicle was rented
@CurrentMileage SMALLINT,  -- Mileage of the vehicle at the time of return
@ConsumedMileage SMALLINT,  -- Mileage consumed during the rental
@FinalCheckNotes NVARCHAR(500),  -- Notes about the condition of the returned vehicle
@AdditionalCharges SMALLMONEY,  -- Any additional charges incurred during the rental
@ActualTotalDueAmount SMALLMONEY;  -- Total amount due for the rental

-- Assign values to the variables

-- Replace these values with actual data
SELECT @BookingID = 2;  -- Replace with the actual BookingID
SELECT @ActualReturnDate = '2023-10-27 12:00:00';  -- Replace with the actual return date

-- Calculate the number of rental days based on the rental start date and actual return date
SELECT @ActualRentalDays = DATEDIFF(DAY, (SELECT RentalStartDate FROM RentalBooking WHERE BookingID = @BookingID), @ActualReturnDate);

-- Replace with the actual mileage of the returned vehicle
SELECT @CurrentMileage = 10200; 

-- Calculate the consumed mileage by subtracting the current mileage from the mileage at the start of the rental
SELECT @ConsumedMileage = @CurrentMileage - (SELECT Vehicles.mileage
FROM RentalBooking
JOIN Vehicles ON Vehicles.VehicleID = RentalBooking.VehicleID
WHERE RentalBooking.BookingID = @BookingID);

-- Provide notes on the condition of the vehicle at return
SELECT @FinalCheckNotes = 'All good';  -- Replace with actual notes

-- Include any additional charges incurred during the rental
SELECT @AdditionalCharges = 25.00;  -- Replace with actual charges

-- Calculate the actual total amount due, considering the rental duration and additional charges

-- Retrieve the VehicleID associated with the BookingID from the RentalBooking table
DECLARE @VehicleID INT;
SELECT @VehicleID = (SELECT VehicleID FROM RentalBooking WHERE BookingID = @BookingID)

-- Calculate the total due amount, taking into account the rental duration and any additional charges
SELECT @ActualTotalDueAmount = (@ActualRentalDays * (SELECT RentalPricePerDay FROM Vehicles WHERE VehicleID = @VehicleID)) + @AdditionalCharges;

-- Insert the record into the VehicleReturns table

INSERT INTO VehicleReturns (BookingID, ActualReturnDate, ActualRentalDays, Mileage, ConsumedMileage, FinalCheckNotes, AdditionalCharges, ActualTotalDueAmount)
VALUES (
	@BookingID, 
	@ActualReturnDate, 
	@ActualRentalDays, 
	@CurrentMileage, 
	@ConsumedMileage, 
	@FinalCheckNotes, 
	@AdditionalCharges, 
	@ActualTotalDueAmount
);

-- Retrieve the ReturnID of the inserted record
DECLARE @ReturnID INT;
SELECT @ReturnID = SCOPE_IDENTITY();

-- Retrieve the paid initial total due amount

DECLARE @PaidInitialTotalDueAmount SMALLMONEY;
SELECT @PaidInitialTotalDueAmount = PaidInitialTotalDueAmount 
FROM RentalTransaction
WHERE BookingID = @BookingID;

-- Update the RentalTransaction Table

UPDATE RentalTransaction
SET ReturnID = @ReturnID,
	ActualTotalDueAmount = @ActualTotalDueAmount,
	TotalRemaining = CASE
		WHEN @ActualTotalDueAmount > @PaidInitialTotalDueAmount THEN @ActualTotalDueAmount - @PaidInitialTotalDueAmount
		ELSE 0.00  -- Ensure that TotalRemaining is not negative
	END,
	TotalRefundedAmount = CASE
		WHEN @PaidInitialTotalDueAmount > @ActualTotalDueAmount THEN @PaidInitialTotalDueAmount - @ActualTotalDueAmount
		ELSE 0.00  -- Ensure that TotalRefundedAmount is not negative
	END,
	UpdatedTransactionDate = @ActualReturnDate

WHERE BookingID = @BookingID;

-- Check if the insertion and update were successful
IF @@ERROR = 0
BEGIN
    -- If successful, commit the transaction

    -- Update the vehicle's mileage in the Vehicles table
    UPDATE Vehicles
    SET mileage = @CurrentMileage
    WHERE VehicleID = @VehicleID;

    -- Set 'ISAvailableForRent' in the Vehicles table to 1 if the FinalCheckNotes indicate that the vehicle is in good condition
    -- (Assuming '0' represents a problem with the car)
    IF @FinalCheckNotes <> '0'
    BEGIN
        UPDATE Vehicles
        SET ISAvailableForRent = 1
        WHERE VehicleID = @VehicleID;
    END

    COMMIT;
    PRINT 'Transaction committed successfully.';
END
ELSE
BEGIN
    -- If unsuccessful, roll back the transaction
    ROLLBACK;
    PRINT 'Transaction rolled back due to an error.';
END;


---------------------------------------------------------------------------------------------------------------------

-- Maintenance for Vehicle 1
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (5, 'Oil Change', 50.00);

-- Maintenance for Vehicle 2
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (6, 'Brake Inspection', 75.00);

-- Maintenance for Vehicle 1
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (7, 'Tire Rotation', 30.00);

-- Maintenance for Vehicle 3
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (8, 'Air Filter Replacement', 20.00);

-- Maintenance for Vehicle 2
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (9, 'Coolant Flush', 60.00);

---------------------------------------------------------------------------------------------------------------------

-- Retrieve and display customer information
SELECT * FROM Customers;

-- Retrieve and display vehicle details
SELECT * FROM Vehicles;

-- Retrieve and display rental booking records
SELECT * FROM RentalBooking;

-- Retrieve and display rental transaction records
SELECT * FROM RentalTransaction;

-- Retrieve and display vehicle return records
SELECT * FROM VehicleReturns;

-- Retrieve and display maintenance records
SELECT * FROM Maintenance;
