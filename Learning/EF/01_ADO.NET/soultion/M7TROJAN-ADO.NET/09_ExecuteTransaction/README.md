```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _09_ExecuteTransaction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Create a new SQL connection using the connection string from the configuration
            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            // Create a new SQL command
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            // Open the connection to the database
            connection.Open();

            // Begin a transaction on the open connection
            SqlTransaction transaction = connection.BeginTransaction();

            // Associate the command with the transaction
            command.Transaction = transaction;

            try
            {
                // Set the command text to deduct 1000 from the balance of the wallet with Id 1
                command.CommandText = "UPDATE Wallets SET Balance = Balance - 1000 WHERE Id = 1";
                command.ExecuteNonQuery();

                // Set the command text to add 1000 to the balance of the wallet with Id 2
                command.CommandText = "UPDATE Wallets SET Balance = Balance + 1000 WHERE Id = 2";
                command.ExecuteNonQuery();

                // Commit the transaction if both commands succeed
                transaction.Commit();

                Console.WriteLine("Transaction Done Successfully");
            }
            catch
            {
                // Print an error message if the transaction fails
                Console.WriteLine("Transaction Failed");

                try
                {
                    // Attempt to roll back the transaction in case of an error
                    transaction.Rollback();
                }
                catch
                {
                    // Log errors or handle rollback failures if needed
                }
            }
            finally
            {
                try
                { 
                    // Close the connection to the database
                    connection.Close();
                }
                catch
                {
                    // Log errors or handle connection close failures if needed
                }
            }
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
   var connection = new SqlConnection(configuration.GetSection("constr").Value);
   ```
   - This creates a `SqlConnection` object using the connection string from the configuration.

3. **SQL Command**:
   ```csharp
   SqlCommand command = connection.CreateCommand();
   command.CommandType = CommandType.Text;
   ```
   - This creates a `SqlCommand` object and sets its `CommandType` to `Text`.

4. **Opening Connection**:
   ```csharp
   connection.Open();
   ```
   - This opens the connection to the database.

5. **Beginning a Transaction**:
   ```csharp
   SqlTransaction transaction = connection.BeginTransaction();
   command.Transaction = transaction;
   ```
   - This begins a transaction on the open connection and associates the command with the transaction.

6. **Executing SQL Commands**:
   ```csharp
   command.CommandText = "UPDATE Wallets SET Balance = Balance - 1000 WHERE Id = 1";
   command.ExecuteNonQuery();

   command.CommandText = "UPDATE Wallets SET Balance = Balance + 1000 WHERE Id = 2";
   command.ExecuteNonQuery();
   ```
   - This sets the command text to update the balances of the wallets and executes the commands.

7. **Committing the Transaction**:
   ```csharp
   transaction.Commit();
   ```
   - This commits the transaction if both commands succeed.

8. **Error Handling and Rollback**:
   ```csharp
   catch
   {
       Console.WriteLine("Transaction Failed");

       try
       {
           transaction.Rollback();
       }
       catch
       {
           // Log errors or handle rollback failures if needed
       }
   }
   ```
   - If an error occurs, the transaction is rolled back to maintain data integrity.

9. **Finally Block**:
   ```csharp
   finally
   {
       try
       { 
           connection.Close();
       }
       catch
       {
           // Log errors or handle connection close failures if needed
       }
   }
   ```
   - Ensures the connection is closed regardless of whether the transaction succeeds or fails.

### Best Practices and Error Handling

- **Using Statements**: Normally, you'd use `using` statements to ensure resources are disposed of correctly. This example shows explicit handling for clarity.
- **Error Logging**: In a production environment, you should log errors to a logging system.
- **Isolation Levels**: Set appropriate isolation levels based on your application's concurrency requirements using `connection.BeginTransaction(IsolationLevel.Serializable)`.
- **Parameterization**: Always use parameters to prevent SQL injection attacks, even though this example uses hardcoded values for simplicity.

### Use Case Scenario: Transferring Funds Between Wallets

In this example, we are simulating a transfer of 1000 units from one wallet to another. This involves two operations that must both succeed or fail together to ensure data consistency. If either operation fails, the entire transaction is rolled back, leaving the database in its original state. This is a common scenario in financial applications where data integrity is critical.

### Additional Considerations

- **Timeouts**: Consider setting command and connection timeouts to prevent hanging transactions.
- **Concurrency**: Ensure that your transaction logic accounts for possible concurrency issues, such as deadlocks or race conditions.
- **Testing**: Rigorously test transaction handling under various scenarios, including network failures and database outages.