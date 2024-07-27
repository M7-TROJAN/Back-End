using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace _03_ExecuteInsertReturnIdentity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IDbConnection dbConnection = new SqlConnection(configuration.GetSection("constr").Value);

            var walletToInsert = new Wallet { Holder = "Mansour", Balance = 7400m };

            var sqlQuery = "INSERT INTO WALLETS (HOLDER, BALANCE) VALUES (@Holder, @Balance); SELECT CAST(SCOPE_IDENTITY() as INT)";

            var parameters = new { Holder = walletToInsert.Holder, Balance = walletToInsert.Balance };


            // this will return a list of integers and we are selecting the first one
            // walletToInsert.Id = dbConnection.Query<int>(sqlQuery, parameters).Single();

            // OR

            // this will return a single integer
            walletToInsert.Id = dbConnection.QuerySingle<int>(sqlQuery, parameters);

            if (walletToInsert.Id > 0)
            {
                Console.WriteLine("Wallet inserted successfully!");
                Console.WriteLine($"Wallet Id: {walletToInsert.Id}");
            }
            else
            {
                Console.WriteLine("Wallet not inserted!");
            }

            Console.ReadKey();
        }
    }
}
