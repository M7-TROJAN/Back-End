using _04_UsingContextFactory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _04_UsingContextFactory.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
