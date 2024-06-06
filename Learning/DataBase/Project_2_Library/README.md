# DT_Library Database

The DT_Library database is designed to manage a library's operations, including user information, book records, borrowing, and more. 
Below is a detailed description of the tables and their relationships.

## Tables

### Users

The `Users` table stores user details.

- `UserID` (Primary Key): Unique identifier for users.
- `UserName`: User's name.
- `Phone`: User's phone number.
- `Email`: User's email address.
- `Address`: User's address.

### LibraryCards

The `LibraryCards` table stores library card details.

- `CardID` (Primary Key): Unique identifier for library cards.
- `UserID` (Foreign Key): Reference to the user associated with the library card.
- `StartDate`: Start date of the library card.
- `EndDate`: End date of the library card.

### Books

The `Books` table stores book details.

- `ID` (Primary Key): Unique identifier for books.
- `Title`: Book title.
- `ISBN`: International Standard Book Number.
- `PublicationDate`: Date of publication.
- `Genre`: Book genre.
- `AdditionalDetails`: Additional information about the book.

### Authors

The `Authors` table stores author details.

- `ID` (Primary Key): Unique identifier for authors.
- `Name`: Author's name.
- `DateOfBirth`: Author's date of birth.

### AuthorsBooks

The `AuthorsBooks` table establishes a many-to-many relationship between authors and books.

- `ID` (Primary Key): Unique identifier for author-book relationships.
- `AuthorID` (Foreign Key): Reference to the author.
- `BookID` (Foreign Key): Reference to the book.

### BookCopies

The `BookCopies` table stores book copy details.

- `CopyID` (Primary Key): Unique identifier for book copies.
- `BookID` (Foreign Key): Reference to the book.
- `AvailabilityStatus`: Indicates the availability of the book copy.

### Reservations

The `Reservations` table stores book reservation details.

- `ID` (Primary Key): Unique identifier for reservations.
- `UserID` (Foreign Key): Reference to the user who made the reservation.
- `CopyID` (Foreign Key): Reference to the reserved book copy.
- `ReservationDate`: Date of the reservation.

### BorrowingRecords

The `BorrowingRecords` table stores book borrowing details.

- `BorrowingRecordID` (Primary Key): Unique identifier for borrowing records.
- `UserID` (Foreign Key): Reference to the user who borrowed the book.
- `CopyID` (Foreign Key): Reference to the borrowed book copy.
- `BorrowingDate`: Date when the book was borrowed.
- `DueDate`: Date when the book is due (must be greater than or equal to `BorrowingDate`).
- `ReturnDate`: Actual return date of the book.
- `IsOverdue`: Overdue status (1 indicates the customer is late, 0 indicates the customer is not late).

### Fines

The `Fines` table stores fine details.

- `FinesID` (Primary Key): Unique identifier for fines.
- `UserID` (Foreign Key): Reference to the user with fines.
- `BorrowingRecordID` (Foreign Key): Reference to the borrowing record associated with the fine.
- `NumberOfLateDays`: The number of days the book was late.
- `FineAmount`: The amount of the fine.
- `PaymentStatus`: Indicates the payment status of the fine.

## Sample Scripts

### Creating a User and Library Card

To insert a new user and generate their library card, use the following script:

```sql
-- Insert a new User
-- INSERT INTO Users (UserName, Phone, Email, Address) VALUES ...

-- Get the UserId of the newly inserted User

-- Insert a new LibraryCard with the corresponding UserId
-- INSERT INTO LibraryCards (UserID, EndDate) VALUES ...
```

## Inserting an Author
To insert an author, use the following script:
```sql
-- Insert an Author
-- INSERT INTO Authors (Name, DateOfBirth) VALUES ...
```

## Inserting a Book
To insert a book, use the following script:
```sql
-- Insert a Book
-- INSERT INTO Books (Title, ISBN, PublicationDate, Genre, AdditionalDetails) VALUES ...
```

## Linking an Author to a Book
To link an author to a book, use the following script:
```sql
-- Link the Author to the Book in AuthorsBooks table
-- INSERT INTO AuthorsBooks (AuthorID, BookID) VALUES ...
```

Borrowing a Book
To perform a borrowing transaction, use the following script:
```sql
-- Insert the record into BorrowingRecords
-- INSERT INTO BorrowingRecords (UserID, CopyID, BorrowingDate, DueDate) VALUES ...

-- Update the BookCopy's availability status to checked out
-- UPDATE BookCopies SET AvailabilityStatus = 0 WHERE ...
```

## Full Real Example
Test Scripts for Database Verification
Important Note: Execute Each Script Separately to Prevent Errors; Avoid Running All at Once Take Care, Bro


