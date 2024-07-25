```csharp
namespace _04_ExecuteInsertExecuteScaler
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
            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            // Initialize variables for the wallet holder's ID, name, and balance
            int id = 0;
            string holder = ReadName();
            decimal balance = ReadBalance();

            // Create a SqlCommand object to execute the SQL query
            var command = connection.CreateCommand();

            // Define the SQL query to insert a new wallet and retrieve the generated ID
            command.CommandText = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance); " +
                                  "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            // Set the command type to Text (default)
            command.CommandType = CommandType.Text;

            // Create a SqlParameter for the Holder parameter
            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.NVarChar, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = holder // The value to be passed to the SQL query
            };

            // Create a SqlParameter for the Balance parameter
            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.Decimal, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = balance // The value to be passed to the SQL query
            };

            // Add the parameters to the command
            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            // Open the database connection
            connection.Open();

            // Execute the SQL query and retrieve the generated ID
            id = (int)command.ExecuteScalar();

            // Print a message indicating that the wallet was created successfully
            Console.WriteLine($"Wallet with ID {id} was created.");

            // Close the database connection
            connection.Close();
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

3. **Reading User Input**:
   ```csharp
   string holder = ReadName();
   decimal balance = ReadBalance();
   ```
   - These methods prompt the user to enter the wallet holder's name and balance.

4. **SQL Command**:
   ```csharp
   command.CommandText = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance); " +
                         "SELECT CAST(SCOPE_IDENTITY() AS INT);";
   ```
   - This defines the SQL query to insert a new wallet and retrieve the generated ID using `SCOPE_IDENTITY()`.

5. **SQL Parameters**:
   ```csharp
   SqlParameter holderParam = new SqlParameter { ... };
   SqlParameter balanceParam = new SqlParameter { ... };
   ```
   - These create parameters for the SQL query to prevent SQL injection and ensure safe data handling.

6. **Executing the Command**:
   ```csharp
   connection.Open();
   id = (int)command.ExecuteScalar();
   ```
   - This opens the connection, executes the query, and retrieves the generated ID.

7. **Printing the Result**:
   ```csharp
   Console.WriteLine($"Wallet with ID {id} was created.");
   ```
   - This prints a message indicating that the wallet was created successfully.

8. **Closing the Connection**:
   ```csharp
   connection.Close();
   ```
   - This closes the database connection.

9. **Reading Name Method**:
   ```csharp
   private static string ReadName() { ... }
   ```
   - This method reads and validates the wallet holder's name from the user.

10. **Reading Balance Method**:
    ```csharp
    private static decimal ReadBalance() { ... }
    ```
    - This method reads and validates the wallet balance from the user.