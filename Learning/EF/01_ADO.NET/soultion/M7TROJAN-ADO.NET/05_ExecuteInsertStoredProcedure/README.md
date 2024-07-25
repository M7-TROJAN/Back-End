```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _05_ExecuteInsertStoredProcedure
{
    public class Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal? Balance { get; set; }

        // Override ToString to provide a formatted string representation of a Wallet object
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
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Create a Wallet object to insert
            var walletToInsert = new Wallet
            {
                Holder = "Asmaa Adel",
                Balance = 20_000
            };

            // Create a SqlConnection object using the connection string from the configuration
            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            // Create a SqlCommand object to execute the stored procedure
            var command = connection.CreateCommand();

            // Set the CommandText to the name of the stored procedure
            command.CommandText = "AddWallet";

            // Set the command type to StoredProcedure
            command.CommandType = CommandType.StoredProcedure;

            // Create a SqlParameter for the Holder parameter
            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder", // Name of the parameter in the stored procedure
                SqlDbType = SqlDbType.NVarChar, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = walletToInsert.Holder // The value to be passed to the stored procedure
            };

            // Create a SqlParameter for the Balance parameter
            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance", // Name of the parameter in the stored procedure
                SqlDbType = SqlDbType.Decimal, // Data type of the parameter
                Direction = ParameterDirection.Input, // Specifies that this is an input parameter
                Value = walletToInsert.Balance // The value to be passed to the stored procedure
            };

            // Add the parameters to the command
            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            // Open the database connection
            connection.Open();

            // Execute the command and check if any rows were affected
            if (command.ExecuteNonQuery() > 0)
            {
                Console.WriteLine($"Wallet for {walletToInsert.Holder} added successfully.");
            }
            else
            {
                Console.WriteLine("Failed to add wallet.");
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
       .AddJsonFile("appsettings.json")
       .Build();
   ```
   - This sets up the configuration to read settings from `appsettings.json`.

2. **Creating Wallet Object**:
   ```csharp
   var walletToInsert = new Wallet
   {
       Holder = "Asmaa Adel",
       Balance = 20_000
   };
   ```
   - This creates a new `Wallet` object to be inserted into the database.

3. **Database Connection**:
   ```csharp
   var connection = new SqlConnection(configuration.GetSection("constr").Value);
   ```
   - This creates a `SqlConnection` object using the connection string from the configuration.

4. **SQL Command**:
   ```csharp
   var command = connection.CreateCommand();
   command.CommandText = "AddWallet";
   command.CommandType = CommandType.StoredProcedure;
   ```
   - This creates a `SqlCommand` object and sets its command text to the name of the stored procedure to be executed.

5. **SQL Parameters**:
   ```csharp
   SqlParameter holderParam = new SqlParameter { ... };
   SqlParameter balanceParam = new SqlParameter { ... };
   ```
   - These create parameters for the stored procedure to pass the wallet holder's name and balance.

6. **Adding Parameters**:
   ```csharp
   command.Parameters.Add(holderParam);
   command.Parameters.Add(balanceParam);
   ```
   - This adds the parameters to the command.

7. **Executing the Command**:
   ```csharp
   connection.Open();
   if (command.ExecuteNonQuery() > 0)
   {
       Console.WriteLine($"Wallet for {walletToInsert.Holder} added successfully.");
   }
   else
   {
       Console.WriteLine("Failed to add wallet.");
   }
   connection.Close();
   ```
   - This opens the connection, executes the stored procedure, and checks if the wallet was added successfully.