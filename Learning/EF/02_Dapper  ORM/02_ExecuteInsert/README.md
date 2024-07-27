# Executing INSERT Operation Using Dapper

This guide demonstrates how to execute an INSERT operation using Dapper in a C# console application. The example includes reading configuration settings from an `appsettings.json` file, establishing a database connection, and executing an INSERT SQL query with parameters.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Dapper library (can be installed via NuGet)

## Step-by-Step Guide

### 1. Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `appsettings.json`

### 2. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=M7_TROJAN;Database=DigitalCurrency;Integrated Security=SSPI;TrustServerCertificate=True"
  }
}
```

### 3. C# Code Explanation

Here's the complete code with detailed comments explaining each step:

```csharp
using Dapper; // Importing the Dapper namespace for ORM operations
using Microsoft.Data.SqlClient; // Importing the SqlClient namespace for database operations
using Microsoft.Extensions.Configuration; // Importing the IConfiguration namespace for configuration settings
using System; // Importing the System namespace for basic functionalities
using System.Data; // Importing the Data namespace for database-related enumerations

namespace ExecuteInsertWithDapper
{
    internal class Program
    {
        static void Main()
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Create a SqlConnection object using the connection string from the configuration
            using IDbConnection dbConnection = new SqlConnection(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);

            // Define a Wallet object to be inserted
            var walletToInsert = new Wallet { Holder = "Sarah", Balance = 7400m };

            // Define the SQL query for the INSERT operation
            var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance)";

            // Define parameters for the SQL query
            var parameters = new { Holder = walletToInsert.Holder, Balance = walletToInsert.Balance };

            // Execute the INSERT operation and get the number of affected rows
            var affectedRows = dbConnection.Execute(sqlQuery, parameters);

            // Print the number of affected rows to the console
            Console.WriteLine($"Affected Rows: {affectedRows}");

            // Wait for user input before closing the console
            Console.ReadKey();
        }
    }

    // Define a Wallet class to represent data from the Wallets table
    public class Wallet
    {
        public int Id { get; set; } // Property to store the wallet ID
        public string? Holder { get; set; } // Property to store the wallet holder's name
        public decimal? Balance { get; set; } // Property to store the wallet balance

        // Override the ToString method to provide a string representation of the Wallet object
        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

### Explanation of Key Points

1. **Configuration Setup**:
    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - This sets up the configuration to read settings from `appsettings.json`.

2. **Database Connection**:
    ```csharp
    using IDbConnection dbConnection = new SqlConnection(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
    ```
    - This creates a `SqlConnection` object using the connection string from the configuration. The `using` statement ensures proper disposal of the connection.

3. **Creating Wallet Object**:
    ```csharp
    var walletToInsert = new Wallet { Holder = "Sarah", Balance = 7400m };
    ```
    - This defines a `Wallet` object with the values to be inserted into the database.

4. **Defining SQL Query and Parameters**:
    ```csharp
    var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance)";
    var parameters = new { Holder = walletToInsert.Holder, Balance = walletToInsert.Balance };
    ```
    - This specifies the SQL query for the INSERT operation and defines the parameters to be used in the query.

5. **Executing the Query**:
    ```csharp
    var affectedRows = dbConnection.Execute(sqlQuery, parameters);
    ```
    - This executes the INSERT operation and returns the number of affected rows.

6. **Outputting Results**:
    ```csharp
    Console.WriteLine($"Affected Rows: {affectedRows}");
    Console.ReadKey();
    ```
    - This prints the number of affected rows to the console and waits for user input before closing the console.
