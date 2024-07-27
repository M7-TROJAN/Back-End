using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
namespace _02_ExecuteInsert
{
    internal class Program
    {
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IDbConnection dbConnection = new SqlConnection(configuration.GetSection("constr").Value);

            var walletToInsert = new Wallet { Holder = "Sarah", Balance = 7400m };

            var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance)";

            var parameters = new { Holder = walletToInsert.Holder, Balance = walletToInsert.Balance  };

            var affectedRows = dbConnection.Execute(sqlQuery, parameters);

            if (affectedRows > 0)
            {
                Console.WriteLine("Wallet inserted successfully!");
                Console.WriteLine($"Affected Rows: {affectedRows}");
            }
            else
            {
                Console.WriteLine("Wallet not inserted!");
            }

            

            Console.ReadKey();

        }
    }
}
