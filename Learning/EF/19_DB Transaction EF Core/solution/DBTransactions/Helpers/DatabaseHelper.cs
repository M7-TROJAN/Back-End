
using DBTransactions.Data;
using DBTransactions.Models;

namespace DBTransactions.Helpers
{
    public static class DatabaseHelper
    {
        public static void RecreateCleanDatabase()
        {
            using var context = new AppDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void PopulateDatabase()
        {
            using (var context = new AppDbContext())
            {
                context.Add(
                    new BankAccount
                    {
                        AccountID = "1",
                        AccountHolder = "Mahmoud Mattar",
                        CurrentBalance = 10000m
                    });

                context.Add(
                    new BankAccount
                    {
                        AccountID = "2",
                        AccountHolder = "Reem Ali",
                        CurrentBalance = 15000
                    });

                context.Add(
                    new BankAccount
                    {
                        AccountID = "3",
                        AccountHolder = "Ahmed Samir",
                        CurrentBalance = 20000
                    });

                context.Add(
                    new BankAccount
                    {
                        AccountID = "4",
                        AccountHolder = "Sara Ahmed",
                        CurrentBalance = 25000
                    });

                context.SaveChanges();
            }
        }
    }
}
