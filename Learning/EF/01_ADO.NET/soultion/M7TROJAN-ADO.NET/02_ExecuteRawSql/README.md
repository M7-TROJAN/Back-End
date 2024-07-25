```csharp
using Microsoft.Data.SqlClient; // Importing the SqlClient namespace for database operations
using Microsoft.Extensions.Configuration; // Importing the IConfiguration namespace for configuration settings
using System.Data; // Importing the Data namespace for database-related enumerations

namespace _02_ExecuteRawSql
{
    // Define a Wallet class to represent data from the Wallets table
    public class Wallet
    {
        public int Id { get; set; } // Property to store the wallet ID
        public string? Holder { get; set; } // Property to store the wallet holder's name
        public decimal? Balance { get; set; } // Property to store the wallet balance

        // Override the ToString method to provide a string representation of the Wallet object
        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }

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

            // Define the raw SQL query to select all records from the Wallets table
            var sql = "SELECT * FROM Wallets";

            // Create a SqlCommand object to execute the SQL query
            SqlCommand command = new SqlCommand(sql, connection);

            // Set the command type to Text, indicating it's a raw SQL query
            command.CommandType = CommandType.Text;

            // Open the database connection
            connection.Open();

            // Execute the SQL query and obtain a SqlDataReader object to read the results
            SqlDataReader reader = command.ExecuteReader();

            // Loop through the results
            while (reader.Read())
            {
                // Create a new Wallet object and populate its properties with data from the current row
                var wallet = new Wallet
                {
                    Id = reader.GetInt32("Id"), // Get the wallet ID
                    Holder = reader.GetString("Holder"), // Get the wallet holder's name
                    Balance = reader.GetDecimal("Balance") // Get the wallet balance
                };

                // Print the Wallet object to the console
                Console.WriteLine(wallet);
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

3. **SQL Command**:
   ```csharp
   var sql = "SELECT * FROM Wallets";
   SqlCommand command = new SqlCommand(sql, connection);
   command.CommandType = CommandType.Text;
   ```
   - This defines the raw SQL query and creates a `SqlCommand` object to execute it.

4. **Executing the Command**:
   ```csharp
   connection.Open();
   SqlDataReader reader = command.ExecuteReader();
   ```
   - This opens the connection and executes the query, returning a `SqlDataReader` object to read the results.

5. **Reading Results**:
   ```csharp
   while (reader.Read())
   {
       var wallet = new Wallet
       {
           Id = reader.GetInt32("Id"),
           Holder = reader.GetString("Holder"),
           Balance = reader.GetDecimal("Balance")
       };

       Console.WriteLine(wallet);
   }
   ```
   - This loop reads each row from the result set, creates a `Wallet` object, and prints it to the console.

6. **Closing the Connection**:
   ```csharp
   connection.Close();
   ```
   - This closes the database connection after reading the results.