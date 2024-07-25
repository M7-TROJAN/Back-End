```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _08_ExecuteRawSqlDataAdapter
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

            // Create a SqlCommand object for the SQL query
            var command = connection.CreateCommand();

            // Set the SQL query to select all rows from the Wallets table
            command.CommandText = "SELECT * FROM Wallets";

            // Set the command type to Text
            command.CommandType = CommandType.Text;

            // Open the database connection
            connection.Open();

            // Create a SqlDataAdapter to execute the command and fill the DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            // Create a DataTable to hold the query results
            DataTable table = new DataTable();

            // Fill the DataTable with the query results
            adapter.Fill(table);

            // Close the database connection
            connection.Close(); // No need to keep the connection open because the data is already in memory

            // Iterate through the rows of the DataTable and create Wallet objects
            foreach (DataRow row in table.Rows)
            {
                var wallet = new Wallet
                {
                    Id = Convert.ToInt32(row["Id"]), // Convert the Id to an integer
                    Holder = row["Holder"].ToString(), // Convert the Holder to a string
                    Balance = Convert.ToDecimal(row["Balance"]) // Convert the Balance to a decimal
                };

                // Print the Wallet object to the console
                Console.WriteLine(wallet);
            }

            Console.WriteLine("--------------------------------------------------");

            // Using DataSet to hold the query results

            // Create a DataSet to hold the query results
            DataSet dataSet = new DataSet();

            // Fill the DataSet with the query results
            adapter.Fill(dataSet);

            // Iterate through the rows of the first table in the DataSet and create Wallet objects
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var wallet = new Wallet
                {
                    Id = Convert.ToInt32(row["Id"]), // Convert the Id to an integer
                    Holder = row["Holder"].ToString(), // Convert the Holder to a string
                    Balance = Convert.ToDecimal(row["Balance"]) // Convert the Balance to a decimal
                };

                // Print the Wallet object to the console
                Console.WriteLine(wallet);
            }
        }
    }

    // Define the Wallet class to represent wallet data
    public class Wallet
    {
        public int Id { get; set; } // Property for the wallet Id
        public string? Holder { get; set; } // Property for the wallet Holder
        public decimal? Balance { get; set; } // Property for the wallet Balance

        // Override the ToString method to provide a custom string representation of the Wallet object
        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
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
   var command = connection.CreateCommand();
   command.CommandText = "SELECT * FROM Wallets";
   command.CommandType = CommandType.Text;
   ```
   - This creates a `SqlCommand` object and sets its `CommandText` to select all rows from the `Wallets` table and `CommandType` to `Text`.

4. **Data Adapter and DataTable**:
   ```csharp
   SqlDataAdapter adapter = new SqlDataAdapter(command);
   DataTable table = new DataTable();
   adapter.Fill(table);
   ```
   - This creates a `SqlDataAdapter` to execute the command and fill a `DataTable` with the results.

5. **Data Processing**:
   ```csharp
   foreach (DataRow row in table.Rows)
   {
       var wallet = new Wallet
       {
           Id = Convert.ToInt32(row["Id"]),
           Holder = row["Holder"].ToString(),
           Balance = Convert.ToDecimal(row["Balance"])
       };
       Console.WriteLine(wallet);
   }
   ```
   - This iterates through the rows of the `DataTable`, creates `Wallet` objects from the row data, and prints them to the console.

6. **Using DataSet**:
   ```csharp
   DataSet dataSet = new DataSet();
   adapter.Fill(dataSet);
   foreach (DataRow row in dataSet.Tables[0].Rows)
   {
       var wallet = new Wallet
       {
           Id = Convert.ToInt32(row["Id"]),
           Holder = row["Holder"].ToString(),
           Balance = Convert.ToDecimal(row["Balance"])
       };
       Console.WriteLine(wallet);
   }
   ```
   - This demonstrates an alternative method using a `DataSet` to hold the query results and process the data.

### What is a DataTable?

- A `DataTable` is an in-memory representation of a single table of data. It consists of a collection of `DataRow` objects, representing rows, and a collection of `DataColumn` objects, representing columns. It can be used to hold data retrieved from a database or other data sources.

### What is a DataSet?

- A `DataSet` is an in-memory representation of an entire relational database, complete with tables, relationships, and constraints. It can hold multiple `DataTable` objects. It is useful when working with data from multiple tables, and it supports operations such as navigating relationships and enforcing constraints between tables.
