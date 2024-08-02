
# ConfigurationByConvention EF Core

This guide demonstrates how to configure and use the `FakeTwitterV2` database with Entity Framework Core in a C# console application. Proper configuration and usage of `DbContext` are crucial for ensuring optimal performance and resource management.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Entity Framework Core packages (can be installed via NuGet)

## Step-by-Step Guide

### 1. Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `FakeTwitterV2Context.cs`
- `User.cs`
- `Tweet.cs`
- `Comment.cs`
- `appsettings.json`

### 2. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "FakeTwitterV2": "Server=YourServer;Database=FakeTwitterV2;Integrated Security=True;"
  }
}
```

### 3. Entity Classes

The entity classes represent the tables in the database.

#### `User.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace _01_ConfigurationByConvention.Entities
{
    public class User
    {
        // Primary key convention => [Id, id , ID] , [{ClassName}Id]
        // if you want to use another name for the primary key you can use the [Key] attribute like this:
        //[Key]
        //public int number { get; set; }

        public int UserId { get; set; } // Primary key

        [Required] // Not null
        public string Username { get; set; }

        override public string ToString()
        {
            return $"[{UserId}] - {Username}";
        }
    }
}
```

#### `Tweet.cs`

```csharp
namespace _01_ConfigurationByConvention.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key to User
        public string TweetText { get; set; } // Tweet content
        public DateTime CreatedAt { get; set; } // Date and time the tweet was created
    }
}
```

#### `Comment.cs`

```csharp
namespace _01_ConfigurationByConvention.Entities
{
    public class Comment
    {
        public int CommentId { get; set; } // Primary key
        public int TweetId { get; set; } // Foreign key to Tweet
        public int UserId { get; set; } // Foreign key to User
        public string CommentText { get; set; } // Comment content
        public DateTime CreatedAt { get; set; } // Date and time the comment was created
    }
}
```

### 4. `FakeTwitterV2Context.cs`

The `FakeTwitterV2Context` class is configured to use `DbContextOptions` for its configuration.

```csharp
using _01_ConfigurationByConvention.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _01_ConfigurationByConvention.Data
{
    internal class FakeTwitterV2Context : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; } // DbSet for Tweets table
        public DbSet<User> Users { get; set; } // DbSet for Users table
        public DbSet<Comment> Comments { get; set; } // DbSet for Comments table

        // if the dbset name is not the same as the table name in the database the exception will be thrown
        // (microsoft.data.sqlclient.SqlException "invalid object Name") 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("FakeTwitterV2");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
```

### 5. Main Program

The main program demonstrates how to query the data from the `FakeTwitterV2` database using EF Core.

```csharp
namespace _01_ConfigurationByConvention
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Data.FakeTwitterV2Context())
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

## Configuration by Convention

### What is Configuration by Convention?

Configuration by Convention is a principle in Entity Framework Core (EF Core) that allows developers to set up the entity model and database schema without needing to write extensive configuration code. EF Core uses conventions to automatically configure the model based on the class definitions and properties.

### Key Rules of Configuration by Convention
1) DbSet Property Name match Table Name
2) Primary key (id) or ({class}id)
3) Column Name match property name

1. **Primary Key Convention**:
   - By default, EF Core will recognize properties named `Id`, `id`, `ID`, or `{ClassName}Id` as primary keys.
   - Example:
     ```csharp
     public int UserId { get; set; } // Recognized as the primary key
     ```
   - If a different property name is desired, the `[Key]` attribute can be used to explicitly designate a primary key:
     ```csharp
     [Key]
     public int CustomId { get; set; }
     ```

2. **Foreign Key Convention**:
   - EF Core recognizes properties that follow the pattern `{NavigationProperty}Id` as foreign keys.
   - Example:
     ```csharp
     public int UserId { get; set; } // Foreign key to User
     ```

3. **Column Name Matching Property Name**:
   - The property names in the entity classes must match the column names in the database tables. This ensures that EF Core correctly maps the properties to the corresponding columns.
   - Example:
     ```csharp
     public string Username { get; set; } // Maps to Username column
     ```
   - If the names do not match, attributes like `[Column("ColumnName")]` can be used to specify the column name:
     ```csharp
     [Column("User_Name")]
     public string Username { get; set; }
     ```

4. **DbSet Property Name Matching Table Name**:
   - The `DbSet` properties in the `DbContext` class must match the table names in the database. If the names do not match, an exception (e.g., `SqlException: invalid object name`) will be thrown.
   - Example:
     ```csharp
     public DbSet<User> Users { get; set; } // Maps to Users table
     ```

## Explanation of Key Points

1. **DbSet Property Name Matching Table Name**:
    ```csharp
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    ```
    - The `DbSet` properties in the `FakeTwitterV2Context` class must match the table names in the database. If the names do not match, an exception (e.g., `SqlException: invalid object name`) will be thrown. Ensuring that the `DbSet` property names match the table names is crucial for correct mapping and querying.

2. **Primary Key Naming Convention**:
    ```csharp
    public int UserId { get; set; } // Primary key
    ```
    - By convention, Entity Framework Core recognizes properties named `Id`, `id`, `ID`, or `{ClassName}Id` as primary keys. In the `User` class, the property `UserId` follows this convention and is automatically recognized as the primary key. If a different name is desired, the `[Key]` attribute can be used to explicitly mark a property as the primary key.

3. **Column Name Matching Property Name**:
    ```csharp
    public string Username { get; set; }
    ```
    - The property names in the entity classes must match the column names in the database tables. This ensures that EF Core correctly maps the properties to the corresponding columns. For example, the `Username` property in the `User` class maps to the `Username` column in the `Users` table. If the names do not match, attributes like `[Column("ColumnName")]` can be used to specify the column name.

4. **Configuration Building**:
    ```csharp
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - The `ConfigurationBuilder` is used to build the configuration by adding the `appsettings.json` file. This allows the application to read configuration settings, such as the connection string, from the `appsettings.json` file.

5. **Retrieving the Connection String**:
    ```csharp
    var connectionString = config.GetConnectionString("FakeTwitterV2");
    ```
    - The connection string is retrieved from the configuration. This connection string is used to configure the `DbContext` to connect to the SQL Server database.

6. **Creating DbContextOptions**:
    ```csharp
    optionsBuilder.UseSqlServer(connectionString);
    ```
    - The `DbContextOptionsBuilder` is used to configure the `DbContext` to use SQL Server with the specified connection string. These options are then passed to the `FakeTwitterV2Context` constructor.

7. **Using the DbContext**:
    ```csharp
    using (var context = new Data.FakeTwitterV2Context())
    {
        // Operations with DbContext
    } // The context is disposed here
    ```
    - The `DbContext` is used within a `using` block to ensure proper disposal. When the `using` block is exited, the `Dispose` method of the `DbContext` is called, releasing any resources it holds. Proper disposal of the `DbContext` is essential to avoid memory leaks and ensure that database connections are released back to the pool.

8. **Querying Data**:
    ```csharp
    foreach (var user in context.Users)
    {
        Console.WriteLine(user);
    }
    ```
    - The data from the `Users`, `Tweets`, and `Comments` tables are queried and printed to the console. This demonstrates how to retrieve and work with data using EF Core.

## Conclusion

This example demonstrates how to configure and use the `FakeTwitterV2` database with Entity Framework Core. By following these steps, you can ensure proper configuration and usage of `DbContext` for building efficient and reliable applications.
```

This structured example includes detailed explanations and comments to enhance understanding, covering all the requested points such as matching `DbSet` property names with table names, primary key conventions, and column name matching.