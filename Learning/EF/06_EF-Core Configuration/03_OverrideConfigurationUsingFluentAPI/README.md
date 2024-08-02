# Overriding Configuration Using Fluent API in EF Core

Entity Framework Core (EF Core) provides two main ways to configure the model: Data Annotations and Fluent API. While Data Annotations are easy to use and provide a straightforward way to configure your model, the Fluent API is more powerful and flexible, allowing you to define configurations that are not possible with Data Annotations. This guide demonstrates how to override the default configuration in EF Core using the Fluent API.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Entity Framework Core packages (can be installed via NuGet)

## Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `FakeTwitterV2Context.cs`
- `User.cs`
- `Tweet.cs`
- `Comment.cs`
- `appsettings.json`

## Step-by-Step Guide

### 1. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "FakeTwitterV2": "Server=YourServer;Database=FakeTwitterV2;Integrated Security=True;"
  }
}
```

### 2. Entity Classes

Define your entity classes without any data annotations.

#### `User.cs`

```csharp
namespace _02_OverrideConfigurationUsingFluentAPI.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return $"[{UserId}] - {Username}";
        }
    }
}
```

#### `Tweet.cs`

```csharp
namespace _02_OverrideConfigurationUsingFluentAPI.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string TweetText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

#### `Comment.cs`

```csharp
namespace _02_OverrideConfigurationUsingFluentAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

### 3. `FakeTwitterV2Context.cs`

The `FakeTwitterV2Context` class is where you will configure the model using the Fluent API.

```csharp
using _02_OverrideConfigurationUsingFluentAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _02_OverrideConfigurationUsingFluentAPI.Data
{
    internal class FakeTwitterV2Context : DbContext
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

            var connectionString = config.GetConnectionString("FakeTwitterV2");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("tblUsers");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            });

            // Configuring Tweet entity
            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.ToTable("tblTweets");
                entity.HasKey(e => e.TweetId);
                entity.Property(e => e.TweetText).IsRequired().HasMaxLength(280);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuring Comment entity
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("tblComments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("CommentId").ValueGeneratedOnAdd();
                entity.Property(e => e.CommentText).IsRequired().HasMaxLength(140);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne<Tweet>()
                      .WithMany()
                      .HasForeignKey(e => e.TweetId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
```

### 4. Main Program

The main program demonstrates how to query the data from the `FakeTwitterV2` database using EF Core.

```csharp
using _02_OverrideConfigurationUsingFluentAPI.Data;

namespace _02_OverrideConfigurationUsingFluentAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new FakeTwitterV2Context())
            {
                Console.WriteLine("\n_________ Users __________\n");
                foreach (var user in context.Users)
                {
                    Console.WriteLine(user);
                }

                Console.WriteLine("\n_________ Tweets __________\n");
                foreach (var tweet in context.Tweets)
                {
                    Console.WriteLine(tweet.TweetText);
                }

                Console.WriteLine("\n_________ Comments __________\n");
                foreach (var comment in context.Comments)
                {
                    Console.WriteLine(comment.CommentText);
                }
            }

            Console.ReadKey();
        }
    }
}
```

## Explanation of Key Points

1. **Specifying Table Name**:
    ```csharp
    modelBuilder.Entity<User>().ToTable("tblUsers");
    ```
    - The `ToTable` method is used to specify the table name in the database that corresponds to the entity class. This is particularly useful when the table name does not follow the default convention.

2. **Configuring Primary Key**:
    ```csharp
    entity.HasKey(e => e.UserId);
    ```
    - The `HasKey` method is used to specify the primary key of the entity.

3. **Configuring Properties**:
    ```csharp
    entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
    ```
    - The `Property` method is used to configure individual properties. In this case, the `Username` property is required and has a maximum length of 50 characters.

4. **Configuring Relationships**:
    ```csharp
    entity.HasOne<User>()
          .WithMany()
          .HasForeignKey(e => e.UserId)
          .OnDelete(DeleteBehavior.Cascade);
    ```
    - The `HasOne`, `WithMany`, and `HasForeignKey` methods are used to configure relationships between entities. The `OnDelete` method specifies the delete behavior for the foreign key relationship.

5. **Setting Default Values**:
    ```csharp
    entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
    ```
    - The `HasDefaultValueSql` method sets a default value for a property using a SQL expression. In this case, the `CreatedAt` property has a default value of the current date and time.

## Conclusion

This example demonstrates how to override the default configuration in Entity Framework Core using the Fluent API. By following these steps, you can customize the database schema and enforce validation rules directly in your entity classes, ensuring that your data model adheres to your business rules and database schema requirements. The Fluent API provides more flexibility and power than Data Annotations, allowing you to define complex configurations that are not possible with Data Annotations alone.