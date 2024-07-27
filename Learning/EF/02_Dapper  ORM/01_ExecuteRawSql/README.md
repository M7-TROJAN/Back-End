# Executing Raw SQL Using Dapper

This guide demonstrates how to execute raw SQL queries using Dapper in a C# console application. The example includes reading configuration settings from an `appsettings.json` file, establishing a database connection, and executing SQL queries with and without parameters.

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

namespace ExecuteRawSqlWithDapper
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
            IDbConnection dbConnection = new SqlConnection(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);

            // Define the raw SQL query to select all records from the Wallets table
            var sqlQuery = "SELECT * FROM WALLETS";

            Console.WriteLine("\n---------------- using Dynamic Query -------------");

            // Execute the query and get results as a dynamic object
            var resultAsDynamic = dbConnection.Query(sqlQuery); // Returns IEnumerable<dynamic>

            // Loop through the dynamic results and print each row
            foreach (var row in resultAsDynamic)
            {
                Console.WriteLine(row);  
            }

            Console.WriteLine("\n---------------- using Strongly Typed Query -------------");

            // Execute the query and get results as strongly typed Wallet objects
            var wallets = dbConnection.Query<Wallet>(sqlQuery); // Returns IEnumerable<Wallet>

            // Loop through the Wallet results and print each wallet
            foreach (var wallet in wallets)
            {
                Console.WriteLine(wallet); 
            }

            Console.WriteLine("\n---------------- using Strongly Typed Query with Parameters -------------");

            // Define query parameters
            var parameters = new { Id = 1 };

            // Execute the query with parameters and get results as strongly typed Wallet objects
            var wallets2 = dbConnection.Query<Wallet>("SELECT * FROM WALLETS WHERE ID = @Id", parameters);

            // Loop through the Wallet results and print each wallet
            foreach (var wallet in wallets2)
            {
                Console.WriteLine(wallet);
            }
            
            // No need to explicitly close the connection, Dapper handles it
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
    IDbConnection dbConnection = new SqlConnection(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
    ```
    - This creates a `SqlConnection` object using the connection string from the configuration.

3. **Executing Dynamic Query**:
    ```csharp
    var resultAsDynamic = dbConnection.Query(sqlQuery);
    ```
    - This executes the SQL query and returns results as a dynamic object.

4. **Executing Strongly Typed Query**:
    ```csharp
    var wallets = dbConnection.Query<Wallet>(sqlQuery);
    ```
    - This executes the SQL query and returns results as strongly typed `Wallet` objects.

5. **Executing Strongly Typed Query with Parameters**:
    ```csharp
    var parameters = new { Id = 1 };
    var wallets2 = dbConnection.Query<Wallet>("SELECT * FROM WALLETS WHERE ID = @Id", parameters);
    ```
    - This executes the SQL query with parameters and returns results as strongly typed `Wallet` objects.

6. **No Explicit Connection Close**:
    - There's no need to explicitly close the connection as Dapper handles it automatically.