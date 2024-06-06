# Clinic Database

This repository contains the SQL scripts for creating and managing the Clinic database. The Clinic database is designed to manage patient and doctor information, appointments, payments, medical records, and prescriptions.

## Table of Contents

- [Database Schema](#database-schema)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Database Schema

The database schema consists of the following tables:

- `Persons`: Stores personal information about individuals.
- `Doctors`: Stores information about doctors, with a one-to-one relationship to `Persons`.
- `Patients`: Stores information about patients, with a one-to-one relationship to `Persons`.
- `Appointment_Status`: Defines appointment statuses.
- `Payments`: Records payment information for appointments.
- `Medical_Records`: Stores medical visit details and diagnoses.
- `Appointments`: Manages appointments between doctors and patients.
- `Prescriptions`: Records prescriptions for patients.

## Getting Started

1. Clone this repository to your local machine.
2. Run the SQL scripts in your preferred database management tool to create the Clinic database and tables.

## Usage

You can use this database to:

- Register patients and doctors.
- Schedule and manage appointments.
- Record medical visits and diagnoses.
- Manage prescriptions and medications.

Feel free to customize and extend the database schema to meet your specific requirements.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀



