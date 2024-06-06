Use Master;

CREATE DATABASE Car_Rental;

Use Car_Rental;

-- Create the Customers table for storing Customer details
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1, 1) PRIMARY KEY,
    CustomerName NVARCHAR(200) UNIQUE NOT NULL,
    National_ID_Number CHAR(14) UNIQUE NOT NULL,
    Driver_License_Number CHAR(14) UNIQUE NOT NULL,
    Phone CHAR(11) NOT NULL CHECK (ISNUMERIC(Phone) = 1), -- Ensure it's numeric

	-- Ensure that each record has valid 'National ID' and Valid 'Driver License Number'
	CONSTRAINT CHK_Valid_National_ID CHECK (ISNUMERIC(National_ID_Number) = 1 And LEN(National_ID_Number) = 14),
	CONSTRAINT CHK_Valid_Driver_License  CHECK (ISNUMERIC(Driver_License_Number) = 1 And LEN(Driver_License_Number) = 14)
);

-- Create the FuelTypes table for storing Vehicles Fuel Type details
CREATE TABLE FuelTypes (
	ID INT PRIMARY KEY,
	FuelType NVARCHAR(20) NOT NULL UNIQUE
);

-- Create the VehiclesCategory table for storing Vehicle Category details
CREATE TABLE VehiclesCategory (
	CategoryID INT IDENTITY(1, 1) PRIMARY KEY,
	CategoryName NVARCHAR(50)NOT NULL UNIQUE
);

-- Create the Vehicles table for storing Vehicle details
CREATE TABLE Vehicles (
	VehicleID INT IDENTITY(1, 1) PRIMARY KEY,
	Make NVARCHAR(50)NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	Year Date NOT NULL,
	mileage INT NOT NULL CHECK(mileage >= 0),
	FuelTypeID INT NOT NULL, -- Foreign key reference to the FuelTypes table's ID.
	PlateNumber NVARCHAR(11) UNIQUE NOT NULL, 
	CarCategoryID INT NOT NULL, -- Foreign key reference to the VehiclesCategory table's CategoryID.
	RentalPricePerDay SMALLMONEY NOT NULL,
	ISAvilableForRent BIT NOT NULL

	CONSTRAINT FK_Vehicle_FuelType FOREIGN KEY (FuelTypeID) REFERENCES FuelTypes(ID),
	CONSTRAINT FK_Vehicle_Category FOREIGN KEY (CarCategoryID) REFERENCES VehiclesCategory(CategoryID)
);

-- Create the Maintenance table for storing vehicle maintenance details
CREATE TABLE Maintenance (
    MaintenanceID INT IDENTITY(1, 1) PRIMARY KEY,
    VehicleID INT NOT NULL, -- Foreign key referencing the Vehicles table's VehicleID.
    MaintenanceDescription NVARCHAR(300) NOT NULL,
    MaintenanceDate DATE NOT NULL DEFAULT GETDATE(),
    MaintenanceCost DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_Maintenance_VehicleID FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
);

-- Create the RentalBooking table for storing rental booking details
CREATE TABLE RentalBooking (
    BookingID INT IDENTITY(1, 1) PRIMARY KEY,
    CustomerID INT NOT NULL, -- Foreign key reference to the Customers table's CustomerID.
    VehicleID INT NOT NULL, -- Foreign key reference to the VehicleIs table's VehicleID.
    RentalDurationDays TINYINT NOT NULL CHECK (RentalDurationDays > 0), -- عدد أيام الإيجار
    RentalStartDate DATE NOT NULL DEFAULT GETDATE(),
    RentalEndDate AS DATEADD(DAY, RentalDurationDays, RentalStartDate), -- Computed column
    PickupLocation NVARCHAR(100) NOT NULL,
    DropOffLocation NVARCHAR(100) NOT NULL,
    RentalPricePerDay SMALLMONEY NOT NULL, --  this Column will get it from VehicleIs Table Based on VehicleID
    InitialTotalDueAmount AS (RentalDurationDays * RentalPricePerDay), --  this Computed Column will Calculate it Based on RentalPricePerDay Column and RentalDurationDays Column
    InitialCheckNotes NVARCHAR(500) NOT NULL,

    CONSTRAINT FK_Booking_CustomerID FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_Booking_VehicleID FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
	
    -- RentalPricePerDay -> سعر الإيجار في اليوم الواحد
    -- InitialTotalDueAmount -> إجمالي المبلغ المستحق الأولي
    -- InitialCheckNotes -> ملاحظات الفحص الأولي
);

-- Create the VehicleReturns table for storing vehicle return details
CREATE TABLE VehicleReturns (
    ReturnID INT IDENTITY(1, 1) PRIMARY KEY,
    BookingID INT UNIQUE NOT NULL, -- Foreign key reference to the RentalBooking table's BookingID.
    ActualReturnDate DATETIME NOT NULL DEFAULT GETDATE(),
    ActualRentalDays TINYINT NOT NULL,
    Mileage SMALLINT NOT NULL,
    ConsumedMileage SMALLINT NOT NULL, -- الأميال المستهلكة The additional mileage driven during the rental period, calculated as the difference between the current mileage and the vehicle's mileage at the start of the rental.
    FinalCheckNotes NVARCHAR(500) NOT NULL,
    AdditionalCharges SMALLMONEY NOT NULL, -- رسوم إضافية
    ActualTotalDueAmount SMALLMONEY Not NULL, -- إجمالي المبلغ المستحق الفعلي
    CONSTRAINT FK_VehicleReturns_BookingID FOREIGN KEY (BookingID) REFERENCES RentalBooking(BookingID),
);

