# Understanding DbContext Lifetime in Entity Framework Core

This guide demonstrates how to manage the lifetime of a `DbContext` in Entity Framework Core in a C# console application. Proper management of `DbContext` lifetime is crucial for ensuring optimal performance and resource management.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Entity Framework Core packages (can be installed via NuGet)

## Step-by-Step Guide

### 1. Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `AppDbContext.cs`
- `Wallet.cs`
- `appsettings.json`

### 2. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YourServer;Database=YourDatabase;Integrated Security=True;"
  }
}
```

### 3. Entity Class

The `Wallet` class represents the entity in the database.

```csharp
namespace _05_DbContextLifeTime.Entities
{
    public class Wallet
    {
        public int Id { get; set; } // Primary key
        public string? Holder { get; set; } // Wallet holder's name
        public decimal Balance { get; set; } // Wallet balance

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

### 4. `AppDbContext.cs`

The `AppDbContext` class is configured to use `DbContextOptions` for its configuration.

```csharp
using Microsoft.EntityFrameworkCore;

namespace _05_DbContextLifeTime.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
```

### 5. Main Program

The main program builds the configuration, retrieves the connection string, configures the options for `DbContext`, and demonstrates the usage of `DbContext` within a `using` block to ensure proper disposal.

```csharp
using _05_DbContextLifeTime.Data;
using _05_DbContextLifeTime.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _05_DbContextLifeTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Get the connection string from the configuration
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Create DbContextOptions with the connection string
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            // Use the DbContext within a using block for proper disposal
            using (var context = new AppDbContext(options))
            {
                // Create a new Wallet entity
                var w1 = new Wallet
                {
                    Holder = "Adham",
                    Balance = 1000m
                };

                // Add the Wallet entity to the context
                context.Wallets.Add(w1);

                // Create another Wallet entity
                var w2 = new Wallet
                {
                    Holder = "Abdalaziz",
                    Balance = 2000m
                };

                // Add the second Wallet entity to the context
                context.Wallets.Add(w2);

                // Ask the user if they want to save changes
                char saveChanges = 'n';

                Console.WriteLine("Do you want to save changes? (y/n)");

                saveChanges = Console.ReadKey().KeyChar;

                if (saveChanges == 'y' || saveChanges == 'Y')
                {
                    // Save the changes to the database
                    context.SaveChanges();
                    Console.WriteLine("\nChanges are saved.");
                }
                else
                {
                    Console.WriteLine("\nChanges are not saved.");
                }

            } // The context is disposed here

            Console.ReadKey();
        }
    }
}
```

## Explanation of Key Points

1. **Configuration Building**:
    ```csharp
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - The `ConfigurationBuilder` is used to build the configuration by adding the `appsettings.json` file. This allows the application to read configuration settings, such as the connection string, from the `appsettings.json` file.

2. **Retrieving the Connection String**:
    ```csharp
    var connectionString = config.GetConnectionString("DefaultConnection");
    ```
    - The connection string is retrieved from the configuration. This connection string is used to configure the `DbContext` to connect to the SQL Server database.

3. **Creating DbContextOptions**:
    ```csharp
    var options = new DbContextOptionsBuilder()
        .UseSqlServer(connectionString)
        .Options;
    ```
    - The `DbContextOptionsBuilder` is used to create `DbContextOptions` that configure the `DbContext` to use SQL Server with the specified connection string. These options are then passed to the `AppDbContext` constructor.

4. **Using the DbContext**:
    ```csharp
    using (var context = new AppDbContext(options))
    {
        // Operations with DbContext
    } // The context is disposed here
    ```
    - The `DbContext` is used within a `using` block to ensure proper disposal. When the `using` block is exited, the `Dispose` method of the `DbContext` is called, releasing any resources it holds. Proper disposal of the `DbContext` is essential to avoid memory leaks and ensure that database connections are released back to the pool.

5. **Adding Entities**:
    ```csharp
    var w1 = new Wallet { Holder = "Adham", Balance = 1000m };
    context.Wallets.Add(w1);

    var w2 = new Wallet { Holder = "Abdalaziz", Balance = 2000m };
    context.Wallets.Add(w2);
    ```
    - Two new `Wallet` entities are created and added to the `Wallets` DbSet. This schedules the entities to be inserted into the database when `SaveChanges` is called.

6. **Conditional Save Changes**:
    ```csharp
    char saveChanges = 'n';
    Console.WriteLine("Do you want to save changes? (y/n)");
    saveChanges = Console.ReadKey().KeyChar;

    if (saveChanges == 'y' || saveChanges == 'Y')
    {
        context.SaveChanges();
        Console.WriteLine("\nChanges are saved.");
    }
    else
    {
        Console.WriteLine("\nChanges are not saved.");
    }
    ```
    - The program prompts the user to confirm whether they want to save the changes. If the user confirms by pressing 'y' or 'Y', `SaveChanges` is called to persist the changes to the database. Otherwise, the changes are not saved.

## Conclusion

This example demonstrates how to manage the lifetime of a `DbContext` in Entity Framework Core. By following these steps, you can ensure proper resource management and avoid common pitfalls such as memory leaks and connection exhaustion. Proper management of `DbContext` lifetime is essential for building efficient and reliable applications.