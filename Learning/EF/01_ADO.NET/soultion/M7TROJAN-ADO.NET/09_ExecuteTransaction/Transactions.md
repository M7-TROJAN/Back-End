# Transactions in ADO.NET

Transactions are a fundamental concept in database operations, ensuring that a series of operations either all succeed or all fail, maintaining the consistency and integrity of the database. This guide will cover everything you need to know about transactions in ADO.NET, including best practices, error handling, and a use case scenario.

## What is a Transaction?

A transaction is a sequence of operations performed as a single logical unit of work. A transaction has four key properties, often referred to as ACID properties:
- **Atomicity**: Ensures that all operations within the transaction are completed successfully. If any operation fails, the entire transaction is rolled back.
- **Consistency**: Ensures that the database remains in a consistent state before and after the transaction.
- **Isolation**: Ensures that the operations within a transaction are isolated from other concurrent transactions.
- **Durability**: Ensures that once a transaction is committed, it remains permanent even in the case of a system failure.

## Creating a Transaction in ADO.NET

### Basic Structure

Here's a basic example of how to create and manage a transaction in ADO.NET:

```csharp
using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "your_connection_string_here";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Perform your database operations here

                // Commit the transaction
                transaction.Commit();
                Console.WriteLine("Transaction committed.");
            }
            catch (Exception ex)
            {
                // Rollback the transaction if any error occurs
                transaction.Rollback();
                Console.WriteLine($"Transaction rolled back. Error: {ex.Message}");
            }
        }
    }
}
```

### Best Practices

1. **Use Using Statements**: Ensure that all resources are properly disposed of by using `using` statements.
2. **Error Handling**: Always use try-catch blocks to handle any exceptions and roll back the transaction in case of an error.
3. **Isolation Levels**: Use appropriate isolation levels based on your application's requirements to manage concurrency and data consistency.

### Handling Exceptions

Proper exception handling is crucial when working with transactions. Here's an enhanced version of the previous example with improved error handling:

```csharp
using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "your_connection_string_here";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Perform your database operations here

                // Commit the transaction
                transaction.Commit();
                Console.WriteLine("Transaction committed.");
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                transaction.Rollback();
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                transaction.Rollback();
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    }
}
```

## Isolation Levels

Isolation levels define the degree to which the operations in one transaction are isolated from those in other transactions. The main isolation levels are:
- **Read Uncommitted**: Allows dirty reads, meaning data can be read even if it has not been committed.
- **Read Committed**: Prevents dirty reads, ensuring that only committed data can be read.
- **Repeatable Read**: Prevents dirty reads and ensures that once data is read, it cannot be modified by other transactions until the current transaction is complete.
- **Serializable**: Ensures complete isolation by locking the data being read or written, preventing other transactions from accessing it until the current transaction is complete.

### Setting Isolation Levels

You can set the isolation level when starting a transaction:

```csharp
SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
```

## Use Case Scenario

### Scenario: Transferring Money Between Accounts

Let's consider a scenario where we need to transfer money from one bank account to another. This involves two operations: debiting one account and crediting another. Both operations must succeed or fail together, ensuring the consistency of the database.

```csharp
using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "your_connection_string_here";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Debit from source account
                string debitQuery = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountId = @SourceAccountId";
                SqlCommand debitCommand = new SqlCommand(debitQuery, connection, transaction);
                debitCommand.Parameters.AddWithValue("@Amount", 100);
                debitCommand.Parameters.AddWithValue("@SourceAccountId", 1);
                debitCommand.ExecuteNonQuery();

                // Credit to destination account
                string creditQuery = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountId = @DestinationAccountId";
                SqlCommand creditCommand = new SqlCommand(creditQuery, connection, transaction);
                creditCommand.Parameters.AddWithValue("@Amount", 100);
                creditCommand.Parameters.AddWithValue("@DestinationAccountId", 2);
                creditCommand.ExecuteNonQuery();

                // Commit the transaction
                transaction.Commit();
                Console.WriteLine("Transaction committed. Money transferred successfully.");
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                transaction.Rollback();
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                transaction.Rollback();
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    }
}
```

### Explanation

1. **Opening the Connection**: The connection to the database is opened.
2. **Starting the Transaction**: A transaction is started using `connection.BeginTransaction()`.
3. **Performing Operations**: Two SQL commands are executed to debit and credit the accounts.
4. **Committing the Transaction**: If both operations succeed, the transaction is committed.
5. **Rolling Back the Transaction**: If any operation fails, the transaction is rolled back, ensuring that the database remains consistent.

## Conclusion

Transactions are essential for maintaining data integrity and consistency in database operations. By following best practices, handling exceptions properly, and using appropriate isolation levels, you can ensure that your applications handle transactions effectively. The use case scenario provided demonstrates how transactions can be applied in real-world scenarios, such as transferring money between accounts.