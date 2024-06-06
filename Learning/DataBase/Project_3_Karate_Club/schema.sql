Use Master;

CREATE DATABASE Karate_Club;

Use Karate_Club;

-- Create the Persons table for storing person details
CREATE TABLE Persons (
    PersonId INT IDENTITY(1, 1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL, -- Person's name
    DateOfBirth DATE CHECK (DateOfBirth < GETDATE()), -- Restrict future dates
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- Restrict to 'M' or 'F'
    Phone CHAR(11) CHECK (ISNUMERIC(Phone) = 1), -- Ensure it's numeric
    Email NVARCHAR(200) CHECK (Email LIKE '%@%'), -- Basic email format check
    Address NVARCHAR(200),
    CONSTRAINT UC_Name UNIQUE (Name), -- Ensure Name is unique
    -- Ensure that each record has valid contact information (either a non-null phone or email)
    CONSTRAINT CHK_ContactInformation CHECK ((Phone IS NOT NULL) OR (Email IS NOT NULL))
);


-- Create the Instructors table for storing Instructor details
CREATE TABLE Instructors (
	InstructorID INT IDENTITY(1, 1) PRIMARY KEY,
    PersonId INT UNIQUE NOT NULL, -- Foreign key reference to Persons table's PersonId.
    Qualifications NVARCHAR(200), -- Instructor's Qualifications
	CONSTRAINT FK_Instructor_Person FOREIGN KEY (PersonId) REFERENCES Persons(PersonId)
);


-- Create the BeltRanks table for storing BeltRank details
CREATE TABLE BeltRanks (
    RankID INT PRIMARY KEY,
    RankName NVARCHAR(200) UNIQUE NOT NULL,
    TestFees SMALLMONEY NOT NULL,
    BeltRankDescription NVARCHAR(MAX) -- Additional information about the belt rank.
);

-- Populate the BeltRanks table with Rank values and descriptions
INSERT INTO BeltRanks (RankID, RankName, TestFees, BeltRankDescription)
VALUES
    (1, 'White Belt', 5, 'Beginner rank, no prior experience required.'),
    (2, 'Yellow Belt', 10, 'Basic understanding of martial arts techniques.'),
    (3, 'Orange Belt', 15, 'Continued improvement, more advanced techniques introduced.'),
    (4, 'Green Belt', 20, 'Progressing towards intermediate levels of skill and knowledge.'),
    (5, 'Blue Belt', 25, 'Solid understanding of core techniques and principles.'),
    (6, 'Purple Belt', 30, 'Advanced level with mastery of fundamental techniques.'),
    (7, 'Brown Belt', 35, 'Highly skilled, preparing for black belt levels.'),
    (8, 'Black Belt (1st Dan)', 40, 'First-degree black belt, proficiency in martial arts.'),
    (9, 'Black Belt (2nd Dan)', 45, 'Second-degree black belt, greater mastery and expertise.'),
    (10, 'Black Belt (3rd Dan)', 50, 'Third-degree black belt, exceptional proficiency.'),
    (11, 'Black Belt (4th Dan)', 55, 'Fourth-degree black belt, expert level of knowledge and skill.'),
    (12, 'Black Belt (5th Dan)', 60, 'Fifth-degree black belt, considered a master in the art.'),
    (13, 'Black Belt (6th Dan)', 65, 'Sixth-degree black belt, highly respected and knowledgeable.'),
    (14, 'Black Belt (7th Dan)', 70, 'Seventh-degree black belt, mentor and teacher to others.'),
    (15, 'Black Belt (8th Dan)', 75, 'Eighth-degree black belt, world-class expertise.'),
    (16, 'Black Belt (9th Dan)', 80, 'Ninth-degree black belt, legendary status in martial arts.'),
    (17, 'Black Belt (10th Dan)', 85, 'Tenth-degree black belt, highest achievable rank, master level.');



-- Create the Members table for storing Member details
CREATE TABLE Members (
	MemberID INT IDENTITY(1, 1) PRIMARY KEY,
    PersonId INT UNIQUE NOT NULL, -- Foreign key reference to Persons table's PersonId.
    LastBeltRank INT, -- Foreign key reference to BeltRanks table's RankID.
	IsActive Bit NOT NULL,
	CONSTRAINT FK_Member_Person FOREIGN KEY (PersonId) REFERENCES Persons(PersonId),
	CONSTRAINT FK_Member_BeltRank FOREIGN KEY (LastBeltRank) REFERENCES BeltRanks(RankID)
);


-- Create the MemberInstructors table for storing member-instructor relationships
CREATE TABLE MemberInstructors (
    MemberID INT, -- Foreign key reference to the Members table's MemberID.
    InstructorID INT, -- Foreign key reference to the Instructors table's InstructorID.
    AssignDate DATETIME NOT NULL Default GETDATE(),
    CONSTRAINT PK_MemberInstructors PRIMARY KEY (MemberID, InstructorID),
    CONSTRAINT FK_MemberInstructors_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT FK_MemberInstructors_Instructor FOREIGN KEY (InstructorID) REFERENCES Instructors(InstructorID)
);


-- Create the Payments table for storing Payment details
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1, 1) PRIMARY KEY,
    Amount DECIMAL NOT NULL,
    Date DATETIME NOT NULL,
    MemberID INT NOT NULL, -- Foreign key reference to the Members table's MemberID.
    CONSTRAINT FK_Payments_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID)
);

