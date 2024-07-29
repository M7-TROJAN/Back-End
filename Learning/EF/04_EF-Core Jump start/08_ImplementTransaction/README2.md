# Transactions with Entity Framework Core

This guide demonstrates how to handle transactions in Entity Framework Core in a C# console application. Transactions ensure that a series of operations are completed successfully or none of them are applied, maintaining database integrity.

## Prerequisites

- .NET SDK
- Entity Framework Core with SQL Server
- `AppDbContext` class configured for the database

## Sample Data

Assume we have a `Wallet` class representing a table in the database:

```csharp
public class Wallet
{
    public int Id { get; set; }
    public string? Holder { get; set; }
    public decimal Balance { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Holder} ({Balance:C})";
    }
}
```

## Code Example

Here is a C# console application demonstrating how to use transactions with Entity Framework Core:

```csharp
namespace _08_TransactionHandling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Start a new transaction
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Create and add two wallets
                        var wallet1 = new Wallet { Holder = "John", Balance = 2000m };
                        var wallet2 = new Wallet { Holder = "Jane", Balance = 3000m };

                        context.Wallets.Add(wallet1);
                        context.Wallets.Add(wallet2);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("Transaction committed successfully!");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }
    }
}
```

## Explanation of Key Points

1. **Start a New Transaction**
   ```csharp
   using (var transaction = context.Database.BeginTransaction())
   ```
   - Begins a new transaction. This ensures that all operations performed within this block are part of a single transaction.

2. **Perform Operations**
   ```csharp
   var wallet1 = new Wallet { Holder = "John", Balance = 2000m };
   var wallet2 = new Wallet { Holder = "Jane", Balance = 3000m };

   context.Wallets.Add(wallet1);
   context.Wallets.Add(wallet2);
   ```
   - Create and add entities to the context. These operations are tracked but not yet committed to the database.

3. **Save Changes**
   ```csharp
   context.SaveChanges();
   ```
   - Saves changes to the database. If successful, changes are prepared to be committed in the transaction.

4. **Commit the Transaction**
   ```csharp
   transaction.Commit();
   ```
   - Commits the transaction, making all changes made during the transaction permanent.

5. **Exception Handling and Rollback**
   ```csharp
   catch (Exception ex)
   {
       transaction.Rollback();
       Console.WriteLine($"An error occurred: {ex.Message}");
   }
   ```
   - Catches exceptions that occur during the transaction. If an error occurs, the transaction is rolled back, reverting all changes made during the transaction.

## Best Practices

- **Use Transactions for Critical Operations**: Ensure transactions are used for operations that require atomicity, consistency, isolation, and durability (ACID properties).
- **Keep Transactions Short**: Minimize the time spent within a transaction to avoid long-running locks and potential performance issues.
- **Handle Exceptions**: Always handle exceptions to ensure transactions are rolled back properly and maintain data integrity.
- **Avoid Nested Transactions**: Use nested transactions cautiously, as they can complicate transaction management and exception handling.

This example demonstrates how to handle transactions in EF Core, including committing and rolling back transactions. Adjust the transaction scope and error handling according to your application's specific requirements.


This guide provides a clear overview of managing transactions with EF Core, including best practices and explanations of key points for handling transactions effectively.