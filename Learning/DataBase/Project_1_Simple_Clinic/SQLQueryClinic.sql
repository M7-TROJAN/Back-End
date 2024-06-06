-- Create the Clinic database and set it as the current database
CREATE DATABASE Clinic;
USE Clinic;

-- Create the Persons table for storing person details
CREATE TABLE Persons (
    PersonId INT IDENTITY(1, 1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL, -- Person's name
    DateOfBirth DATE CHECK (DateOfBirth <= GETDATE()), -- Restrict future dates
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- Restrict to 'M' or 'F'
    Phone CHAR(11) CHECK (ISNUMERIC(Phone) = 1), -- Ensure it's numeric
    Email NVARCHAR(200) CHECK (Email LIKE '%@%'), -- Basic email format check
    Address NVARCHAR(200),
    CONSTRAINT UC_Name UNIQUE (Name), -- Ensure Name is unique
    -- Ensure that each record has valid contact information (either a non-null phone or email)
    CONSTRAINT CHK_ContactInformation CHECK ((Phone IS NOT NULL) OR (Email IS NOT NULL))
);

-- Create the Doctors table with a foreign key constraint to Persons
CREATE TABLE Doctors (
    DoctorId INT IDENTITY(1, 1) PRIMARY KEY,
    PersonId INT UNIQUE NOT NULL, -- Foreign key reference to Persons table's PersonId.
	Specialization nvarchar(50) not null,
    CONSTRAINT FK_Doctor_Person FOREIGN KEY (PersonId) REFERENCES Persons(PersonId)
);

-- Create the Patients table with a foreign key constraint to Persons
CREATE TABLE Patients (
    PatientId INT IDENTITY(1, 1) PRIMARY KEY,
    PersonId INT UNIQUE NOT NULL, -- Foreign key reference to Persons table's PersonId.
    CONSTRAINT FK_Patient_Person FOREIGN KEY (PersonId) REFERENCES Persons(PersonId)
);

-- Create the Appointment_Status table to track appointment statuses
CREATE TABLE Appointment_Status (
    AppointmentID INT PRIMARY KEY,
    Status_Description NVARCHAR(20) NOT NULL -- Description of appointment status
);

-- Populate the Appointment_Status table with status values
INSERT INTO Appointment_Status (AppointmentID, Status_Description)
VALUES
    (1, 'Pending'),
    (2, 'Confirmed'),
    (3, 'Completed'),
    (4, 'Canceled'),
    (5, 'Rescheduled'),
    (6, 'No Show');



-- Create the Payments table to store payment information
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1, 1) PRIMARY KEY,
    Payment_Date DATETIME NOT NULL, -- Date of payment
    PaymentMethod NVARCHAR(50) NOT NULL, -- Payment method
    AmountPaid DECIMAL NOT NULL, -- Amount paid
    AdditionalNotes NVARCHAR(200) DEFAULT 'None' -- Additional payment notes
);

-- Create the Medical_Records table for recording medical information
CREATE TABLE Medical_Records (
    MedicalRecordID INT IDENTITY(1, 1) PRIMARY KEY,
    VisitDescription NVARCHAR(200), -- Description of the medical visit
    Diagnosis NVARCHAR(200), -- Medical diagnosis
    AdditionalNotes NVARCHAR(200) -- Additional medical notes
);

-- Create the Appointments table with foreign key constraints
CREATE TABLE Appointments (
    AppointmentId INT IDENTITY(1, 1) PRIMARY KEY,
    DoctorId INT not null, -- Foreign key reference to Doctors table's DoctorId.
    PatientId INT not null, -- Foreign key reference to Patients table's PatientId.
    AppointmentDateTime DATETIME NOT NULL, -- Date and time of the appointment
    Appointment_Statu INT, -- Foreign key reference to Appointment_Status table's AppointmentID.
    PaymentID INT not null, -- Foreign key reference to Payments table's PaymentID.
	MedicalRecord_ID INT, -- Foreign key reference to Medical_Records table's MedicalRecordID.
    CONSTRAINT FK_Doctor_Appointment FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId),
    CONSTRAINT FK_Patient_Appointment FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),
    CONSTRAINT FK_Appointment_Status FOREIGN KEY (Appointment_Statu) REFERENCES Appointment_Status(AppointmentID),
    CONSTRAINT FK_Payment FOREIGN KEY (PaymentID) REFERENCES Payments(PaymentID),
    CONSTRAINT FK_MedicalRecord FOREIGN KEY (MedicalRecord_ID) REFERENCES Medical_Records(MedicalRecordID)
);


