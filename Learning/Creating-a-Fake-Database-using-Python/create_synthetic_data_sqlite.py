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
