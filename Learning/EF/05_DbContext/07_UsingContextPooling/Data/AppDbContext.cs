using _07_UsingContextPooling.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _07_UsingContextPooling.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
