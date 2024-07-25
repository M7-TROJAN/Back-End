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
                .AddJsonFile("appsettings.json")
                .Build();

            var walletToInsert = new Wallet
            {
                Holder = "Asmaa Adel",
                Balance = 20_000
            };

            var connection = new SqlConnection(configuration.GetSection("constr").Value);

            var command = connection.CreateCommand();

            command.CommandText = "AddWallet";

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = walletToInsert.Holder
            };

            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = walletToInsert.Balance
            };

            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            connection.Open();

            if(command.ExecuteNonQuery() > 0)
            {
                Console.WriteLine($"wallet for {walletToInsert.Holder} added successully");

            }
            else
            {
                Console.WriteLine("Failed to add wallet");
            }

            connection.Close();


        }
    }
}
