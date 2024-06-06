-- Use the 'FakeAccounts' database
USE [FakeAccounts];
GO

-- Clear the plan cache to ensure execution plan recompilation
DBCC FREEPROCCACHE;

-- Enable time and I/O statistics for each query
SET STATISTICS TIME ON;
SET STATISTICS IO ON;

-- Query 1: Count of all records in the Accounts table
SELECT count(*) FROM Accounts;

-- Query 2: Count of records where Gender is 'Female'
SELECT COUNT(*) FROM Accounts WHERE Gender = 'Female';

-- Query 3: Count of records where LastName is 'stone'
SELECT COUNT(*) FROM Accounts WHERE LastName = 'stone';

-- Query 4: Count of records where FirstName is 'Alex'
SELECT COUNT(*) FROM Accounts WHERE FirstName = 'Alex';

-- Query 5: Count of records with BirthDate between '1998-01-01' and '1998-03-30'
SELECT COUNT(*) FROM Accounts WHERE BirthDate BETWEEN '1998-01-01' AND '1998-03-30';

-- Query 6: Count of records where FirstName contains 'ee'
SELECT COUNT(*) FROM Accounts WHERE FirstName LIKE '%ee%';

-- Query 7: Count of records where UserName starts with 'a' and has 's' as the second character
SELECT COUNT(*) FROM Accounts WHERE UserName LIKE 'a%s';

-- Query 8: Count of records where LastName is 'stone', 'Oliver', or 'Mark'
SELECT COUNT(*) FROM Accounts WHERE LastName in ('stone', 'Oliver', 'Mark');

-- Query 9: Count of records with BirthDate as '2000-01-01'
SELECT COUNT(*) FROM Accounts WHERE BirthDate = '2000-01-01';

-- Query 10: Count of records where AccountType is 'Group'
SELECT COUNT(*) FROM Accounts WHERE AccountType = 'Group';

-- Query 11: Count of records with TotalFollowers greater than 50000
SELECT COUNT(*) FROM Accounts WHERE TotalFollowers > 50000;

-- Create an index on FirstName to optimize queries on that column
CREATE INDEX Accounts_FirstName_IDX ON Accounts(FirstName);

-- Query 3 After Making the Index on FirstName
SELECT COUNT(*) FROM Accounts WHERE FirstName = 'Alex';

-- Query 12: Count of records where FirstName is 'Alex' and LastName is 'Stone'
SELECT COUNT(*) FROM Accounts WHERE FirstName = 'Alex' AND LastName = 'Stone';

-- Query 13: Count of records where LastName is 'Stone' and FirstName is 'Alex'
SELECT COUNT(*) FROM Accounts WHERE LastName = 'Stone' AND FirstName = 'Alex';

-- Create a multi-column index on Email and AccountType
CREATE INDEX Accounts_Email_AccountType_IDX ON Accounts(Email, AccountType);

-- Query 14: Select Email and AccountType where Email is 'aaroncurtis@example.com' and AccountType is 'Page'
SELECT Email, AccountType FROM Accounts WHERE Email = 'aaroncurtis@example.com' and AccountType = 'Page';

-- Query 15: Select UserName and TotalFollowers where TotalFollowers is 20214
SELECT UserName, TotalFollowers FROM Accounts WHERE TotalFollowers = 20214;
