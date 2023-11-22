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
