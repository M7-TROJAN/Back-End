# AppDbContext Example with Parameterless Constructor (Internal Configuration)

This example demonstrates how to create a `DbContext` class in Entity Framework Core with a parameterless constructor. The context is configured to read the connection string from an `appsettings.json` file and use SQL Server as the database provider.

## Sample Data

Assume that this class represents a `Wallet` entity in the database.

```csharp
namespace _01_InternalConfiguration.Entities
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

The `AppDbContext` class is configured to read from an `appsettings.json` file and use SQL Server.

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

The `AppDbContext` class reads the connection string from the configuration file and uses it to configure the database provider.

```csharp
using _01_InternalConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _01_InternalConfiguration.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure the DbContext to use SQL Server with the connection string
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
```

## Explanation

1. **DbSet Property**: The `Wallets` property of type `DbSet<Wallet>` is used to query and save instances of the `Wallet` entity.

    ```csharp
    public DbSet<Wallet> Wallets { get; set; } = null!;
    ```

2. **OnConfiguring Method**: The `OnConfiguring` method is overridden to configure the database provider and connection string.

    ```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Build the configuration to read from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
            .Build(); // Build the configuration

        // Get the connection string from the configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configure the DbContext to use SQL Server with the connection string
        optionsBuilder.UseSqlServer(connectionString);
    }
    ```

3. **Configuration**: The `ConfigurationBuilder` is used to build the configuration by adding the `appsettings.json` file. The connection string is then retrieved from the configuration.

    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    ```

4. **DbContext Options**: The `optionsBuilder` is used to configure the `DbContext` to use SQL Server with the retrieved connection string.

    ```csharp
    optionsBuilder.UseSqlServer(connectionString);
    ```

## Conclusion

This example demonstrates how to configure a `DbContext` with a parameterless constructor to read connection settings from an external configuration file. This approach promotes separation of concerns and makes the application configuration more flexible and manageable.
