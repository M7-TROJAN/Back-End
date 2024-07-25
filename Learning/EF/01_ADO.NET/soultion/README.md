
---

# ExecuteRawSql in ADO.NET

## Introduction

In ADO.NET, executing raw SQL queries allows you to run SQL statements directly against the database. This approach provides flexibility and control over database operations, enabling you to perform complex queries and operations that might not be supported by higher-level abstractions.

## Table of Contents
- [Introduction](#introduction)
- [Setting Up the Environment](#setting-up-the-environment)
- [Executing Raw SQL](#executing-raw-sql)
  - [Using `SqlCommand`](#using-sqlcommand)
  - [Handling Results](#handling-results)
  - [Executing Non-Query Commands](#executing-non-query-commands)
- [Example: Select Query](#example-select-query)
- [Example: Insert, Update, Delete](#example-insert-update-delete)
- [Using Parameters](#using-parameters)
- [Best Practices](#best-practices)

## Setting Up the Environment

Before you begin, make sure you have the necessary environment set up. This includes:
- A .NET project.
- A database to connect to (e.g., SQL Server).
- Necessary NuGet packages (`System.Data.SqlClient`).

### Example `appsettings.json`
Create an `appsettings.json` file to store your database connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
  }
}
```

### Configuration Setup
In your `Program.cs` or startup file, set up the configuration to read from `appsettings.json`:
```csharp
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace ExecuteRawSqlExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // Use the connection string in your methods
        }
    }
}
```

## Executing Raw SQL

### Using `SqlCommand`
To execute raw SQL, you use the `SqlCommand` class. The `SqlCommand` object represents a SQL statement or stored procedure that you can execute against a SQL Server database.

#### Example: Creating a `SqlCommand`
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string sqlQuery = "SELECT * FROM YourTable";
    SqlCommand command = new SqlCommand(sqlQuery, connection);

    // Execute the command and process the results
}
```

### Handling Results
When executing a SQL query, you often need to handle the results. The `SqlDataReader` class provides a way to read a forward-only stream of rows from a SQL Server database.

#### Example: Reading Data with `SqlDataReader`
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string sqlQuery = "SELECT * FROM YourTable";
    SqlCommand command = new SqlCommand(sqlQuery, connection);

    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            // Access data using reader methods
            int id = reader.GetInt32(0); // Get data from the first column
            string name = reader.GetString(1); // Get data from the second column
        }
    }
}
```

### Executing Non-Query Commands
Non-query commands, such as `INSERT`, `UPDATE`, and `DELETE`, do not return rows. Instead, you use the `ExecuteNonQuery` method.

#### Example: Executing a Non-Query Command
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string insertQuery = "INSERT INTO YourTable (Column1, Column2) VALUES (@Value1, @Value2)";
    SqlCommand command = new SqlCommand(insertQuery, connection);
    command.Parameters.AddWithValue("@Value1", "SomeValue");
    command.Parameters.AddWithValue("@Value2", "AnotherValue");

    int rowsAffected = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsAffected} rows inserted.");
}
```

## Example: Select Query
Here's a complete example of executing a SELECT query and reading the results:

```csharp
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ExecuteRawSqlExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM YourTable";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        Console.WriteLine($"ID: {id}, Name: {name}");
                    }
                }
            }
        }
    }
}
```

## Example: Insert, Update, Delete
Here's how to execute `INSERT`, `UPDATE`, and `DELETE` commands:

### Insert Example
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string insertQuery = "INSERT INTO YourTable (Column1, Column2) VALUES (@Value1, @Value2)";
    SqlCommand command = new SqlCommand(insertQuery, connection);
    command.Parameters.AddWithValue("@Value1", "Value1");
    command.Parameters.AddWithValue("@Value2", "Value2");

    int rowsAffected = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsAffected} rows inserted.");
}
```

### Update Example
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string updateQuery = "UPDATE YourTable SET Column1 = @Value1 WHERE Column2 = @Value2";
    SqlCommand command = new SqlCommand(updateQuery, connection);
    command.Parameters.AddWithValue("@Value1", "NewValue1");
    command.Parameters.AddWithValue("@Value2", "Value2");

    int rowsAffected = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsAffected} rows updated.");
}
```

### Delete Example
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string deleteQuery = "DELETE FROM YourTable WHERE Column1 = @Value1";
    SqlCommand command = new SqlCommand(deleteQuery, connection);
    command.Parameters.AddWithValue("@Value1", "Value1");

    int rowsAffected = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsAffected} rows deleted.");
}
```

## Using Parameters
Using parameters in your SQL queries helps prevent SQL injection attacks and improves code readability.

### Example with Parameters
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    string sqlQuery = "SELECT * FROM YourTable WHERE Column1 = @Value1";
    SqlCommand command = new SqlCommand(sqlQuery, connection);
    command.Parameters.AddWithValue("@Value1", "Value1");

    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            Console.WriteLine($"ID: {id}, Name: {name}");
        }
    }
}
```

## Best Practices
- **Use Parameters**: Always use parameters to avoid SQL injection.
- **Handle Exceptions**: Use try-catch blocks to handle exceptions and ensure resources are cleaned up.
- **Use `using` Statements**: Ensure proper disposal of resources like `SqlConnection` and `SqlCommand`.
- **Open Connections Late, Close Early**: Minimize the time that a connection is open to improve performance and scalability.
- **Logging**: Log your SQL queries and execution times to diagnose performance issues.

---

By following these guidelines and examples, you'll be able to effectively use `ExecuteRawSql` in ADO.NET to interact with your database. If you have any questions or need further assistance, feel free to ask!
