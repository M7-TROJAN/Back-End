using _03_UsingDependancyInjection.Entities;
using Microsoft.EntityFrameworkCore;

namespace _03_UsingDependancyInjection.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
