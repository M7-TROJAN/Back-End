using _04_OverrideConfiguratiobByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _04_OverrideConfiguratiobByGroupingConfiguration.Data
{
    internal class FakeTwitterV1Context : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("FakeTwitterV1");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Config.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Config.TweetConfiguration());
            modelBuilder.ApplyConfiguration(new Config.CommentConfiguration());
        }
    }
}
