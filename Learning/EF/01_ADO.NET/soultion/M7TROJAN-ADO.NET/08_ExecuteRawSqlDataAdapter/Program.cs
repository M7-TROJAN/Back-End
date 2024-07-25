using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _08_ExecuteRawSqlDataAdapter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            var command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Wallets";

            command.CommandType = CommandType.Text;

            connection.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataTable table = new DataTable();

            adapter.Fill(table);

            connection.Close(); // There is no need to connect because the data is already in memory


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

            Console.WriteLine("--------------------------------------------------");

            // using dataset

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


        }
    }
}
