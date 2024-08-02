# Overriding Configuration Using Grouping Configuration in EF Core

Entity Framework Core (EF Core) allows for flexible model configuration through the Fluent API. This guide demonstrates how to override the default configuration using grouping configuration for a structured and maintainable approach.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Entity Framework Core packages (can be installed via NuGet)

## Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `FakeTwitterV1Context.cs`
- `User.cs`
- `Tweet.cs`
- `Comment.cs`
- `appsettings.json`
- `CommentConfiguration.cs`
- `TweetConfiguration.cs`
- `UserConfiguration.cs`

## Step-by-Step Guide

### 1. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "FakeTwitterV1": "Server=YourServer;Database=FakeTwitterV1;Integrated Security=True;"
  }
}
```

### 2. Entity Classes

Define your entity classes without any data annotations.

#### `User.cs`

```csharp
namespace _04_OverrideConfigurationByGroupingConfiguration.Entities
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
namespace _04_OverrideConfigurationByGroupingConfiguration.Entities
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
namespace _04_OverrideConfigurationByGroupingConfiguration.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

### 3. `FakeTwitterV1Context.cs`

The `FakeTwitterV1Context` class is where you will configure the model using the Fluent API and grouping configuration.

```csharp
using _04_OverrideConfigurationByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _04_OverrideConfigurationByGroupingConfiguration.Data
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
```

### 4. Configuration Classes

Create configuration classes to define the schema details for each entity.

#### `CommentConfiguration.cs`

```csharp
using _04_OverrideConfigurationByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfigurationByGroupingConfiguration.Data.Config
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("tblComments"); // this maps to the table name in the database

            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.CommentText)
                .IsRequired()
                .HasMaxLength(280);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
```

#### `TweetConfiguration.cs`

```csharp
using _04_OverrideConfigurationByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfigurationByGroupingConfiguration.Data.Config
{
    public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.ToTable("tblTweets"); // this maps to the table name in the database

            builder.HasKey(t => t.TweetId);

            builder.Property(t => t.TweetText)
                .IsRequired()
                .HasMaxLength(280);

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
```

#### `UserConfiguration.cs`

```csharp
using _04_OverrideConfigurationByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfigurationByGroupingConfiguration.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tblUsers"); // this maps to the table name in the database
            
            builder.HasKey(u => u.UserId); 
            
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
```

### 5. Main Program

The main program demonstrates how to query the data from the `FakeTwitterV1` database using EF Core.

```csharp
using _04_OverrideConfigurationByGroupingConfiguration.Data;

namespace _04_OverrideConfigurationByGroupingConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new FakeTwitterV1Context())
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
    builder.ToTable("tblUsers");
    ```
    - The `ToTable` method is used to specify the table name in the database that corresponds to the entity class. This is particularly useful when the table name does not follow the default convention.

2. **Configuring Primary Key**:
    ```csharp
    builder.HasKey(e => e.UserId);
    ```
    - The `HasKey` method is used to specify the primary key of the entity.

3. **Configuring Properties**:
    ```csharp
    builder.Property(e => e.Username).IsRequired().HasMaxLength(50);
    ```
    - The `Property` method is used to configure individual properties. In this case, the `Username` property is required and has a maximum length of 50 characters.

4. **Setting Default Values**:
    ```csharp
    builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
    ```
    - The `HasDefaultValueSql` method sets a default value for a property using a SQL expression. In this case, the `CreatedAt` property has a default value of the current date and time.

## Conclusion

This example demonstrates how to override the default configuration in Entity Framework Core using the Fluent API and grouping configuration. By following these steps, you can customize the database schema and enforce validation rules directly in your entity classes, ensuring that your data model adheres to your business rules and database schema requirements. The Fluent API provides more flexibility and power than Data Annotations, allowing you to define complex configurations that are not possible with Data Annotations alone.