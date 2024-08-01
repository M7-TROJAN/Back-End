using _06_DbContextAndConcurrency.Data;
using _06_DbContextAndConcurrency.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _06_DbContextAndConcurrency
{
    internal class Program
    {
        static AppDbContext dbContext;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            dbContext = serviceProvider.GetRequiredService<AppDbContext>();


            // now we want to execute two jobs in parallel on the same Time

            // JobOne: Add a new Wallet
            Wallet wallet = new Wallet
            {
                Holder = "Jasem",
                Balance = 500
            };

            // JobTwo: Add a new Wallet
            Wallet wallet2 = new Wallet
            {
                Holder = "Heba",
                Balance = 1000
            };

            
            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => JobOne(wallet)),
                Task.Factory.StartNew(() => JobTwo(wallet2))
            };

            Task.WhenAll(tasks).ContinueWith((t) =>
            {
               Console.WriteLine("JobOne & JobTwo are Executed Cocurrently!");
            });

            Console.ReadKey();


        }

        static async Task JobOne(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);

            await dbContext.SaveChangesAsync();
        }

        static async Task JobTwo(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);

            await dbContext.SaveChangesAsync();
        }
    }
}
