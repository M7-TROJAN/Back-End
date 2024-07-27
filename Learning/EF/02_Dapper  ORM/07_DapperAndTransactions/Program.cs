using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Transactions;
namespace _07_DapperAndTransactions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Transfer 2000
            // From: 2 
            // To:  4

            // Using Dapper with manual transaction handling
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var walletFrom = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 2 }, transaction: trans);
                        var walletTo = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 4 }, transaction: trans);

                        if (walletFrom == null || walletTo == null)
                        {
                            throw new Exception("One or both wallets not found.");
                        }

                        walletFrom.Balance -= 2000;
                        walletTo.Balance += 2000;

                        connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletFrom.Id, Balance = walletFrom.Balance }, transaction: trans);
                        connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletTo.Id, Balance = walletTo.Balance }, transaction: trans);

                        trans.Commit();

                        Console.WriteLine("Transaction committed successfully.");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
                    }
                }
            }

            // Using TransactionScope for transaction management
            using (var transactionScope = new TransactionScope())
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    try
                    {
                        var walletFrom = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 2 });
                        var walletTo = connection.QueryFirstOrDefault<Wallet>("SELECT * FROM Wallets WHERE Id = @Id", new { Id = 4 });

                        if (walletFrom == null || walletTo == null)
                        {
                            throw new Exception("One or both wallets not found.");
                        }

                        walletFrom.Balance -= 2000;
                        walletTo.Balance += 2000;

                        connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletFrom.Id, Balance = walletFrom.Balance });
                        connection.Execute("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", new { Id = walletTo.Id, Balance = walletTo.Balance });

                        // Complete the transaction
                        transactionScope.Complete();

                        Console.WriteLine("Transaction committed successfully using TransactionScope.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Transaction rolled back due to an error: {ex.Message}");
                    }
                }
            }
        }
    }
}
