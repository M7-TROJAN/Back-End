using DBTransactions.Data;
using DBTransactions.Helpers;
using DBTransactions.Models;
using System.Data.SqlTypes;

namespace DBTransactions
{
    internal class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            // Run_Initial_Transfer_Walkthrough();
            // Run_Changes_Within_Multiple_SaveChanges();
            // Run_Changes_Within_Single_SaveChanges();
            // Run_Changes_Within_Multiple_SaveChanges_In_DbTransaction();
            // Run_Changes_Within_Multiple_SaveChanges_In_DbTransaction_BestPractices();
            SavePoints_In_DbTransaction();

        }

        private static void Run_Initial_Transfer_Walkthrough()
        {
            var Account1 = new BankAccount
            {
                AccountID = "12345",
                AccountHolder = "Mahmoud Mattar",
                CurrentBalance = 1000
            };

            var Account2 = new BankAccount
            {
                AccountID = "54321",
                AccountHolder = "Ahmed Mattar",
                CurrentBalance = 500
            };

            // Print the initial balances
            Console.WriteLine($"Account1 Initial Balance: {Account1.CurrentBalance}");
            Console.WriteLine($"Account2 Initial Balance: {Account2.CurrentBalance}");

            // Transfer 500 from Account1 to Account2

            var ammountToTransfer = 500;

            Account1.Withdraw(ammountToTransfer);

            Account2.Deposit(ammountToTransfer);

            // pritn the new balances
            Console.WriteLine($"Account1 New Balance: {Account1.CurrentBalance}");
            Console.WriteLine($"Account2 New Balance: {Account2.CurrentBalance}");
        }

        private static void Run_Changes_Within_Multiple_SaveChanges()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();

                var Account1 = context.BankAccounts.Find("1");
                var Account2 = context.BankAccounts.Find("2");

                // Print the initial balances
                Console.WriteLine($"Account1 Initial Balance: {Account1.CurrentBalance}");
                Console.WriteLine($"Account2 Initial Balance: {Account2.CurrentBalance}");

                // Transfer 500 from Account1 to Account2
                var ammountToTransfer = 500;

                Account1.Withdraw(ammountToTransfer);
                context.SaveChanges(); // Save the changes to the database

                if (random.Next(0, 2) == 0)
                {
                    throw new Exception("Random Exception"); // Simulate an exception
                }

                Account2.Deposit(ammountToTransfer);
                context.SaveChanges(); // Save the changes to the database

                // Print the new balances
                Console.WriteLine($"Account1 New Balance: {Account1.CurrentBalance}");
                Console.WriteLine($"Account2 New Balance: {Account2.CurrentBalance}");
            }
        }


        private static void Run_Changes_Within_Single_SaveChanges()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();

                var Account1 = context.BankAccounts.Find("1");
                var Account2 = context.BankAccounts.Find("2");

                // Transfer 500 from Account1 to Account2
                var ammountToTransfer = 500;

                Account1.Withdraw(ammountToTransfer);

                if (random.Next(0, 2) == 0)
                {
                    throw new Exception("Random Exception"); // Simulate an exception
                }

                Account2.Deposit(ammountToTransfer);

                context.SaveChanges(); // EF Core By Defult ensure that all changes will be in a single transaction (either all changes are saved or none of them are saved)

            }
        }

        private static void Run_Changes_Within_Multiple_SaveChanges_In_DbTransaction()
        {
            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var Account1 = context.BankAccounts.Find("1");
                    var Account2 = context.BankAccounts.Find("2");

                    // Transfer 500 from Account1 to Account2
                    var ammountToTransfer = 500;

                    Account1.Withdraw(ammountToTransfer);
                    context.SaveChanges();

                    if (random.Next(0, 2) == 0)
                    {
                        throw new Exception("Random Exception"); // Simulate an exception
                    }

                    Account2.Deposit(ammountToTransfer);
                    context.SaveChanges();

                    transaction.Commit(); // Commit the transaction
                }
            }
        }

        private static void Run_Changes_Within_Multiple_SaveChanges_In_DbTransaction_BestPractices()
        {
            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var Account1 = context.BankAccounts.Find("1");
                        var Account2 = context.BankAccounts.Find("2");

                        // Transfer 500 from Account1 to Account2
                        var ammountToTransfer = 500;

                        Account1.Withdraw(ammountToTransfer);
                        context.SaveChanges();

                        if (random.Next(0, 2) == 0)
                        {
                            throw new Exception("Random Exception"); // Simulate an exception
                        }

                        Account2.Deposit(ammountToTransfer);
                        context.SaveChanges();

                        transaction.Commit(); // Commit the transaction
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback the transaction
                        Console.WriteLine($"An error occured: {ex.Message}");
                    }
                }
            }

        }


        private static void SavePoints_In_DbTransaction()
        {
            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var Account1 = context.BankAccounts.Find("1");
                        var Account2 = context.BankAccounts.Find("2");

                        transaction.CreateSavepoint("BeforeWithdraw"); // Create a savepoint

                        // Transfer 500 from Account1 to Account2
                        var ammountToTransfer = 500;

                        Account1.Withdraw(ammountToTransfer);
                        context.SaveChanges();

                        transaction.CreateSavepoint("AfterWithdraw"); // Create a savepoint

                        if (random.Next(0, 2) == 0)
                        {
                            throw new Exception("Random Exception"); // Simulate an exception
                        }

                        Account2.Deposit(ammountToTransfer);
                        context.SaveChanges();

                        transaction.CreateSavepoint("AfterDeposit"); // Create a savepoint


                        transaction.Commit(); // Commit the transaction
                    }
                    catch(SqlAlreadyFilledException ex)
                    {
                        transaction.RollbackToSavepoint("BeforeWithdraw"); // Rollback to the savepoint
                        Console.WriteLine($"An error occured: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback the transaction
                        Console.WriteLine($"An error occured: {ex.Message}");
                    }
                }
            }

        }
    }
}
