using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace _04_ExecuteInsertExecuteScaler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            int id = 0;
            string holder = ReadName();
            decimal balance = ReadBalance();


            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            command.CommandType = CommandType.Text; // default

            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = holder
            };

            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = balance
            };

            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            connection.Open();

            id = (int)command.ExecuteScalar();

            Console.WriteLine($"Wallet with ID {id} was created.");

            connection.Close();

        }

        private static string ReadName()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter the name: ");
                    string name = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        throw new ArgumentException("Name cannot be empty.");
                    }

                    return name;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid name, please try again. Error: {ex.Message}");
                }
            }
        }

        private static decimal ReadBalance()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter the balance: ");
                    string input = Console.ReadLine();

                    if (decimal.TryParse(input, out decimal balance))
                    {
                        return balance;
                    }
                    else
                    {
                        throw new FormatException("Balance must be a valid decimal number.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid balance, please try again. Error: {ex.Message}");
                }
            }
        }
    }
}
