
# Using DbContext Pooling in Entity Framework Core

This guide demonstrates how to use `DbContext` pooling in Entity Framework Core to improve performance and resource management. The provided code example shows how to configure `DbContext` pooling, read configuration settings, and perform database operations efficiently.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Entity Framework Core packages (can be installed via NuGet)

## Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `AppDbContext.cs`
- `Wallet.cs`
- `appsettings.json`

### `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YourServer;Database=YourDatabase;Integrated Security=True;"
  }
}
```

### Entity Class

The `Wallet` class represents the entity in the database.

```csharp
namespace _07_UsingContextPooling.Entities
{
    public class Wallet
    {
        public int Id { get; set; } // Primary key
        public string Holder { get; set; } // Wallet holder's name
        public decimal Balance { get; set; } // Wallet balance

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

### DbContext Class

The `AppDbContext` class represents the database context and is used to interact with the database.

```csharp
using Microsoft.EntityFrameworkCore;
using _07_UsingContextPooling.Entities;

namespace _07_UsingContextPooling.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; } // Represents the Wallets table in the database

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
```

## Step-by-Step Guide

### 1. Build Configuration

The configuration is built to read from `appsettings.json`:

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
    .Build(); // Build the configuration

// Get the connection string from the configuration
var connectionString = configuration.GetConnectionString("DefaultConnection");
```

### 2. Create Service Collection and Register Services

A service collection is created and services are registered, including the `DbContext` with pooling enabled:

```csharp
var services = new ServiceCollection();

// Register the services
services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));

// Create the service provider
IServiceProvider serviceProvider = services.BuildServiceProvider();
```

### 3. Use the DbContext

The `AppDbContext` service is retrieved from the service provider, and database operations are performed:

```csharp
using (var context = serviceProvider.GetService<AppDbContext>())
{
    try
    {
        var wallets = context.Wallets;

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

## Explanation of Key Points

1. **Building Configuration**:
    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - This code builds the configuration to read settings from the `appsettings.json` file, which includes the connection string.

2. **Creating Service Collection**:
    ```csharp
    var services = new ServiceCollection();
    ```
    - This creates a new `ServiceCollection` to register services needed by the application.

3. **Registering DbContext with Pooling**:
    ```csharp
    services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));
    ```
    - This registers the `AppDbContext` with pooling enabled, using the connection string from the configuration.

4. **Creating Service Provider**:
    ```csharp
    IServiceProvider serviceProvider = services.BuildServiceProvider();
    ```
    - This builds the service provider to resolve services like `AppDbContext`.

5. **Using the DbContext**:
    ```csharp
    using (var context = serviceProvider.GetService<AppDbContext>())
    {
        // Perform database operations
    }
    ```
    - This retrieves the `AppDbContext` instance from the service provider and uses it to perform database operations.

### Benefits of DbContext Pooling

- **Performance**: Reduces the overhead of creating and disposing of `DbContext` instances.
- **Resource Management**: Efficiently manages the context instances, reducing resource consumption.

### Summary

This guide demonstrates how to configure and use `DbContext` pooling in Entity Framework Core. By following these steps, you can improve the performance and resource management of your application when interacting with the database.
