# Configuring AppDbContext Externally Using DbContextOptions

This example demonstrates how to configure the `AppDbContext` class in Entity Framework Core using `DbContextOptions`. This approach allows you to manage the configuration of the database context externally, promoting better separation of concerns and flexibility.

## Sample Data

Assume that this class represents a `Wallet` entity in the database.

```csharp
namespace _02_ExternalConfiguration.Entities
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

The `AppDbContext` class is configured to use `DbContextOptions` for its configuration.

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

The `AppDbContext` class accepts `DbContextOptions` in its constructor.

```csharp
using _02_ExternalConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _02_ExternalConfiguration.Data
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

The main program builds the configuration, retrieves the connection string, and creates the `DbContextOptions` object to pass to the `AppDbContext` constructor.

```csharp
using _02_ExternalConfiguration.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _02_ExternalConfiguration
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

            // Create the DbContextOptions object with the connection string
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            // Initialize the context with the options
            using (var context = new AppDbContext(options))
            {
                var wallets = context.Wallets;

                // Display the wallets
                foreach (var wallet in wallets)
                {
                    Console.WriteLine(wallet);
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

4. **DbContext Options**: The `DbContextOptionsBuilder` is used to configure the `DbContext` to use SQL Server with the retrieved connection string.

    ```csharp
    var options = new DbContextOptionsBuilder()
        .UseSqlServer(connectionString)
        .Options;
    ```

5. **Initialize Context**: The `AppDbContext` is initialized with the `DbContextOptions` object.

    ```csharp
    using (var context = new AppDbContext(options))
    {
        var wallets = context.Wallets;

        // Display the wallets
        foreach (var wallet in wallets)
        {
            Console.WriteLine(wallet);
        }
    }
    ```

## Conclusion

This example demonstrates how to configure a `DbContext` externally using `DbContextOptions`. This approach allows for more flexible and manageable configuration, promoting better separation of concerns in your application.