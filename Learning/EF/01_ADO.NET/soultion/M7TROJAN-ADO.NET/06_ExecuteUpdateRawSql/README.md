```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _06_ExecuteUpdateRawSql
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

            // SQL query to update the Wallets table
            var sql = "UPDATE Wallets SET Holder = @Holder, Balance = @Balance WHERE Id = @Id";

            // Create a SqlParameter for the Id parameter
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Id", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.Int, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = 3 // The value to be passed to the SQL query
            };

            // Create a SqlParameter for the Holder parameter
            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.NVarChar, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = "Mahmoud Adham" // The value to be passed to the SQL query
            };

            // Create a SqlParameter for the Balance parameter
            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance", // Name of the parameter in the SQL query
                SqlDbType = SqlDbType.Decimal, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = 2000.0 // The value to be passed to the SQL query
            };

            // Create a SqlCommand object to execute the SQL query
            SqlCommand command = new SqlCommand(sql, connection);

            // Add the parameters to the command
            command.Parameters.Add(idParam);
            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            // Set the command type to Text
            command.CommandType = CommandType.Text;

            // Open the database connection
            connection.Open();

            // Execute the command and get the number of rows affected
            int rowsAffected = command.ExecuteNonQuery();

            // Check if any rows were affected
            if (rowsAffected > 0)
            {
                Console.WriteLine("Update done successfully.");
                Console.WriteLine($"Rows affected: {rowsAffected}");
            }
            else
            {
                Console.WriteLine("Update failed.");
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
   var sql = "UPDATE Wallets SET Holder = @Holder, Balance = @Balance WHERE Id = @Id";
   ```
   - This defines the SQL query to update the `Wallets` table.

4. **SQL Parameters**:
   ```csharp
   SqlParameter idParam = new SqlParameter { ... };
   SqlParameter holderParam = new SqlParameter { ... };
   SqlParameter balanceParam = new SqlParameter { ... };
   ```
   - These create parameters for the SQL query to pass the wallet ID, holder's name, and balance.

5. **Adding Parameters**:
   ```csharp
   command.Parameters.Add(idParam);
   command.Parameters.Add(holderParam);
   command.Parameters.Add(balanceParam);
   ```
   - This adds the parameters to the command.

6. **Executing the Command**:
   ```csharp
   connection.Open();
   int rowsAffected = command.ExecuteNonQuery();
   if (rowsAffected > 0)
   {
       Console.WriteLine("Update done successfully.");
       Console.WriteLine($"Rows affected: {rowsAffected}");
   }
   else
   {
       Console.WriteLine("Update failed.");
   }
   connection.Close();
   ```
   - This opens the connection, executes the SQL query, and checks if the update was successful by verifying the number of rows affected.