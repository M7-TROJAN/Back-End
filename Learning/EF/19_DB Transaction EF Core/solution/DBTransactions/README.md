### Understanding Database Transactions in EF Core

In EF Core, transactions are a powerful tool used to ensure that a series of operations either complete successfully as a group or fail as a group. This is critical for maintaining data consistency, especially in scenarios involving multiple related changes.

#### Key Concepts

1. **Transaction**: A transaction is a sequence of one or more operations that are executed as a single unit. If any operation in the transaction fails, the entire transaction fails, and the database is rolled back to its state before the transaction began.

2. **Commit**: Committing a transaction means that all operations within the transaction are successfully completed, and the changes are permanently applied to the database.

3. **Rollback**: Rolling back a transaction undoes all operations performed within the transaction, reverting the database to its previous state before the transaction started.

4. **Savepoint**: A savepoint is a point within a transaction to which you can roll back without affecting the entire transaction. This allows partial rollbacks in complex transactions.

### Example 1: Basic Transaction Management

```csharp
private static void Run_Changes_Within_Single_SaveChanges()
{
    using (var context = new AppDbContext())
    {
        DatabaseHelper.RecreateCleanDatabase();
        DatabaseHelper.PopulateDatabase();

        var Account1 = context.BankAccounts.Find("1");
        var Account2 = context.BankAccounts.Find("2");

        var ammountToTransfer = 500;

        Account1.Withdraw(ammountToTransfer);

        if (random.Next(0, 2) == 0)
        {
            throw new Exception("Random Exception"); // Simulate an exception
        }

        Account2.Deposit(ammountToTransfer);

        context.SaveChanges(); // EF Core ensures that all changes are saved in a single transaction
    }
}
```

**Explanation**: In this example, EF Core handles the transaction automatically when `SaveChanges()` is called. If an exception occurs before `SaveChanges()`, the changes are not saved, ensuring data integrity.

### Example 2: Explicit Transaction Management

```csharp
private static void Run_Changes_Within_Multiple_SaveChanges_In_DbTransaction()
{
    using (var context = new AppDbContext())
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            var Account1 = context.BankAccounts.Find("1");
            var Account2 = context.BankAccounts.Find("2");

            var ammountToTransfer = 500;

            Account1.Withdraw(ammountToTransfer);
            context.SaveChanges(); // Save the first set of changes

            if (random.Next(0, 2) == 0)
            {
                throw new Exception("Random Exception"); // Simulate an exception
            }

            Account2.Deposit(ammountToTransfer);
            context.SaveChanges(); // Save the second set of changes

            transaction.Commit(); // Commit the transaction
        }
    }
}
```

**Explanation**: Here, we manually control the transaction. The `BeginTransaction()` method starts a transaction, and `Commit()` finalizes it. If an exception occurs, the transaction is not committed, and the changes are not applied.

### Example 3: Using Savepoints

```csharp
private static void SavePoints_In_DbTransaction()
{
    using (var context = new AppDbContext())
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var Account1 = context.BankAccounts.Find("1");
                var Account2 = context.BankAccounts.Find("2");

                transaction.CreateSavepoint("BeforeWithdraw"); // Create a savepoint

                var ammountToTransfer = 500;

                Account1.Withdraw(ammountToTransfer);
                context.SaveChanges(); 

                transaction.CreateSavepoint("AfterWithdraw"); // Create another savepoint

                if (random.Next(0, 2) == 0)
                {
                    throw new Exception("Random Exception"); // Simulate an exception
                }

                Account2.Deposit(ammountToTransfer);
                context.SaveChanges();

                transaction.CreateSavepoint("AfterDeposit"); // Create another savepoint

                transaction.Commit(); // Commit the transaction
            }
            catch (Exception ex)
            {
                transaction.RollbackToSavepoint("BeforeWithdraw"); // Rollback to a specific savepoint
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
```

**Explanation**: Savepoints allow partial rollbacks within a transaction. This is useful in complex scenarios where you want to undo some operations without canceling the entire transaction. In this example, if an exception occurs, we can roll back to the "BeforeWithdraw" savepoint, effectively undoing only the operations after that point.

### Real-World Scenario

Imagine you're building a banking system where funds are transferred between accounts. You want to ensure that either both the withdrawal and deposit occur or neither does. Transactions and savepoints are crucial here:

- **Without transactions**: If the withdrawal succeeds but the deposit fails, one account would be missing funds, and the other wouldn’t gain them, leading to inconsistent data.
- **With transactions**: If the deposit fails, the withdrawal is rolled back, ensuring that no money is lost.

This ensures that the system remains consistent and reliable, even in the face of errors or exceptions.