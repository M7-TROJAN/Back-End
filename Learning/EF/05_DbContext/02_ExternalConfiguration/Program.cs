using _02_ExternalConfiguration.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _02_ExternalConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // This is the same as the previous example but now we are using the AppDbContext class with a constructor that accepts DbContextOptions.

            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Create the DbContextOptions object with the connection string and the SQL Server provider
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new AppDbContext(options))
            {
                var wallets = context.Wallets;

                foreach (var wallet in wallets)
                {
                    Console.WriteLine(wallet);
                }
            }
        }
    }
}
