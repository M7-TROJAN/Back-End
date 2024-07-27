# Execute Delete Operation with Dapper

In this example, we will demonstrate how to perform a delete operation using Dapper in C#. The code snippet deletes a record from the `Wallets` table based on a specified ID.

## Code Example

```csharp
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace _05_ExecuteDelete
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
                // Define the SQL query to delete the record
                var sqlQuery = "DELETE FROM Wallets WHERE Id = @Id";

                // Define the parameters for the query
                var parameters = new { Id = 1 };

                // Execute the SQL query and get the number of rows affected
                var rowsAffected = db.Execute(sqlQuery, parameters);

                // Print the number of rows affected to the console
                Console.WriteLine($"Rows affected: {rowsAffected}");
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
var sqlQuery = "DELETE FROM Wallets WHERE Id = @Id";
```
- Defines the SQL query to delete a record from the `Wallets` table where the `Id` matches a specified value.

### 5. **Parameters**

```csharp
var parameters = new { Id = 1 };
```
- Creates an anonymous object to hold the parameter for the SQL query. The `@Id` placeholder in the query is replaced with this value.

### 6. **Executing the Query**

```csharp
var rowsAffected = db.Execute(sqlQuery, parameters);
```
- Executes the SQL query using Dapper's `Execute` method and retrieves the number of rows affected by the delete operation.

### 7. **Output**

```csharp
Console.WriteLine($"Rows affected: {rowsAffected}");
```
- Prints the number of rows affected by the delete operation to the console.

## Best Practices

1. **Error Handling**:
   Wrap database operations in try-catch blocks to handle potential exceptions.

   ```csharp
   try
   {
       var rowsAffected = db.Execute(sqlQuery, parameters);
       Console.WriteLine($"Rows affected: {rowsAffected}");
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

2. **Transactions**:
   For operations that need to be executed atomically, use transactions.

   ```csharp
   using (var transaction = db.BeginTransaction())
   {
       try
       {
           var rowsAffected = db.Execute(sqlQuery, parameters, transaction);
           transaction.Commit();
           Console.WriteLine($"Rows affected: {rowsAffected}");
       }
       catch (Exception)
       {
           transaction.Rollback();
           throw;
       }
   }
   ```