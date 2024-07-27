using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;


namespace _01_ExecuteRawSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IDbConnection dbConnection = new SqlConnection(configuration.GetSection("constr").Value);

            var sqlQuery = "SELECT * FROM WALLETS";

            Console.WriteLine("\n---------------- using Dynamic Query -------------");

            var resultAsDynamic = dbConnection.Query(sqlQuery); // returns IEnumerable<dynamic>

            foreach (var row in resultAsDynamic)
            {
                Console.WriteLine(row);  
            }

            Console.WriteLine("\n---------------- using Strongly Typed Query -------------");

            var wallets = dbConnection.Query<Wallet>(sqlQuery); // returns IEnumerable<Wallet>

            foreach (var wallet in wallets)
            {
                Console.WriteLine(wallet); 
            }

            Console.WriteLine("\n---------------- using Strongly Typed Query with Parameters -------------");

            var parameters = new { Id = 2 };

            var wallets2 = dbConnection.Query<Wallet>("SELECT * FROM WALLETS WHERE ID = @Id", parameters);

            foreach (var wallet in wallets2)
            {
                Console.WriteLine(wallet);
            }
            
            // no need to close the connection, Dapper will do it for you
        }
    }
}
