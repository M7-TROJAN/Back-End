
# Execute Multiple Queries with Dapper

In this example, we will demonstrate how to execute multiple queries using Dapper in C#. The code snippet executes three separate SQL queries to retrieve the minimum, maximum, and sum of balances from the `Wallets` table.

## Code Example

```csharp
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace _06_ExecuteMultipleQueries
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Create a new SqlConnection object using the connection string
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                // Define the SQL query with multiple statements
                var sqlQuery = "SELECT MIN(Balance) FROM Wallets;" +
                               "SELECT MAX(Balance) FROM Wallets;" +
                               "SELECT SUM(Balance) FROM Wallets;";

                // Execute the query and obtain a SqlMapper.GridReader object
                using (var result = db.QueryMultiple(sqlQuery))
                {
                    // Read the results of the first query
                    var minBalance = result.Read<decimal>().Single();
                    // Read the results of the second query
                    var maxBalance = result.Read<decimal>().Single();
                    // Read the results of the third query
                    var sumBalance = result.Read<decimal>().Single();

                    // Print the results to the console
                    Console.WriteLine($"Min balance: {minBalance}");
                    Console.WriteLine($"Max balance: {maxBalance}");
                    Console.WriteLine($"Sum balance: {sumBalance}");
                }
            }
        }
    }
}
```

## Explanation

### 1. **Configuration Setup**

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
```
- This sets up the configuration to read settings from the `appsettings.json` file.

### 2. **Connection String**

```csharp
var connectionString = configuration.GetConnectionString("DefaultConnection");
```
- Retrieves the connection string named `DefaultConnection` from the configuration.

### 3. **Database Connection**

```csharp
using (IDbConnection db = new SqlConnection(connectionString))
```
- Creates a new `SqlConnection` object to interact with the SQL Server database. The `using` statement ensures that the connection is properly disposed of after use.

### 4. **SQL Query**

```csharp
var sqlQuery = "SELECT MIN(Balance) FROM Wallets;" +
               "SELECT MAX(Balance) FROM Wallets;" +
               "SELECT SUM(Balance) FROM Wallets;";
```
- Defines a SQL query with multiple statements separated by semicolons. Each statement performs a different aggregation operation on the `Wallets` table.

### 5. **Executing Multiple Queries**

```csharp
using (var result = db.QueryMultiple(sqlQuery))
```
- Executes the SQL query using Dapper's `QueryMultiple` method, which returns a `SqlMapper.GridReader` object to read multiple result sets.

### 6. **Reading Results**

```csharp
var minBalance = result.Read<decimal>().Single();
var maxBalance = result.Read<decimal>().Single();
var sumBalance = result.Read<decimal>().Single();
```
- Reads the results of each query using the `Read<decimal>()` method. The `Single()` method is used to retrieve the single result from each result set.

### 7. **Output**

```csharp
Console.WriteLine($"Min balance: {minBalance}");
Console.WriteLine($"Max balance: {maxBalance}");
Console.WriteLine($"Sum balance: {sumBalance}");
```
- Prints the results of the queries to the console.

## Best Practices

1. **Error Handling**:
   Wrap database operations in try-catch blocks to handle potential exceptions.

   ```csharp
   try
   {
       using (var result = db.QueryMultiple(sqlQuery))
       {
           var minBalance = result.Read<decimal>().Single();
           var maxBalance = result.Read<decimal>().Single();
           var sumBalance = result.Read<decimal>().Single();
           Console.WriteLine($"Min balance: {minBalance}");
           Console.WriteLine($"Max balance: {maxBalance}");
           Console.WriteLine($"Sum balance: {sumBalance}");
       }
   }
   catch (SqlException ex)
   {
       Console.WriteLine($"Database error: {ex.Message}");
   }
   catch (Exception ex)
   {
       Console.WriteLine($"An error occurred: {ex.Message}");
   }
   ```

2. **Query Optimization**:
   Combine related queries into a single batch to reduce the number of round trips to the database. Ensure that the SQL statements are optimized for performance.

3. **Transaction Management**:
   Use transactions when executing multiple queries that need to be atomic, ensuring that either all queries succeed or none do.

   ```csharp
   using (var transaction = db.BeginTransaction())
   {
       try
       {
           using (var result = db.QueryMultiple(sqlQuery, transaction: transaction))
           {
               var minBalance = result.Read<decimal>().Single();
               var maxBalance = result.Read<decimal>().Single();
               var sumBalance = result.Read<decimal>().Single();
               Console.WriteLine($"Min balance: {minBalance}");
               Console.WriteLine($"Max balance: {maxBalance}");
               Console.WriteLine($"Sum balance: {sumBalance}");
           }
           transaction.Commit();
       }
       catch (Exception)
       {
           transaction.Rollback();
           throw;
       }
   }
   ```