using _02_ExternalConfiguration.Entities;
using Microsoft.EntityFrameworkCore;

namespace _02_ExternalConfiguration.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