- Example to insert a user and Generate his own LibraryCard
```sql

BEGIN TRANSACTION; -- Start a transaction

-- Insert a new User
INSERT INTO Users (UserName, Phone, Email, Address)
VALUES ('Mahmoud Mohamed', '01019060452', 'Mahmoud@example.com', '123 Main St');

-- Get the UserId of the newly inserted User
DECLARE @NewUserId INT;
SELECT @NewUserId = SCOPE_IDENTITY();

-- Insert a new LibraryCard with the corresponding UserId
INSERT INTO LibraryCards (UserID, EndDate)
VALUES (@NewUserId, DATEADD(YEAR, 1, GETDATE())); -- Assuming a 1-year subscription

COMMIT; -- Commit the transaction if both inserts are successful
```
- Example to insert an Author
```sql
INSERT INTO Authors (Name, DateOfBirth)
VALUES ('Taha Hussein', '1889-11-14');
```


- Example to insert a Book
```sql
INSERT INTO Books (Title, ISBN, PublicationDate, Genre, AdditionalDetails)
VALUES ('The Days', '978-1-123456-00-0', '2023-01-01', 'Autobiography', 'An autobiography of a famous author.');

-- Link the Author to the Book in AuthorsBooks table
DECLARE @NewAuthorId INT, @NewBookId INT;
SELECT @NewAuthorId = (SELECT ID FROM Authors WHERE Name = 'Taha Hussein'); -- Get the Author ID
SELECT @NewBookId = (SELECT ID FROM Books WHERE Title = 'The Days'); -- Get the Book ID

INSERT INTO AuthorsBooks (AuthorID, BookID)
VALUES (@NewAuthorId, @NewBookId);
```

- Insert a new Book Copy and link it to the Book
```sql
DECLARE @NewBookId INT;
SELECT @NewBookId = (SELECT ID FROM Books WHERE Title = 'The Days'); -- Get the Book ID
INSERT INTO BookCopies (BookID, AvailabilityStatus)
VALUES (@NewBookId, 1); -- Assuming the initial availability status is 1 (available)

-- Insert another Book Copy of the existing Book and link it to the Book
DECLARE @NewBookId2 INT;
SELECT @NewBookId2 = (SELECT ID FROM Books WHERE Title = 'The Days'); -- Get the Book ID
INSERT INTO BookCopies (BookID, AvailabilityStatus)
VALUES (@NewBookId, 1); -- Assuming the initial availability status is 1 (available)
```

- Example to perform a Borrowing transaction
```sql
-- Assuming you have a LibraryCard for the user and a BookCopy available
DECLARE @UserId INT, @LibraryCardID INT, @BookCopyID INT;
SELECT @UserId = (SELECT Users.UserID FROM Users WHERE Users.UserName = 'Mahmoud Mohamed'); -- Get the User ID
SELECT @BookCopyID = (SELECT Top 1 CopyID FROM BookCopies WHERE AvailabilityStatus = 1);

-- Perform the Borrowing transaction
BEGIN TRANSACTION;

-- Insert the record into BorrowingRecords
INSERT INTO BorrowingRecords (UserID, CopyID, BorrowingDate, DueDate)
VALUES (@UserId, @BookCopyID, GETDATE(), DATEADD(DAY, 7, GETDATE())); -- Assuming 7-day borrowing period

-- Update the BookCopy's availability status to checked out (0 means checked out)
UPDATE BookCopies
SET AvailabilityStatus = 0
WHERE CopyID = @BookCopyID;

COMMIT; -- Commit the transaction if both inserts are successful
```

- Example: Performing a Book Reservation Transaction
```sql
BEGIN TRANSACTION; -- Begin a transaction

-- Assume the user named 'Magdy Khaled' wants to reserve a copy of the book 'The Mystery Of Death' while it's available

-- Get the User ID for 'Magdy Khaled'
DECLARE @UserId INT;
SELECT @UserId = (SELECT Users.UserID FROM Users WHERE Users.UserName = 'Magdy Khaled');

-- Get the Book ID for 'The Mystery Of Death'
DECLARE @BookID INT;
SELECT @BookID = (SELECT Books.ID FROM Books WHERE Books.Title = 'The Mystery Of Deth');

-- Get the Copy ID of the available book copy
DECLARE @BookCopyID INT;
SELECT @BookCopyID = (SELECT TOP 1 BookCopies.CopyID FROM BookCopies WHERE BookCopies.BookID = @BookID);

-- Insert a reservation record
INSERT INTO Reservations (UserID, CopyID, ReservationDate)
VALUES (@UserId, @BookCopyID, GETDATE());

COMMIT; -- Commit the transaction if the insert is successful
```

- Feel free to customize and extend the database schema to meet your specific requirements.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
