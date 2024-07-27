# Dapper and Transactions

How to handle transactions using Dapper in C#. The code snippet includes two methods: manual transaction handling and using `TransactionScope`. We will also explore best practices for transaction management.

## Code Example

```csharp
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Transactions;

namespace _07_DapperAndTransactions
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

            // Transfer 2000 from wallet with Id 2 to wallet with Id 4

            // Using Dapper with manual transaction handling
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Open the connection to the database

                using (var trans = connection.BeginTransaction()) // Begin a new transaction
                {
                    try
                    {
                        // Retrieve the wallet with Id 2 within the transaction
                        var walletFrom = connection.QueryFirstOrDefault<Wallet>(
                            "SELECT * FROM Wallets WHERE Id = @Id",
                            new { Id = 2 },
                            transaction: trans
                        );

                        // Retrieve the wallet with Id 4 within the transaction
                        var walletTo = connection.QueryFirstOrDefault<Wallet>(
                            "SELECT * FROM Wallets WHERE Id = @Id",
                            new { Id = 4 },
                            transaction: trans
                        );

                        // Check if both wallets were found
                        if (walletFrom == null || walletTo == null)
                        {
                            throw new Exception("One or both wallets not found.");
                        }

                        // Update balances
                        walletFrom.Balance -= 2000;
                        walletTo.Balance += 2000;

                        // Update the wallet with Id 2
                        connection.Execute(
                            "UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
                            new { Id = walletFrom.Id, Balance = walletFrom.Balance },
                            transaction: trans
                        );

                        // Update the wallet with Id 4
                        connection.Execute(
                            "UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
                            new { Id = walletTo.Id, Balance = walletTo.Balance },
                            transaction: trans
                        );

                        // Commit the transaction
                        trans.Commit();

                        Console.WriteLine("Transaction committed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        trans.Rollback();
                        Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
                    }
                }
            }

            // Using TransactionScope for transaction management
            using (var transactionScope = new TransactionScope())
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection to the database

                    try
                    {
                        // Retrieve the wallet with Id 2
                        var walletFrom = connection.QueryFirstOrDefault<Wallet>(
                            "SELECT * FROM Wallets WHERE Id = @Id",
                            new { Id = 2 }
                        );

                        // Retrieve the wallet with Id 4
                        var walletTo = connection.QueryFirstOrDefault<Wallet>(
                            "SELECT * FROM Wallets WHERE Id = @Id",
                            new { Id = 4 }
                        );

                        // Check if both wallets were found
                        if (walletFrom == null || walletTo == null)
                        {
                            throw new Exception("One or both wallets not found.");
                        }

                        // Update balances
                        walletFrom.Balance -= 2000;
                        walletTo.Balance += 2000;

                        // Update the wallet with Id 2
                        connection.Execute(
                            "UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
                            new { Id = walletFrom.Id, Balance = walletFrom.Balance }
                        );

                        // Update the wallet with Id 4
                        connection.Execute(
                            "UPDATE Wallets SET Balance = @Balance WHERE Id = @Id",
                            new { Id = walletTo.Id, Balance = walletTo.Balance }
                        );

                        // Complete the transaction scope
                        transactionScope.Complete();

                        Console.WriteLine("Transaction committed successfully using TransactionScope.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
                    }
                }
            }
        }
    }

    // Class representing a wallet
    public class Wallet
    {
        public int Id { get; set; } // Wallet Id
        public string? Holder { get; set; } // Wallet holder's name
        public decimal? Balance { get; set; } // Wallet balance

        // Override ToString method for easy printing
        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
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
- Sets up the configuration to read settings from the `appsettings.json` file.

### 2. **Connection String**

```csharp
var connectionString = configuration.GetConnectionString("DefaultConnection");
```
- Retrieves the connection string named `DefaultConnection` from the configuration.

### 3. **Using Manual Transaction Handling**

```csharp
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (var trans = connection.BeginTransaction())
    {
        try
        {
            var walletFrom = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 2 }, transaction: trans);
            var walletTo = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 4 }, transaction: trans);

            if (walletFrom == null || walletTo == null)
            {
                throw new Exception("One or both wallets not found.");
            }

            walletFrom.Balance -= 2000;
            walletTo.Balance += 2000;

            connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletFrom.Id, Balance = walletFrom.Balance }, transaction: trans);
            connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletTo.Id, Balance = walletTo.Balance }, transaction: trans);

            trans.Commit();

            Console.WriteLine("Transaction committed successfully.");
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
        }
    }
}
```
- **Open Connection**: Opens a new `SqlConnection` object to the database.
- **Begin Transaction**: Starts a new transaction using `BeginTransaction()`.
- **Retrieve Wallets**: Fetches wallet records within the transaction scope.
- **Check Records**: Ensures both wallets are found before proceeding.
- **Update Balances**: Adjusts balances and updates the records in the database.
- **Commit/Rollback**: Commits the transaction if successful or rolls back in case of an error.

### 4. **Using TransactionScope**

```csharp
using (var transactionScope = new TransactionScope())
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();

        try
        {
            var walletFrom = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 2 });
            var walletTo = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 4 });

            if (walletFrom == null || walletTo == null)
            {
                throw new Exception("One or both wallets not found.");
            }

            walletFrom.Balance -= 2000;
            walletTo.Balance += 2000;

            connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletFrom.Id, Balance = walletFrom.Balance });
            connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletTo.Id, Balance = walletTo.Balance });

            transactionScope.Complete();

            Console.WriteLine("Transaction committed successfully using TransactionScope.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
        }
    }
}
```
- **TransactionScope**: Manages the transaction scope for operations that need to be atomic across multiple connections or resources.
- **Open Connection**: Opens a new `SqlConnection` object.
- **Retrieve Wallets**: Fetches wallet records without an explicit transaction, relying on `TransactionScope` for transaction management.
- **Check Records**: Ensures both wallets are found before proceeding.
- **Update Balances**: Adjusts balances and updates the records.
- **Complete Transaction**: Calls `Complete()` to commit the transaction if everything is successful.

## Best Practices

1. **Error Handling**:
   Wrap database operations in try-catch blocks to handle potential exceptions.

   ```csharp
   try
   {
       // Database operations
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

2. **Transaction Management**:
   Use transactions to ensure that multiple database operations are atomic and consistent. For operations that need to span multiple resources, use `TransactionScope`.

   ```csharp
   using (var transactionScope = new TransactionScope())
   {
       // Database operations
       transactionScope.Complete();
   }
   ```

3. **Testing**:
   Test transaction handling thoroughly to ensure that the rollback works as expected and that the application behaves correctly under various failure scenarios.
