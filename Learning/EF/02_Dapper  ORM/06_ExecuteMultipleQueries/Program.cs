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
                    // Using ReadSingle<T> method 
                    //var minBalance = result.ReadSingle<decimal>();
                    //var maxBalance = result.ReadSingle<decimal>();
                    //var sumBalance = result.ReadSingle<decimal>();
                    
                    // Or using Read<T> method
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
