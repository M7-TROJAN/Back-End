using EF014.CreateDropAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EF014.CreateDropAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Database will be created if it does not exist
                await context.Database.EnsureCreatedAsync();

                // Generate SQL script to create the database
                var sqlScript = context.Database.GenerateCreateScript();

                // Print the SQL script
                Console.WriteLine(sqlScript);

                // Wait for 30 seconds
                await Task.Delay(30000);

                // Database will be deleted if it does exist
                await context.Database.EnsureDeletedAsync();
            }

            Console.ReadKey();
        }

    }
}

