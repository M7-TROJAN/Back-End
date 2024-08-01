
# Using DbContext Factory in Entity Framework Core

This guide demonstrates how to use a `DbContextFactory` to create instances of the `AppDbContext` class in Entity Framework Core. This approach is useful in scenarios where you need to create `DbContext` instances outside of a typical dependency injection scope, such as in background services, multi-threaded applications, or factory patterns.

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
namespace _04_UsingContextFactory.Entities
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
```

### 5. Main Program

The main program builds the configuration, retrieves the connection string, configures the services for dependency injection, registers the `DbContextFactory`, and uses the `AppDbContext` through the factory.

```csharp
using _04_UsingContextFactory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _04_UsingContextFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Create a service collection
            var services = new ServiceCollection();

            // Register the services
            services.AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(connectionString));

            // Create the service provider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get the IDbContextFactory service
            var contextFactory = serviceProvider.GetService<IDbContextFactory<AppDbContext>>();

            // Get the AppDbContext service
            using (var context = contextFactory.CreateDbContext())
            {
                try
                {
                    var wallets = context.Wallets;

                    // Display the wallets
                    foreach (var wallet in wallets)
                    {
                        Console.WriteLine(wallet);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
```

## Explanation of Key Points

1. **DbSet Property**:
    ```csharp
    public DbSet<Wallet> Wallets { get; set; } = null!;
    ```
    - The `Wallets` property of type `DbSet<Wallet>` is used to query and save instances of the `Wallet` entity.

2. **Constructor**:
    ```csharp
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    ```
    - The `AppDbContext` constructor accepts `DbContextOptions` and passes it to the base `DbContext` constructor.

3. **Configuration**:
    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    ```
    - The `ConfigurationBuilder` is used to build the configuration by adding the `appsettings.json` file. The connection string is then retrieved from the configuration.

4. **Service Collection**:
    ```csharp
    var services = new ServiceCollection();
    services.AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(connectionString));
    ```
    - A `ServiceCollection` is created, and the `DbContextFactory` for `AppDbContext` is registered with the connection string configuration using `AddDbContextFactory`.

5. **Service Provider**:
    ```csharp
    IServiceProvider serviceProvider = services.BuildServiceProvider();
    ```
    - The service provider is built from the service collection.

6. **Using Context Factory**:
    ```csharp
    var contextFactory = serviceProvider.GetService<IDbContextFactory<AppDbContext>>();

    using (var context = contextFactory.CreateDbContext())
    {
        try
        {
            var wallets = context.Wallets;

            // Display the wallets
            foreach (var wallet in wallets)
            {
                Console.WriteLine(wallet);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    ```
    - The `IDbContextFactory<AppDbContext>` service is retrieved from the service provider, and a `DbContext` instance is created using `CreateDbContext`. The context is used within a `using` block to ensure proper disposal.

## Conclusion

This example demonstrates how to configure and use a `DbContextFactory` in Entity Framework Core. This approach is particularly useful for scenarios where you need to create `DbContext` instances outside the typical dependency injection scope, ensuring proper context management and disposal.