CREATE TABLE Prescriptions (
    PrescriptionID INT IDENTITY(1, 1) PRIMARY KEY, 
    MedicalRecordID INT NOT NULL, -- Foreign key reference to Medical_Records table's MedicalRecordID.
    MedicationName NVARCHAR(100) NOT NULL, -- Name of the prescribed medication (required)
    Dosage NVARCHAR(50), -- Dosage information
    Frequency NVARCHAR(50), -- Frequency of medication intake
    StartDate DATE, -- Start date for the prescription
    EndDate DATE, -- End date for the prescription
    SpecialInstructions NVARCHAR(200), 
    CONSTRAINT FK_MedicalRecord_Prescription FOREIGN KEY (MedicalRecordID) REFERENCES Medical_Records(MedicalRecordID)
);

-- Trigger for inserting a new doctor
CREATE TRIGGER PreventDoctorAsPatient
ON Doctors
FOR INSERT
AS
BEGIN
    -- Check if there is an overlap between the inserted records and existing patients
    IF EXISTS (
        SELECT 1 FROM inserted AS i
        WHERE EXISTS (
            SELECT 1 FROM Patients AS p
            WHERE p.PersonId = i.PersonId
        )
    )
    BEGIN
        -- Output a conflict message and the PersonId values involved
        PRINT 'Conflict Detected:';
        PRINT 'PersonId values in inserted:';
        DECLARE @PersonIdInInserted INT;
        SELECT @PersonIdInInserted = i.PersonId FROM inserted AS i;
        PRINT @PersonIdInInserted;
        PRINT 'PersonId values in Patients:';
        DECLARE @PersonIdInPatients INT;
        SELECT @PersonIdInPatients = p.PersonId FROM Patients AS p;
        PRINT @PersonIdInPatients;

        -- Raise an error and roll back the specific INSERT
        THROW 50000, 'This person is already a patient and cannot be a doctor.', 1;
        ROLLBACK; -- Roll back the specific INSERT
    END;
END;

-- Trigger for inserting a new patient
CREATE TRIGGER PreventPatientAsDoctor
ON Patients
FOR INSERT
AS
BEGIN
    -- Check if there is an overlap between the inserted records and existing doctors
    IF EXISTS (
        SELECT 1 FROM inserted AS i
        WHERE EXISTS (
            SELECT 1 FROM Doctors AS d
            WHERE d.PersonId = i.PersonId
        )
    )
    BEGIN
        -- Output a conflict message and the PersonId values involved
        PRINT 'Conflict Detected:';
        PRINT 'PersonId values in inserted:';
        DECLARE @PersonIdInInserted INT;
        SELECT @PersonIdInInserted = i.PersonId FROM inserted AS i;
        PRINT @PersonIdInInserted;
        PRINT 'PersonId values in Doctors:';
        DECLARE @PersonIdInDoctors INT;
        SELECT @PersonIdInDoctors = d.PersonId FROM Doctors AS d;
        PRINT @PersonIdInDoctors;

        -- Raise an error and roll back the specific INSERT
        THROW 50000, 'This person is already a doctor and cannot be a patient.', 1;
        ROLLBACK; -- Roll back the specific INSERT
    END;
END;



-- If U want to dele a TRIGGER Use The Below Statment
-- DROP TRIGGER TriggerName;

-- Drop Trigger PreventDoctorAsPatient;

-- Drop Trigger PreventPatientAsDoctor;

