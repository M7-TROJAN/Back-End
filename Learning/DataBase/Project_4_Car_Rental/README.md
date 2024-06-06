# Car Rental Management System

 Welcome to the Car Rental Management System, a comprehensive database designed to streamline the operations of your car rental business efficiently.

## Project Purpose

Hello, I am Mahmoud, a CS student. This project has been created with the primary goal of providing me with a hands-on learning experience in database management. While the system's capabilities are well-suited for a car rental business, please note that the primary objective here is educational, and it is not intended for commercial use.

The Car Rental Management System serves as a learning tool, allowing me to practice and implement the database design principles I've studied. I can use this system to gain a deeper understanding of how databases work in real-world scenarios, enhancing my knowledge and skills in the field of database management.

## Table of Contents
1. [Relational Schema](#relational-schema)
2. [Project Structure](#project-structure)
3. [Database Schema](#database-schema)
4. [Usage](#usage)
5. [Sample Scripts](#Usage-Examples)
6. [Full Real Examples](#Full-Real-Examples)
7. [License](#license)
8. [Author](#Author)

## Relational Schema

![Relational Schema](Relational_Schema_Rental_Car_.png)

- You can view the [DrawSQL diagram here](https://drawsql.app/teams/m7/diagrams/car-rental).


## Project Structure

The project repository contains the following files:

- `Requirements.pdf`: This document contains the project requirements and specifications.

- `schema.sql`: The SQL script to create the database schema for the Car Rental Management System.

- `insert-data.sql`: This SQL script provides sample data to populate the database tables, demonstrating how to add customers, fuel types, vehicle categories, vehicles, and make rental bookings.

## Database Schema

The schema for the database includes the following tables:
- Customers
- FuelTypes
- VehiclesCategory
- Vehicles
- Maintenance
- RentalBooking
- VehicleReturns
- RentalTransaction

The database schema is defined in the `schema.sql` file.

## Usage

1. Import the database schema by running the `schema.sql` script on your SQL Server.

2. Insert sample data and records using the `insert-data.sql` script as an example. You can customize this script to add more data as needed.

3. The system is now ready for you to manage your car rental business. Use SQL queries to interact with the database and perform operations such as booking rentals, returning vehicles, and managing customer data.

4. Ensure to update and customize the scripts for your specific use case, including real customer data, vehicle details, and booking information.


## Usage Examples

**Booking a Rental**:

Suppose you want to book a rental for a customer. You can use the following SQL script as an example:

```sql
-- To insert a new customer and book a rental, use the following script:
-- Insert a new Customer
-- INSERT INTO Customers (CustomerName, National_ID_Number, Driver_License_Number, Phone) VALUES ...

-- Get the CustomerID of the newly inserted Customer

-- Get the VehicleID based on availability and criteria

-- Insert a new RentalBooking record
-- INSERT INTO RentalBooking (CustomerID, VehicleID, RentalDurationDays, RentalStartDate, PickupLocation, DropOffLocation, RentalPricePerDay, InitialCheckNotes) VALUES ...

-- Get the BookingID of the newly inserted Booking

-- Insert a record into the RentalTransaction table to track the initial payment for a rental booking.
-- INSERT INTO RentalTransaction ( BookingID, PaidInitialTotalDueAmount, PaymentDetails, TransactionDate, ReturnID, ActualTotalDueAmount, TotalRemaining, TotalRefundedAmount, UpdatedTransactionDate ) VALUES ...
```

## Full Real Examples
**Test Scripts for Database Verification**

**Important Note: Execute Each Script Separately to Prevent Errors; Avoid Running All at Once Take Care, Bro**

- Example to insert fuel types
```SQL
-- Insert sample fuel types into the FuelTypes table
INSERT INTO FuelTypes (ID, FuelType) VALUES
(1, 'Gasoline (Petrol)'),
(2, 'Diesel'),
(3, 'Electric'),
(4, 'Hybrid');

```

- Example to insert vehicle categories
```SQL
-- Insert sample vehicle categories into the VehiclesCategory table
INSERT INTO VehiclesCategory (CategoryName) VALUES
('4x4'),
('Sedan'),
('SUV'),
('Sports Car'),
('Minivan');
```

-- Example to Insert Vehicles
```sql
-- Insert a Sample Vehicle
INSERT INTO Vehicles (Make, Model, Year, mileage, FuelTypeID, PlateNumber, CarCategoryID, RentalPricePerDay, ISAvilableForRent)
VALUES ('Toyota', 'Camry', '2021-01-01', 10000, 1, 'ABC123', 1, 50.00, 1),
	   ('Tesla', 'Model 3', '2023-01-01', 0, 4, 'TESLA001', 2, 80.00, 1);

```

- Insert a Vehicle by specifying FuelType and CarCategory
```sql
DECLARE @FuelTypeID INT, @CarCategoryID INT;

-- Get the FuelTypeID for 'Gasoline' fuel type
SELECT @FuelTypeID = ID FROM FuelTypes WHERE FuelType LIKE '%Gasoline%';

-- Get the CarCategoryID for 'Sports' category
SELECT @CarCategoryID = CategoryID FROM VehiclesCategory WHERE CategoryName LIKE '%Sports%';

-- Insert the vehicle into the Vehicles table
INSERT INTO Vehicles (Make, Model, Year, Mileage, FuelTypeID, PlateNumber, CarCategoryID, RentalPricePerDay, ISAvilableForRent)
VALUES ('Honda', 'Civic', '2020-01-01', 8000, @FuelTypeID, 'XYZ789', @CarCategoryID, 45.00, 1);
```

- Example to insert customers
```sql
-- Insert the first customer
INSERT INTO Customers (CustomerName, National_ID_Number, Driver_License_Number, Phone)
VALUES ('Mahmoud Mohamed', '29803031401528', '56789012345678', '01019760452');

-- Insert the second customer
INSERT INTO Customers (CustomerName, National_ID_Number, Driver_License_Number, Phone)
VALUES ('Mohamed Salah', '30003031491528', '43210987654321', '01129416608');
```

- Example for Creating a New Rental Booking and Recording the Associated Transaction:

```sql
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
-- so we don't need to include him as a new record We Will Retrive Him From Customers Table.
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

-- Retrieve 'InitialTotalDueAmount' From the inserted RentalBooking record
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
```

- Example for Recording a Vehicle Return and Updating Rental Transaction

```sql
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
```

- Example to insert a Maintenance record
```sql
-- Maintenance for Vehicle 1
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (11, 'Oil Change', 50.00);

-- Maintenance for Vehicle 2
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (12, 'Brake Inspection', 75.00);

-- Maintenance for Vehicle 1
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (13, 'Tire Rotation', 30.00);

-- Maintenance for Vehicle 3
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (14, 'Air Filter Replacement', 20.00);

-- Maintenance for Vehicle 2
INSERT INTO Maintenance (VehicleID, MaintenanceDescription, MaintenanceCost)
VALUES (15, 'Coolant Flush', 60.00);
```

- Feel free to customize and extend the database schema to meet your specific requirements.

## License
This project is licensed under the [MIT License](LICENSE). You are free to use, modify, and distribute this code as per the license terms.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)
- GitHub: [mattar740](https://github.com/mattar740)

Have a great time Happy learning and coding! 🚀
