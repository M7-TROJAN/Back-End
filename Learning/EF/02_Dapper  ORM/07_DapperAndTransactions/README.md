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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Transfer 2000
            // From: 2 
            // To:  4

            // Using Dapper with manual transaction handling
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

            // Using TransactionScope for transaction management
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

                        // Complete the transaction
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

    public class Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal? Balance { get; set; }

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
- **Open Connection**: Creates and opens a new `SqlConnection` object using the connection string.
- **Begin Transaction**: Starts a new transaction using `BeginTransaction()`.
- **Execute Queries**: Fetches wallet records, adjusts balances, and updates them within the transaction.
- **Commit/Rollback Transaction**: Commits the transaction if all operations succeed or rolls it back in case of an error.

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

            // Complete the transaction
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
- **TransactionScope**: Manages the transaction scope and ensures all operations succeed or fail together.
- **Open Connection**: Opens a new `SqlConnection` object.
- **Execute Queries**: Fetches wallet records, adjusts balances, and updates them.
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
```