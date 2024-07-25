```csharp
internal class Program
{
    static void Main(string[] args)
    {
        // Build the configuration to read from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
            .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
            .Build(); // Build the configuration

        // Create a SqlConnection object using the connection string from the configuration
        var connection = new SqlConnection(configuration.GetSection("constr").Value);

        // Read the name and balance from the user
        string name = ReadName();
        decimal balance = ReadBalance();

        // Define the SQL query to insert a new wallet into the database
        var sql = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance)";

        // Create a SqlParameter for the Holder parameter
        SqlParameter holderParam = new SqlParameter
        {
            ParameterName = "@Holder", // Name of the parameter in the SQL query
            SqlDbType = SqlDbType.NVarChar, // Data type of the parameter
            Direction = ParameterDirection.Input, // Specifies that this is an input parameter
            Value = name // The value to be passed to the SQL query
        };

        // Create a SqlParameter for the Balance parameter
        SqlParameter balanceParam = new SqlParameter
        {
            ParameterName = "@Balance", // Name of the parameter in the SQL query
            SqlDbType = SqlDbType.Decimal, // Data type of the parameter
            Direction = ParameterDirection.Input, // Specifies that this is an input parameter
            Value = balance // The value to be passed to the SQL query
        };

        // Create a SqlCommand object to execute the SQL query
        SqlCommand command = new SqlCommand(sql, connection);

        // Add the parameters to the command
        command.Parameters.Add(holderParam);
        command.Parameters.Add(balanceParam);

        // Set the command type to Text, indicating it's a raw SQL query
        command.CommandType = CommandType.Text;

        // Open the database connection
        connection.Open();

        // Execute the SQL query and get the number of rows affected
        int rowsAffected = command.ExecuteNonQuery();

        // Check if the wallet was inserted successfully
        if (rowsAffected == 1)
        {
            Console.WriteLine("Wallet inserted successfully.");
            Console.WriteLine($"Rows affected: {rowsAffected}");
        }
        else
        {
            Console.WriteLine("Wallet not inserted.");
            Console.WriteLine($"Rows affected: {rowsAffected}");
        }

        // Close the database connection
        connection.Close();
    }
}
```

### Explanation of Key Points

1. **Configuration Setup**:
   ```csharp
   var configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
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
   string name = ReadName();
   decimal balance = ReadBalance();
   ```
   - These methods prompt the user to enter the wallet holder's name and balance.

4. **SQL Command**:
   ```csharp
   var sql = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance)";
   ```
   - This defines the SQL query to insert a new wallet into the database.

5. **SQL Parameters**:
   ```csharp
   SqlParameter holderParam = new SqlParameter { ... };
   SqlParameter balanceParam = new SqlParameter { ... };
   ```
   - These create parameters for the SQL query to prevent SQL injection and ensure safe data handling.

6. **Executing the Command**:
   ```csharp
   connection.Open();
   int rowsAffected = command.ExecuteNonQuery();
   ```
   - This opens the connection and executes the query, returning the number of rows affected.

7. **Checking the Result**:
   ```csharp
   if (rowsAffected == 1) { ... } else { ... }
   ```
   - This checks if the wallet was inserted successfully and prints the result.

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