-- Create the SubscriptionPeriods table for storing subscription period details
CREATE TABLE SubscriptionPeriods (
    PeriodID INT IDENTITY(1, 1) PRIMARY KEY,
    StartDate DATE NOT NULL DEFAULT GETDATE(), -- Start date of the SubscriptionPeriod
    EndDate AS DATEADD(YEAR, 1, StartDate) PERSISTED, -- End date is one year from the start date
    Fees SMALLMONEY,
    MemberID INT NOT NULL, -- Foreign key reference to the Members table's MemberID.
    PaymentID INT NOT NULL, -- Foreign key reference to the Payments table's PaymentID.
    CONSTRAINT FK_SubscriptionPeriods_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT FK_SubscriptionPeriods_Payment FOREIGN KEY (PaymentID) REFERENCES Payments(PaymentID)
);



-- Create the BeltTests table for storing belt test details
CREATE TABLE BeltTests (
    TestID INT IDENTITY(1, 1) PRIMARY KEY,
    MemberID INT NOT NULL, -- Foreign key reference to the Members table's MemberID.
    RankID INT NOT NULL, -- Foreign key reference to BeltRanks table's RankID.
    Result BIT,
    Date DATE NOT NULL DEFAULT GETDATE(), -- Current date as the default
    TestedByInstructorID INT NOT NULL, -- Foreign key reference to the Instructors table's InstructorID.
    PaymentID INT NOT NULL, -- Foreign key reference to the Payments table's PaymentID.

    CONSTRAINT FK_BeltTests_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT FK_BeltTests_Rank FOREIGN KEY (RankID) REFERENCES BeltRanks(RankID),
    CONSTRAINT FK_BeltTests_Instructor FOREIGN KEY (TestedByInstructorID) REFERENCES Instructors(InstructorID),
    CONSTRAINT FK_BeltTests_Payment FOREIGN KEY (PaymentID) REFERENCES Payments(PaymentID)
);


-- Create a trigger to prevent a person from being both an instructor and a member
CREATE TRIGGER PreventMemberAsInstructor
ON Instructors
FOR INSERT
AS
BEGIN
    -- Check if there is an overlap between the inserted records and existing members
    IF EXISTS (
        SELECT 1
        FROM inserted AS i
        WHERE EXISTS (
            SELECT 1
            FROM Members AS m
            WHERE m.PersonId = i.PersonId
        )
    )
    BEGIN
        -- Raise an error and roll back the specific INSERT if a person is both an instructor and a member
        THROW 50000, 'A person cannot be both an instructor and a member.', 1;
        ROLLBACK; -- Roll back the specific INSERT
    END;
END;

-- Create a trigger to prevent an instructor from being a member
CREATE TRIGGER PreventInstructorAsMember
ON Members
FOR INSERT
AS
BEGIN
    -- Check if there is an overlap between the inserted records and existing instructors
    IF EXISTS (
        SELECT 1
        FROM inserted AS i
        WHERE EXISTS (
            SELECT 1
            FROM Instructors AS ins
            WHERE ins.PersonId = i.PersonId
        )
    )
    BEGIN
        -- Raise an error and roll back the specific INSERT if an instructor is also a member
        THROW 50000, 'An instructor cannot be a member.', 1;
        ROLLBACK; -- Roll back the specific INSERT
    END;
END;
