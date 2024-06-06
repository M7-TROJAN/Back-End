use master;

create database DT_Library;

USE DT_Library;

-- Create the Users table for storing User details
CREATE TABLE Users (
    UserID int identity(1,1) primary key,
	UserName NVARCHAR(50) UNIQUE not null,
	Phone char(11) check(isNumeric(phone) = 1),
	Email NVARCHAR(100) CHECK (
    Email LIKE '%@%' -- Check if it contains the "@" symbol
    AND Email LIKE '%.%' -- Check if it contains a dot (.) for the domain
    AND Email NOT LIKE '%@%@%' -- Ensure there's only one "@" symbol
    AND Email NOT LIKE '%.%.' -- Ensure there's only one dot (.) for the domain
    AND LEN(Email) <= 100 AND LEN(Email) >= 5 -- -- Maximum and Minimum total length for an email address
    AND CHARINDEX('@', Email) <= LEN(Email) - CHARINDEX('.', REVERSE(Email)) - 1 -- Ensure the "@" is before the last "."
    AND CHARINDEX('@', Email) >= 2 -- Ensure the "@" is not at the beginning
    AND CHARINDEX('.', REVERSE(Email)) >= 2 -- Ensure the "." is not at the end
),
Address nvarchar(200)
);

-- Create the LibraryCards table for storing Library Card details
CREATE TABLE LibraryCards (
    CardID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key reference to Users
    StartDate DATE NOT NULL Default GETDATE(), -- Start date of the library card
    EndDate DATE NOT NULL Default DATEADD(YEAR, 1, GETDATE()), -- End date is one year from the start date
    CONSTRAINT FK_LibraryCards_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID)
);


-- Create the Books table for storing Book details
CREATE TABLE Books (
    ID int identity(1,1) primary key,
	Title NVARCHAR(100) UNIQUE not null,
	ISBN NVARCHAR(25) UNIQUE,
	PublicationDate Date not null,  -- PublicationDate -> تاريخ النشر
	Genre NVARCHAR(50), -- Genre -> النوع
	AdditionalDetails nvarchar(MAX)
);

-- Create the Authors table for storing Author details
CREATE TABLE Authors (
ID int identity(1,1) primary key,
Name NVARCHAR(50) UNIQUE not null,
DateOfBirth DATE
);

-- Create the AuthorsBooks table for storing AuthorBooks details
CREATE TABLE AuthorsBooks (
    ID int identity(1,1) primary key,
    AuthorID int not null,
    BookID int not null,
    CONSTRAINT FK_AuthorsBooks_AuthorID FOREIGN KEY (AuthorID) REFERENCES Authors(ID),
    CONSTRAINT FK_AuthorsBooks_BookID FOREIGN KEY (BookID) REFERENCES Books(ID)
);

-- Create the BookCopies table for storing BookCopies details
CREATE TABLE BookCopies (
CopyID int identity(1,1) primary key,
BookID int NOT NULL,
AvailabilityStatus BIT NOT NULL
CONSTRAINT FK_BookCopies_BookID FOREIGN KEY (BookID) REFERENCES Books(ID)
);

-- Create the Reservations table for storing Reservation details
CREATE TABLE Reservations (
ID int identity(1,1) primary key,
UserID int not null,
CopyID int not null,
ReservationDate Date not null, --  ReservationDate -> تاريخ الحجز
CONSTRAINT FK_Reservations_USERID FOREIGN KEY (UserID) REFERENCES Users(UserID),
CONSTRAINT FK_Reservations_CopyID FOREIGN KEY (CopyID) REFERENCES BookCopies(CopyID)
);

-- Create the BorrowingRecords table for storing Borrowing details
CREATE TABLE BorrowingRecords (
    BorrowingRecordID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    CopyID INT NOT NULL,
    BorrowingDate DATE NOT NULL, -- Date when the book was borrowed (تاريخ الاقتراض)
    DueDate DATE NOT NULL CHECK (DueDate >= BorrowingDate), -- Ensure DueDate is greater than or equal to BorrowingDate. (DueDate Means تاريخ الاستحقاق)
    ReturnDate DATE, -- Actual return date of the book
    IsOverdue BIT, -- Overdue status: 1 indicates the customer is late, 0 indicates the customer is not late
    CONSTRAINT FK_BorrowingRecords_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_BorrowingRecords_CopyID FOREIGN KEY (CopyID) REFERENCES BookCopies(CopyID)
);

-- Create the Fines table for storing Fines details
CREATE TABLE Fines (
    FinesID int identity(1,1) primary key,
    UserID int not null,
    BorrowingRecordID int not null,
    NumberOfLateDays smallint not null,
    FineAmount decimal not null,
    PaymentStatus bit,
    CONSTRAINT FK_Fines_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Fines_BorrowingRecordID FOREIGN KEY (BorrowingRecordID) REFERENCES BorrowingRecords(BorrowingRecordID)
);

-- Create a table to store library settings
CREATE TABLE LibrarySettings (
    -- Number of default borrowing days for library materials
    DefaultBorrowingDays TINYINT,

    -- Default fine per day for overdue materials
    DefaultFinePerDay DECIMAL(5, 2)
);
