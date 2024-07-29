
# Implementing Transactions in Entity Framework Core

This guide demonstrates how to implement transactions using Entity Framework Core in a C# console application. Transactions ensure that a series of operations either complete successfully together or fail together, maintaining database integrity.

## Prerequisites

- .NET SDK
- SQL Server
- Entity Framework Core installed via NuGet (`Microsoft.EntityFrameworkCore.SqlServer`)

## Sample Data

Assume the following `Wallet` class represents the data in the database:

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

Here is a complete example of using transactions with Entity Framework Core:

```csharp
using Microsoft.EntityFrameworkCore;

namespace _08_ImplementTransaction
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
                        // Amount to transfer
                        var amountToTransfer = 1000;

                        // Retrieve the wallets involved in the transaction
                        var walletFrom = context.Wallets.Find(7);
                        var walletTo = context.Wallets.Find(10);

                        // Check if both wallets exist
                        if (walletFrom is null || walletTo is null)
                        {
                            Console.WriteLine("Wallet not found");
                            return;
                        }

                        // Operation #1: Withdraw amount from wallet 7
                        walletFrom.Balance -= amountToTransfer;
                        context.SaveChanges(); // Save changes within the transaction

                        // Operation #2: Deposit amount to wallet 10
                        walletTo.Balance += amountToTransfer;
                        context.SaveChanges(); // Save changes within the transaction

                        // Commit the transaction to apply changes to the database
                        transaction.Commit();

                        Console.WriteLine("Transfer done successfully");
                    }
                    catch (Exception e)
                    {
                        // Rollback the transaction in case of an error
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
```

## Explanation of Key Points

1. **Starting a Transaction**:
    ```csharp
    using (var transaction = context.Database.BeginTransaction())
    ```
    - This starts a new transaction. All subsequent changes to the database will be part of this transaction until `Commit` or `Rollback` is called.

2. **Performing Operations**:
    ```csharp
    walletFrom.Balance -= amountToTransfer;
    context.SaveChanges(); // Save changes within the transaction
    ```
    - Changes made to entities are tracked by the context. `context.SaveChanges()` commits these changes to the database but within the scope of the transaction.

3. **Committing the Transaction**:
    ```csharp
    transaction.Commit();
    ```
    - This commits all changes made during the transaction to the database. Until `Commit` is called, changes are not permanent and can be rolled back.

4. **Rolling Back the Transaction**:
    ```csharp
    transaction.Rollback();
    ```
    - In case of an error, `Rollback` is called to revert all changes made during the transaction, ensuring the database remains in a consistent state.

5. **Error Handling**:
    ```csharp
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        transaction.Rollback();
    }
    ```
    - Exceptions are caught, and the transaction is rolled back to ensure no partial updates are made if an error occurs.

## Best Practices

- **Wrap Changes in Transactions**: Always use transactions for operations that involve multiple changes to ensure data integrity.
- **Handle Exceptions Gracefully**: Implement proper exception handling to roll back transactions and maintain database consistency.
- **Keep Transactions Short**: Minimize the amount of time transactions are open to avoid locking issues and performance problems.

By following these practices, you ensure that your application maintains data integrity and handles errors gracefully.
