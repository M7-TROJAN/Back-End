using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace _06_ExecuteMultipleQueries
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "SELECT MIN(Balance) FROM Wallets;" +
                       "SELECT MAX(Balance) FROM Wallets;" +
                       "SELECT SUM(Balance) FROM Wallets;";

                using (var result = db.QueryMultiple(sqlQuery))
                {
                    var minBalance = result.Read<decimal>().Single();
                    var maxBalance = result.Read<decimal>().Single();
                    var sumBalance = result.Read<decimal>().Single();

                    Console.WriteLine($"Min balance: {minBalance}");
                    Console.WriteLine($"Max balance: {maxBalance}");
                    Console.WriteLine($"Sum balance: {sumBalance}");
                }
            }
        }
    }
}
