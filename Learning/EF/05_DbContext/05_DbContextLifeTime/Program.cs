using _05_DbContextLifeTime.Data;
using _05_DbContextLifeTime.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _05_DbContextLifeTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optins = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new AppDbContext(optins))
            {
                var w1 = new Wallet
                {
                    Holder = "Adham",
                    Balance = 1000m
                };

                context.Wallets.Add(w1);

                var w2 = new Wallet
                {
                    Holder = "Abdalaziz",
                    Balance = 2000m
                };

                context.Wallets.Add(w2);

                char saveChanges = 'n';

                Console.WriteLine("Do you want to save changes? (y/n)");

                saveChanges = Console.ReadKey().KeyChar;

                if (saveChanges == 'y' || saveChanges == 'Y')
                {
                    context.SaveChanges();
                    Console.WriteLine("\nChanges are saved.");
                }
                else
                {
                    Console.WriteLine("\nChanges are not saved.");
                }

            } // The context is disposed here

            Console.ReadKey();
        }
    }
}
