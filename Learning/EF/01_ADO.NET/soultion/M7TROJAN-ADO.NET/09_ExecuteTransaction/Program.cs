using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _09_ExecuteTransaction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new SqlConnection(configuration.GetSection("constr").Value);


            SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.Text;

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();

            command.Transaction = transaction;

            try
            {
                command.CommandText = "UPDATE Wallets SET Balance = Balance - 1000 WHERE Id = 1";
                command.ExecuteNonQuery();

                command.CommandText = "UPDATE Wallets SET Balance = Balance + 1000 WHERE Id = 2";
                command.ExecuteNonQuery();

                transaction.Commit();

                Console.WriteLine("Transaction Done Successfully");
            }
            catch
            {
                Console.WriteLine("Transaction Failed");

                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // log errors or do something
                }

            }
            finally
            {
                try
                { 
                    connection.Close();
                }
                catch
                {
                    // log errors or do something
                }
            }

        }
    }
}
