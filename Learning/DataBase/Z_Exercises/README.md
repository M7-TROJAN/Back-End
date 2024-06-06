# SQL Practice Problems

Welcome agin! In this collection, you'll find a variety of SQL challenges, each thoughtfully described. These problems have been instrumental in elevating my SQL skills. They cover a wide range of topics, helping me test my knowledge and sharpen my querying abilities. Feel free to explore and use them to enhance your SQL proficiency.

## Getting Started

### Prerequisites

Before you get started, ensure you have a SQL Server ready to go. You'll also need the sample databases stored in the "Sample_Database_To_Practice_On" and "practice-on-northwind-database" folders to work on the problems.

Inside the "Sample_Database_To_Practice_On" folder, you'll find two sub-folders:

1. VehicleMakesDB Folder: This one contains the VehicleMakesDB.bak file. which it serves as a sample database for problems 1 to 50, and there's also a diagram (relational_schema_diagram.png).

2. EmployeesDB Folder: Inside, you'll discover the EmployeesDB.bak file, which serves as a sample database for problems 51 to 54. It includes a diagram (relational_schema_diagram.png) too.

and Inside the "practice-on-northwind-database" folder, you'll find 3 Files:

1. `northwind-Schema.png`: Illustrative diagram of the Northwind database schema.
2. `NorthWindDatbaseScript.sql`: SQL script for creating the Northwind database.
3. `queries.sql`: File containing 50 SQL queries and their corresponding answers for the Northwind database. These queries cover various data retrieval, manipulation, and reporting tasks within the database.

These databases and their 104 queries are where I practiced and improved my SQL skills by tackling problems.
You too can enjoy the learning journey!

### How to Restore the Sample Database

You can restore the sample database using SQL Server Management Studio (SSMS) or the SQL Server command-line tools. Here's how to do it using SSMS:

1. Open SQL Server Management Studio.

2. Connect to your SQL Server instance.

3. Right-click on "Databases" in the Object Explorer.

4. Select "Restore Database."

5. In the "General" section, enter a new database name (e.g., "VehicleMakesDB").

6. In the "Source" section, choose "Device" and select the "VehicleMakesDB.bak" file as the backup media.

7. Click "OK" to start the restoration process.

Or You Can Use This Script:
```sql
RESTORE DATABASE MyDatabase
FROM DISK = 'The Path\MyDatabase.bak'
```

### How to Use the Problems

In the "Problems" directory, you'll find SQL scripts for each problem (e.g., "Problem1.sql," "Problem2.sql," etc.). The problem descriptions are provided as comments at the top of each script.

To work on a problem:

1. Open the SQL script for the problem you want to solve.

2. Read the problem description in the comments.

3. Write your SQL queries to solve the problem.

4. Execute the queries in SQL Server to check your results.

5. Don't be discouraged if you encounter challenges. You can always examine my solution for guidance.

5. Feel free to modify the database to experiment and test different solutions.

## Contributing

If you have additional SQL problems you'd like to contribute or if you find any issues with the existing problems, please feel free to open an issue or pull request on this repository.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
