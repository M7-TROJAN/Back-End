using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace _03_ExecuteInsertRawSql
{
    public class  Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal? Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
        
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            // read the name and balance from the user
            string name = ReadName();
            decimal balance = ReadBalance();

            // insert the new wallet into the database

            var sql = $"INSERT INTO Wallets (Holder, Balance) VALUES (@Holder, @Balance)";

            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = name
            };

            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = balance
            };
             
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            command.CommandType = CommandType.Text;

            connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

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
