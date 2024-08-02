using _02_OverrideConfigurationByDataAnnotations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _02_OverrideConfigurationByDataAnnotations.Data
{
    internal class FakeTwitterV1Context : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // if the dbset name is not the same as the table name in the database the exception will be thrown
        // (microsoft.data.sqlclient.SqlException "invalid object Name") 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("FakeTwitterV1");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
