using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;


namespace _05_ExecuteDelete
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
                var sqlQuery = "DELETE FROM Wallets WHERE Id = @Id";

                var parameters = new { Id = 1 };

                var rowsAffected = db.Execute(sqlQuery, parameters);

                Console.WriteLine($"Rows affected: {rowsAffected}");
            }
        }
    }
}
