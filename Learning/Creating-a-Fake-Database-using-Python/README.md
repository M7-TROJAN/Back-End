# Creating a Synthetic Database using Python's Faker Library

This guide illustrates how to utilize Python in conjunction with the Faker library to produce synthetic data and save it in both a CSV file and an SQLite database.

## Prerequisites
1. Ensure you have Python installed on your system.
2. To install the Faker library, use the following command:
    ```sh
    pip install faker
    ```

## Overview

Creating synthetic data is a fundamental practice in database management, software testing, and learning SQL. Python's Faker library allows you to generate artificial data that closely resembles real-world information. This README demonstrates the creation of synthetic data in both a CSV file and an SQLite database, providing essential hands-on experience for anyone learning database operations.

## Instructions

### Generate Synthetic Data in a CSV File

Use the Faker library and Python's CSV module to fabricate mock data and store it in a CSV file.

#### Code Example:

```python
from faker import Faker
import csv

fake = Faker()

# Generate synthetic data and write to a CSV file
with open('fake_data.csv', 'w', newline='', encoding='utf-8') as csvfile:
    fieldnames = ['FirstName', 'LastName', 'Email', 'Address']
    writer = csv.DictWriter(csvfile, fieldnames=fieldnames)

    writer.writeheader()
    for _ in range(1000):  # Adjust the range for the number of entries
        writer.writerow({
            'FirstName': fake.first_name(),
            'LastName': fake.last_name(),
            'Email': fake.email(),
            'Address': fake.address()
        })
```

## Create a Synthetic Database in SQLite
Utilize the Faker library in combination with the sqlite3 module to generate synthetic data and store it in an SQLite database.

### Code Example:

```python
from faker import Faker
import sqlite3

fake = Faker()

# Establish a connection with an SQLite database
connection = sqlite3.connect('LargeDatabase.sqlite')
cursor = connection.cursor()

# Create a table and insert synthetic data
cursor.execute('''
    CREATE TABLE Users (
        UserID INTEGER PRIMARY KEY,
        FirstName TEXT,
        LastName TEXT,
        Email TEXT,
        Address TEXT
    )
''')

# Insert synthetic data into the "Users" table
for _ in range(1000):  # Adjust the range for the number of entries
    cursor.execute('''
        INSERT INTO Users (FirstName, LastName, Email, Address) VALUES (?, ?, ?, ?)
    ''', (fake.first_name(), fake.last_name(), fake.email(), fake.address()))

# Commit changes and close the connection
connection.commit()
connection.close()
```

### Faker Samples
For more examples and a consolidated view of various synthetic data generation methods using Faker, explore the [Faker Samples script](faker_data_samples.py). This script provides additional code snippets that demonstrate a wide range of Faker's capabilities in generating mock data.

## How synthetic databases helped me

SSynthetic Databases it's serves as a test environment for real-world scenarios.
Mock Databases have played a pivotal role in my learning journey, allowing me to practically apply all the concepts I've learned in database materials. They have provided me with the opportunity to experiment with various scenarios and extensively test my understanding of the concepts covered in my database learning. The practice of using synthetic databases has significantly enhanced my ability to grasp SQL operations and refine my skills through practical applications. This approach has not only allowed me to apply theoretical knowledge but also to gain hands-on experience, improving my confidence and proficiency in managing databases.

## Importing Synthetic Data to SQL Server

Generated synthetic data in CSV format can be imported into SQL Server Management Studio. Follow the import process to populate tables in SQL Server databases with the synthetic data created from the CSV file.

### Steps for Importing Data:

1. **Open SQL Server Management Studio (SSMS)** and connect to the target database where you want to import the synthetic data.

2. **Right-click on the database name** in the Object Explorer pane, select `Tasks`, and then click on `Import Data...`.

3. In the SQL Server Import and Export Wizard:

    - **Data Source:** Select `Flat File Source` to specify your CSV file as the source.
    - **File Name:** Browse and choose the generated `fake_data.csv`.
    - **Format:** Ensure the format settings match your CSV file (e.g., delimited or fixed width).
    - **Column Heading Rows:** Check if the first row contains column headings.

4. **Choose a Destination:** Select `SQL Server Native Client 11.0` or appropriate ODBC provider as the destination.

5. **Server Name:** Enter the server name where your SQL Server is hosted.

6. **Database:** Pick the database where you want to import the data.

7. **Copy Data:** Choose whether to copy data to an existing table or create a new table.

8. **Map Source and Destination Columns:** Match CSV columns with the corresponding database table columns.

9. **Run the Import:** Review the summary and run the import process.

10. **Verify Data Import:** Check your SQL Server database to ensure the data has been successfully imported into the specified table.

The imported synthetic data will now be available in your SQL Server database tables, providing a test environment for performing SQL operations and analysis.

## Sources
- **Faker Documentation**: https://faker.readthedocs.io/en/master/

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