-- Create the RentalTransaction table for storing rental transaction details
CREATE TABLE RentalTransaction (
    TransactionID INT IDENTITY(1, 1) PRIMARY KEY,
    BookingID INT UNIQUE NOT NULL, -- Foreign key reference to the RentalBooking table's BookingID.
    PaidInitialTotalDueAmount SMALLMONEY NOT NULL, -- إجمالي المبلغ المستحق الأولي المدفوع
    PaymentDetails NVARCHAR(100) NOT NULL,
    TransactionDate DATETIME NOT NULL,
    ReturnID INT UNIQUE, -- Foreign key reference to the VehicleReturns table's ReturnID.
    ActualTotalDueAmount SMALLMONEY , -- إجمالي المبلغ المستحق الفعلي
    TotalRemaining SMALLMONEY , -- اجمالي المتبقي
    TotalRefundedAmount SMALLMONEY, -- إجمالي المبلغ المسترد
    UpdatedTransactionDate DATETIME,
    CONSTRAINT FK_RentalTransaction_BookingID FOREIGN KEY (BookingID) REFERENCES RentalBooking(BookingID),
    CONSTRAINT FK_RentalTransaction_ReturnID FOREIGN KEY (ReturnID) REFERENCES VehicleReturns(ReturnID)
);




-- CREATE Some Vehicles views (Optional)

-- Create a view to display information about gasoline cars available for rent.
CREATE VIEW Gasoline_Cars AS (
    SELECT VehicleID, Make, Year, mileage, 
           FuelTypes.FuelType AS 'Fuel Type', 
           PlateNumber AS 'Plate Number', 
           VehiclesCategory.CategoryName AS 'Car Category', 
           RentalPricePerDay, 
           CASE
               WHEN ISAvilableForRent = 1 THEN 'Available'
               WHEN ISAvilableForRent = 0 THEN 'Not Available'
           END AS 'IS Available For Rent'
    FROM Vehicles
    JOIN FuelTypes ON FuelTypes.ID = Vehicles.FuelTypeID
    JOIN VehiclesCategory ON VehiclesCategory.CategoryID = Vehicles.CarCategoryID
    WHERE FuelTypes.FuelType LIKE '%Gasoline%'
);

-- Create a view to display information about diesel cars available for rent.
CREATE VIEW Diesel_Cars AS (
    SELECT VehicleID, Make, Year, mileage, 
           FuelTypes.FuelType AS 'Fuel Type', 
           PlateNumber AS 'Plate Number', 
           VehiclesCategory.CategoryName AS 'Car Category', 
           RentalPricePerDay, 
           CASE
               WHEN ISAvilableForRent = 1 THEN 'Available'
               WHEN ISAvilableForRent = 0 THEN 'Not Available'
           END AS 'IS Available For Rent'
    FROM Vehicles
    JOIN FuelTypes ON FuelTypes.ID = Vehicles.FuelTypeID
    JOIN VehiclesCategory ON VehiclesCategory.CategoryID = Vehicles.CarCategoryID
    WHERE FuelTypes.FuelType LIKE '%Diesel%'
);


-- Create a view to display information about electric cars available for rent.
CREATE VIEW Electric_Cars AS (
    SELECT VehicleID, Make, Year, mileage, 
           FuelTypes.FuelType AS 'Fuel Type', 
           PlateNumber AS 'Plate Number', 
           VehiclesCategory.CategoryName AS 'Car Category', 
           RentalPricePerDay, 
           CASE
               WHEN ISAvilableForRent = 1 THEN 'Available'
               WHEN ISAvilableForRent = 0 THEN 'Not Available'
           END AS 'IS Available For Rent'
    FROM Vehicles
    JOIN FuelTypes ON FuelTypes.ID = Vehicles.FuelTypeID
    JOIN VehiclesCategory ON VehiclesCategory.CategoryID = Vehicles.CarCategoryID
    WHERE FuelTypes.FuelType LIKE '%Electric%'
);

-- Create a view to display information about hybrid cars available for rent.
CREATE VIEW Hybrid_Cars AS (
    SELECT VehicleID, Make, Year, mileage, 
           FuelTypes.FuelType AS 'Fuel Type', 
           PlateNumber AS 'Plate Number', 
           VehiclesCategory.CategoryName AS 'Car Category', 
           RentalPricePerDay, 
           CASE
               WHEN ISAvilableForRent = 1 THEN 'Available'
               WHEN ISAvilableForRent = 0 THEN 'Not Available'
           END AS 'IS Available For Rent'
    FROM Vehicles
    JOIN FuelTypes ON FuelTypes.ID = Vehicles.FuelTypeID
    JOIN VehiclesCategory ON VehiclesCategory.CategoryID = Vehicles.CarCategoryID
    WHERE FuelTypes.FuelType LIKE '%Hybrid%'
);
