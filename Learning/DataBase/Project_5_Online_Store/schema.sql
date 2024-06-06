
CREATE DATABASE  OnlineStore;

USE OnlineStore;

-- Create Customers table to store customer information.
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1, 1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    Email NCHAR(100) UNIQUE NOT NULL,
	Phone NCHAR(11) NOT NULL CHECK (Phone NOT LIKE '%[^0-9]%'),
    Address NVARCHAR(200) NOT NULL,
    Username NVARCHAR(25) UNIQUE NOT NULL,
    Password NVARCHAR(32) NOT NULL
);

-- Constraint to validate Email format.
ALTER TABLE Customers
ADD CONSTRAINT CHK_EmailFormat
CHECK (
    Email LIKE '%@%'
    AND Email LIKE '%.%'
    AND LEN(Email) >= 10
    AND LEN(Email) <= 255
);

-- Constraint to validate Username format.
-- The Username can contain letters and numbers but should not be all numeric.
ALTER TABLE Customers
ADD CONSTRAINT CHK_UsernameFormat
CHECK (
    (Username NOT LIKE '%[0-9]%') OR
    (Username LIKE '%[a-zA-Z]%[0-9a-zA-Z]%')
);

-- Constraint to enforce password complexity.
-- The password must be between 5 and 32 characters, contain at least one lowercase letter,
-- one uppercase letter, and one special character.
ALTER TABLE Customers
ADD CONSTRAINT CHK_PasswordComplexity
CHECK (
    LEN(Password) > 4 AND LEN(Password) <= 32 AND
    Password LIKE '%[a-z]%' AND
    Password LIKE '%[A-Z]%' AND
    Password LIKE '%[!@#%^&*()]%'
);

-- unique indexes for Customers.
CREATE UNIQUE INDEX UQ_Customers_Username ON Customers(Username);
CREATE UNIQUE INDEX UQ_Customers_Email ON Customers(Email);

-- Table to store product categories
CREATE TABLE ProductCategory (
    CategoryID INT IDENTITY(1, 1) PRIMARY KEY,
    CategoryName NVARCHAR(100) UNIQUE NOT NULL
);

-- Table to store product catalog information
CREATE TABLE ProductCatalog (
    ProductID INT IDENTITY(1, 1) PRIMARY KEY,
    ProductName NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Price SMALLMONEY NOT NULL,
    QuantityInStock INT NOT NULL,
    CategoryID INT NOT NULL,
    
    CONSTRAINT FK_ProductCatalog_CategoryID FOREIGN KEY (CategoryID) 
    REFERENCES ProductCategory(CategoryID)
);

-- Table to store product images
CREATE TABLE ProductImages (
    ImageID INT IDENTITY(1, 1) PRIMARY KEY,
    ImageURL NVARCHAR(MAX) NOT NULL,
    ProductID INT NOT NULL,
    ImageOrder SMALLINT NOT NULL,
    
    CONSTRAINT FK_ProductImages_ProductID 
    FOREIGN KEY (ProductID) 
    REFERENCES ProductCatalog(ProductID)
);

-- Table to store order statuses
CREATE TABLE OrderStatus (
    ID TINYINT IDENTITY(1, 1) PRIMARY KEY,
    Status NVARCHAR(50) UNIQUE NOT NULL
);

-- Table to store orders
CREATE TABLE Orders (
    OrderID INT IDENTITY(1, 1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount SMALLMONEY,
    StatusID TINYINT,
    
    CONSTRAINT FK_Orders_CustomerID FOREIGN KEY (CustomerID) 
    REFERENCES Customers(CustomerID),
    
    CONSTRAINT FK_Orders_StatusID FOREIGN KEY (StatusID) 
    REFERENCES OrderStatus(ID)
);

-- Table to store Customer Reviews
CREATE TABLE Reviews (
    ReviewID INT IDENTITY(1, 1) PRIMARY KEY,
    ProductID INT NOT NULL,
    CustomerID INT NOT NULL,
    ReviewText NVARCHAR(255) NOT NULL,
    Rating DECIMAL(2, 1) NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Review_Product FOREIGN KEY (ProductID) 
    REFERENCES ProductCatalog(ProductID),
    
    CONSTRAINT FK_Review_Customer FOREIGN KEY (CustomerID) 
    REFERENCES Customers(CustomerID)
);

-- Table to store order items
CREATE TABLE OrderItems (
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL CHECK(Quantity >= 1),
    Price SMALLMONEY NOT NULL,
    TotalItemsPrice SMALLMONEY NOT NULL,
    
    CONSTRAINT PK_OrderItems PRIMARY KEY (OrderID, ProductID),  -- Composite primary key
    
    CONSTRAINT FK_OrderItems_OrderID FOREIGN KEY (OrderID) 
    REFERENCES Orders(OrderID),
    
    CONSTRAINT FK_OrderItems_ProductID FOREIGN KEY (ProductID) 
    REFERENCES ProductCatalog(ProductID)
);

-- Table to store payments
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1, 1) PRIMARY KEY,
    OrderID INT NOT NULL,
    Amount SMALLMONEY NOT NULL,
    PaymentMethod NVARCHAR(255) NOT NULL,
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Payments_OrderID FOREIGN KEY (OrderID) 
    REFERENCES Orders(OrderID)
);

-- Create a unique index for Payments.
CREATE UNIQUE INDEX UQ_Payments_OrderID ON Payments(OrderID);


-- Table to store shippings
CREATE TABLE Shippings (
    ShippingID INT IDENTITY(1, 1) PRIMARY KEY,
    OrderID INT NOT NULL,
    CarrierName NVARCHAR(255) NOT NULL,
    TrackingNumber NCHAR(255) NOT NULL,
    ShippingStatusID TINYINT NOT NULL,
    EstimatedDeliveryDate DATETIME NOT NULL DEFAULT DATEADD(DAY, 7, GETDATE()),
    ActualDeliveryDate DATETIME,
    
    CONSTRAINT FK_Shippings_OrderID FOREIGN KEY (OrderID) 
    REFERENCES Orders(OrderID),
    
    CONSTRAINT FK_Shippings_ShippingStatusID FOREIGN KEY (ShippingStatusID) 
    REFERENCES OrderStatus(ID)
);
