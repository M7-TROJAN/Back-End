using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace _05_UpdateData
{
    internal class AppDbContext : DbContext
    {
        // Represent the collection of all entities in the database
        public DbSet<Wallet> Wallets { get; set; }

        // DbSet<T> is a property that represents a table in the database.
        // if you have 60 tables, you need to have 60 DbSet<T> properties.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
