
# Configuring DbContext Using Dependency Injection

This example demonstrates how to configure the `AppDbContext` class in Entity Framework Core using dependency injection (DI). This approach is considered best practice in modern .NET applications, promoting better separation of concerns, testability, and maintainability.

## Sample Data

Assume that this class represents a `Wallet` entity in the database.

```csharp
namespace _03_UsingDependancyInjection.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

## DbContext Configuration

The `AppDbContext` class is configured to use `DbContextOptions` for its configuration, supporting dependency injection.

### appsettings.json

Create an `appsettings.json` file with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YourServer;Database=YourDatabase;Integrated Security=True;"
  }
}
```

### DbContext Class

The `AppDbContext` class accepts `DbContextOptions` in its constructor, allowing it to be configured externally and injected where needed.

```csharp
using _03_UsingDependancyInjection.Entities;
using Microsoft.EntityFrameworkCore;

namespace _03_UsingDependancyInjection.Data
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

### Main Program

The main program builds the configuration, retrieves the connection string, configures the services for dependency injection, and uses the `AppDbContext` through the service provider.

```csharp
using _03_UsingDependancyInjection.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _03_UsingDependancyInjection
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

            var services = new ServiceCollection();

            // Register the services
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // Create the service provider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get the AppDbContext service
            using (var context = serviceProvider.GetService<AppDbContext>())
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

## Explanation

1. **DbSet Property**: The `Wallets` property of type `DbSet<Wallet>` is used to query and save instances of the `Wallet` entity.

    ```csharp
    public DbSet<Wallet> Wallets { get; set; } = null!;
    ```

2. **Constructor**: The `AppDbContext` constructor accepts `DbContextOptions` and passes it to the base `DbContext` constructor.

    ```csharp
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    ```

3. **Configuration**: The `ConfigurationBuilder` is used to build the configuration by adding the `appsettings.json` file. The connection string is then retrieved from the configuration.

    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    ```

4. **Service Collection**: A `ServiceCollection` is created and the `AppDbContext` is registered with the connection string configuration using `AddDbContext`.

    ```csharp
    var services = new ServiceCollection();
    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    ```

5. **Service Provider**: The service provider is built from the service collection.

    ```csharp
    IServiceProvider serviceProvider = services.BuildServiceProvider();
    ```

6. **Using Context**: The `AppDbContext` is retrieved from the service provider and used within a `using` block to ensure proper disposal.

    ```csharp
    using (var context = serviceProvider.GetService<AppDbContext>())
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

## Conclusion

This example demonstrates how to configure a `DbContext` using dependency injection. This approach leverages the built-in dependency injection framework in .NET, promoting better separation of concerns and flexibility in managing configurations.