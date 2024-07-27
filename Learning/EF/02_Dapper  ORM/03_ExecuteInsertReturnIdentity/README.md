# Executing INSERT Operation with Identity Retrieval Using Dapper

This guide demonstrates how to execute an INSERT operation using Dapper and retrieve the identity of the newly inserted record in a C# console application.

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

Here is the complete code with detailed comments explaining each step:

```csharp
using Dapper; // Importing the Dapper namespace for ORM operations
using Microsoft.Data.SqlClient; // Importing the SqlClient namespace for database operations
using Microsoft.Extensions.Configuration; // Importing the IConfiguration namespace for configuration settings
using System; // Importing the System namespace for basic functionalities
using System.Data; // Importing the Data namespace for database-related enumerations

namespace ExecuteInsertReturnIdentity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Create a SqlConnection object using the connection string from the configuration
            using IDbConnection dbConnection = new SqlConnection(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);

            // Define a Wallet object to be inserted
            var walletToInsert = new Wallet { Holder = "Mansour", Balance = 7400m };

            // Define the SQL query for the INSERT operation with identity retrieval
            var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance); SELECT CAST(SCOPE_IDENTITY() as INT)";

            // Define parameters for the SQL query
            var parameters = new { Holder = walletToInsert.Holder, Balance = walletToInsert.Balance };

            // Execute the INSERT operation and retrieve the identity of the inserted record
            walletToInsert.Id = dbConnection.QuerySingle<int>(sqlQuery, parameters);

            // Check if the insertion was successful and print the result
            if (walletToInsert.Id > 0)
            {
                Console.WriteLine("Wallet inserted successfully!");
                Console.WriteLine($"Wallet Id: {walletToInsert.Id}");
            }
            else
            {
                Console.WriteLine("Wallet not inserted!");
            }
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
    var walletToInsert = new Wallet { Holder = "Mansour", Balance = 7400m };
    ```
    - This defines a `Wallet` object with the values to be inserted into the database.

4. **Defining SQL Query with Identity Retrieval**:
    ```csharp
    var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance); SELECT CAST(SCOPE_IDENTITY() as INT)";
    ```
    - This specifies the SQL query for the INSERT operation and includes `SCOPE_IDENTITY()` to retrieve the identity of the newly inserted record.

5. **Executing the Query and Retrieving Identity**:
    ```csharp
    walletToInsert.Id = dbConnection.QuerySingle<int>(sqlQuery, parameters);
    ```
    - This executes the INSERT operation and retrieves the identity of the inserted record.

6. **Checking Insertion Result**:
    ```csharp
    if (walletToInsert.Id > 0)
    {
        Console.WriteLine("Wallet inserted successfully!");
        Console.WriteLine($"Wallet Id: {walletToInsert.Id}");
    }
    else
    {
        Console.WriteLine("Wallet not inserted!");
    }
    ```
    - This checks if the insertion was successful by evaluating if the `Id` is greater than 0 and prints the result.
