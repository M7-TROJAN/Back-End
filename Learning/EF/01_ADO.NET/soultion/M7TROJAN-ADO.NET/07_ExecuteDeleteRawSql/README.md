Here is the code with added comments to make it more understandable:

```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _07_ExecuteDeleteRawSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path for the configuration
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Create a SqlConnection object using the connection string from the configuration
            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            // SQL query to delete a wallet from the Wallets table
            var sql = "DELETE FROM Wallets WHERE Id = @Id";

            // Create a SqlParameter for the Id parameter
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Id", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.Int, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = 3 // The value to be passed to the SQL query (ID of the wallet to be deleted)
            };

            // Create a SqlCommand object to execute the SQL query
            SqlCommand command = new SqlCommand(sql, connection);

            // Add the parameter to the command
            command.Parameters.Add(idParam);

            // Set the command type to Text
            command.CommandType = CommandType.Text;

            // Open the database connection
            connection.Open();

            // Execute the command and get the number of rows affected
            int rowsAffected = command.ExecuteNonQuery();

            // Check if any rows were affected
            if (rowsAffected > 0)
            {
                Console.WriteLine("Wallet deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete failed.");
            }

            // Close the database connection
            connection.Close();
        }
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

3. **SQL Query**:
   ```csharp
   var sql = "DELETE FROM Wallets WHERE Id = @Id";
   ```
   - This defines the SQL query to delete a wallet from the `Wallets` table.

4. **SQL Parameter**:
   ```csharp
   SqlParameter idParam = new SqlParameter
   {
       ParameterName = "@Id",
       SqlDbType = SqlDbType.Int,
       Direction = ParameterDirection.Input,
       Value = 3
   };
   ```
   - This creates a parameter for the SQL query to specify the wallet ID to be deleted.

5. **Adding Parameter**:
   ```csharp
   command.Parameters.Add(idParam);
   ```
   - This adds the parameter to the command.

6. **Executing the Command**:
   ```csharp
   connection.Open();
   int rowsAffected = command.ExecuteNonQuery();
   if (rowsAffected > 0)
   {
       Console.WriteLine("Wallet deleted successfully.");
   }
   else
   {
       Console.WriteLine("Delete failed.");
   }
   connection.Close();
   ```
   - This opens the connection, executes the SQL query, and checks if the delete operation was successful by verifying the number of rows affected.