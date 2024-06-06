-- Test Scripts for Database Verification
-- Important Note: Execute Each Script Separately to Prevent Errors; Avoid Running All at Once Take Care Bro
-- ..........................................................................................................................

-- Example to insert an Instructor with Qualifications
BEGIN TRANSACTION; -- Start a transaction
-- Insert a new Person
INSERT INTO Persons (Name, DateOfBirth, Gender, Phone, Email, Address)
VALUES ('Ali Emad', '1980-01-15', 'M', '01123456789', 'karate.master@example.com', '456 Elm St');

-- Get the PersonId of the newly inserted Person
DECLARE @NewPersonId INT;
SELECT @NewPersonId = SCOPE_IDENTITY();

-- Insert a new Instructor with the corresponding PersonId and Qualifications
INSERT INTO Instructors(PersonId, Qualifications)
VALUES (@NewPersonId, '5th Dan Black Belt in Karate, Certified Karate Instructor, 15+ years of teaching experience'); 

COMMIT; -- Commit the transaction if both inserts are successful

-- ..........................................................................................................................

-- Example to insert a Member and Generate his own Subscription Periods
BEGIN TRANSACTION; -- Start a transaction

-- Insert a new Person
INSERT INTO Persons (Name, DateOfBirth, Gender ,Phone, Email, Address)
VALUES ('Mahmoud Mohamed', '1998-03-03', 'M', '01019060452', 'Mahmoud@example.com', '123 Main St');

-- Get the PersonId of the newly inserted Person
DECLARE @NewPersonId INT;
SELECT @NewPersonId = SCOPE_IDENTITY();

-- Insert a new Member with the corresponding PersonId
-- Assuming the Member is a Beginner so he will has a 'White Belt'
DECLARE @BeltRank INT;
SELECT @BeltRank = (SELECT RankID from BeltRanks WHERE RankName = 'White Belt');
INSERT INTO Members(PersonId, LastBeltRank, IsActive)
VALUES (@NewPersonId, @BeltRank, 1); 

-- Get the Inserted Member's ID
DECLARE @MemberID INT;
SELECT @MemberID = Members.MemberID 
FROM Members 
WHERE Members.PersonId = @NewPersonId;


-- Assign an instructor
DECLARE @instructor INT;

-- Get the instructor's ID
SELECT @instructor = Instructors.InstructorID
FROM Instructors
JOIN Persons ON Persons.PersonId = Instructors.PersonId
WHERE Persons.Name = 'Ali Emad';

-- Insert the instructor-member relationship into the MemberInstructors table
INSERT INTO MemberInstructors (MemberID, InstructorID, AssignDate)
VALUES (@MemberID, @instructor, GETDATE());

-- Insert a new Payment record
DECLARE @PaymentID INT;
INSERT INTO Payments (Amount, Date, MemberID)
VALUES (100, GETDATE(), @MemberID);

-- Get the PaymentID of the newly inserted Payment
SELECT @PaymentID = SCOPE_IDENTITY();

-- Insert a new SubscriptionPeriod for the member
INSERT INTO SubscriptionPeriods (StartDate, Fees, MemberID, PaymentID)
VALUES (GETDATE(), 100, @MemberID, @PaymentID);

COMMIT; -- Commit the transaction if all inserts are successful

-----------------------------------------------------------------------------------------------------

-- Insert a Belt Test record
BEGIN TRANSACTION; -- Start a transaction

-- Get The Member
DECLARE @MemberID INT;
SELECT @MemberID = Members.MemberID from Members
JOIN Persons ON Persons.PersonId = Members.PersonId
WHERE Persons.Name = 'Mahmoud Mohamed';

-- Get The Rank
DECLARE @RankID INT;
SELECT @RankID = BeltRanks.RankID from BeltRanks
WHERE BeltRanks.RankName = 'Black Belt (1st Dan)';

-- Get the instructor's ID
DECLARE @instructor INT;
SELECT @instructor = Instructors.InstructorID
FROM Instructors
JOIN Persons ON Persons.PersonId = Instructors.PersonId
WHERE Persons.Name = 'Ali Emad';

-- Insert a new Payment record
-- first get the Payment amount 
DECLARE @PaymentAmount DECIMAL;
SELECT @PaymentAmount = TestFees fROM BeltRanks Where BeltRanks.RankID = @RankID

DECLARE @PaymentID INT;
INSERT INTO Payments (Amount, Date, MemberID)
VALUES (@PaymentAmount, GETDATE(), @MemberID);

-- Get the PaymentID of the newly inserted Payment
SELECT @PaymentID = SCOPE_IDENTITY();

INSERT INTO BeltTests (MemberID, RankID, Result, TestedByInstructorID, PaymentID)
VALUES (@MemberID, @RankID, Null, @instructor, @PaymentID);
COMMIT; -- Commit the transaction if all inserts are successful
-----------------------------------------------------------------------------------------------------


Select * from  Persons
Select * from  Members
Select * from  Instructors
Select * from  MemberInstructors
Select * from  SubscriptionPeriods
Select * from  Payments
Select * from  BeltRanks
Select * from  BeltTests








Delete from Persons;
Delete from Members
Delete from Instructors
Delete from  MemberInstructors
Delete from  SubscriptionPeriods
Delete from  Payments
Delete from  BeltTests

