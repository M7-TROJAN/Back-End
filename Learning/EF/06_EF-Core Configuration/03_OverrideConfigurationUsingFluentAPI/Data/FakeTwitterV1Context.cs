using _03_OverrideConfigurationUsingFluentAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _03_OverrideConfigurationUsingFluentAPI.Data
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

            modelBuilder.Entity<User>()
                .ToTable("tblUsers"); // this maps the User entity to the tblUsers table in the database

            modelBuilder.Entity<Tweet>()
                .ToTable("tblTweets"); // this maps the Tweet entity to the tblTweets table in the database

            modelBuilder.Entity<Comment>()
                .ToTable("tblComments"); // this maps the Comment entity to the tblComments table in the database

            modelBuilder.Entity<Comment>()
                .Property(c => c.Id)
                .HasColumnName("CommentId"); // this maps the Id property to the CommentId column in the database

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id); // this maps the Id property to the primary key in the database


            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("tblComments"); // this maps the Comment entity to the tblComments table in the database
                entity.HasKey(e => e.Id); // this maps the Id property to the primary key in the database
                entity.Property(e => e.Id).HasColumnName("CommentId").ValueGeneratedOnAdd(); // this maps the Id property to the CommentId column in the database and sets it to be generated on add (auto-increment)
                entity.Property(e => e.CommentText).IsRequired().HasMaxLength(140); // this maps the CommentText property to the CommentText column in the database and sets it to be required and have a maximum length of 140 characters
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()"); // this maps the CreatedAt property to the CreatedAt column in the database and sets it to have a default value of the current date and time
                entity.HasOne<Tweet>().WithMany() // this maps the Comment entity to the Tweet entity with a one-to-many relationship
                      .HasForeignKey(e => e.TweetId) // this maps the TweetId property to the foreign key in the database
                      .OnDelete(DeleteBehavior.Cascade); // this sets the delete behavior to cascade
                entity.HasOne<User>().WithMany() // this maps the Comment entity to the User entity with a one-to-many relationship
                      .HasForeignKey(e => e.UserId) // this maps the UserId property to the foreign key in the database
                      .OnDelete(DeleteBehavior.Cascade); // this sets the delete behavior to cascade
            });

        }
    }
}
