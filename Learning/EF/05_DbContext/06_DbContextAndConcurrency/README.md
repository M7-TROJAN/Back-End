
# Handling DbContext and Concurrency(التزامن) in Entity Framework Core

This guide demonstrates how to handle concurrency when using `DbContext` in parallel tasks. It also shows how to manage `DbContext` lifetime and avoid common issues such as invalid operation exceptions.

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
namespace _06_DbContextAndConcurrency.Entities
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

### `AppDbContext.cs`

The `AppDbContext` class is configured to use `DbContextOptions` for its configuration.

```csharp
using Microsoft.EntityFrameworkCore;

namespace _06_DbContextAndConcurrency.Data
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

## Initial Code with Concurrency Issue

The following code attempts to execute two jobs in parallel using the same `DbContext` instance, which leads to an invalid operation exception.

```csharp
using _06_DbContextAndConcurrency.Data;
using _06_DbContextAndConcurrency.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _06_DbContextAndConcurrency
{
    internal class Program
    {
        static AppDbContext dbContext;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            // now we want to execute two jobs in parallel at the same time

            // JobOne: Add a new Wallet
            Wallet wallet = new Wallet
            {
                Holder = "Jasem",
                Balance = 500
            };

            // JobTwo: Add a new Wallet
            Wallet wallet2 = new Wallet
            {
                Holder = "Heba",
                Balance = 1000
            };

            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => JobOne(wallet)),
                Task.Factory.StartNew(() => JobTwo(wallet2))
            };

            Task.WhenAll(tasks).ContinueWith((t) =>
            {
               Console.WriteLine("JobOne & JobTwo are Executed Concurrently!");
            });

            // This code will throw an exception (invalid operation exception)
            // because the two jobs are executed in parallel and both try to access the same dbContext instance at the same time

            Console.ReadKey();
        }

        static void JobOne(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);
            dbContext.SaveChanges();
        }

        static void JobTwo(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);
            dbContext.SaveChanges();
        }
    }
}
```

### Explanation of Key Points

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

3. **Service Collection**:
    ```csharp
    var services = new ServiceCollection();
    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    IServiceProvider serviceProvider = services.BuildServiceProvider();
    dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    ```
    - The `ServiceCollection` is used to register the `AppDbContext` with dependency injection. The `DbContext` is configured to use SQL Server with the specified connection string. The `IServiceProvider` is built to provide the required services, including the `AppDbContext`.

4. **Concurrency Issue**:
    - The code tries to execute two parallel tasks (`JobOne` and `JobTwo`) that both use the same `DbContext` instance. This leads to an invalid operation exception because `DbContext` is not thread-safe and cannot be used concurrently across multiple threads.

## Solution: Separate DbContext Instances

To solve the concurrency issue, create a separate `DbContext` instance for each job.

```csharp
using _06_DbContextAndConcurrency.Data;
using _06_DbContextAndConcurrency.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _06_DbContextAndConcurrency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // JobOne: Add a new Wallet
            Wallet wallet = new Wallet
            {
                Holder = "Jasem",
                Balance = 500
            };

            // JobTwo: Add a new Wallet
            Wallet wallet2 = new Wallet
            {
                Holder = "Heba",
                Balance = 1000
            };

            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => JobOne(wallet)),
                Task.Factory.StartNew(() => JobTwo(wallet2))
            };

            Task.WhenAll(tasks).ContinueWith((t) =>
            {
               Console.WriteLine("JobOne & JobTwo are Executed Concurrently!");
            });

            Console.ReadKey();
        }

        static void JobOne(Wallet wallet)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            dbContext.Wallets.Add(wallet);
            dbContext.SaveChanges();
        }

        static void JobTwo(Wallet wallet)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            dbContext.Wallets.Add(wallet);
            dbContext.SaveChanges();
        }
    }
}
```

### Explanation of Key Points

- **Separate DbContext Instances**:
    - In each job, a new `DbContext` instance is created. This ensures that each job has its own `DbContext` instance, preventing concurrency issues.

## Improved Solution: Using Async and Await

To further improve the solution, use asynchronous methods to execute the tasks concurrently without blocking the main thread.

```csharp
using _06_DbContextAndConcurrency.Data;
using _06_DbContextAndConcurrency.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _06_DbContextAndConcurrency
{
    internal class Program
    {
        static AppDbContext dbContext;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            // JobOne: Add a new Wallet
            Wallet wallet = new Wallet
            {
                Holder = "Jasem",
                Balance = 500
            };

            // JobTwo: Add a new Wallet
            Wallet wallet2 = new Wallet
            {
                Holder = "Heba",
                Balance = 1000
            };

            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => JobOne(wallet)),
                Task.Factory.StartNew(() => JobTwo(wallet2))
            };

            Task.WhenAll(tasks).Continue

With((t) =>
            {
               Console.WriteLine("JobOne & JobTwo are Executed Concurrently!");
            });

            Console.ReadKey();
        }

        static async Task JobOne(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);
            await dbContext.SaveChangesAsync();
        }

        static async Task JobTwo(Wallet wallet)
        {
            dbContext.Wallets.Add(wallet);
            await dbContext.SaveChangesAsync();
        }
    }
}
```

### Explanation of Key Points

1. **Asynchronous Methods**:
    ```csharp
    static async Task JobOne(Wallet wallet)
    {
        dbContext.Wallets.Add(wallet);
        await dbContext.SaveChangesAsync();
    }

    static async Task JobTwo(Wallet wallet)
    {
        dbContext.Wallets.Add(wallet);
        await dbContext.SaveChangesAsync();
    }
    ```
    - The `JobOne` and `JobTwo` methods are updated to be asynchronous. The `async` keyword is used to mark the methods as asynchronous, and the `await` keyword is used to asynchronously save changes to the database.

2. **Task Factory**:
    ```csharp
    var tasks = new Task[]
    {
        Task.Factory.StartNew(() => JobOne(wallet)),
        Task.Factory.StartNew(() => JobTwo(wallet2))
    };

    Task.WhenAll(tasks).ContinueWith((t) =>
    {
       Console.WriteLine("JobOne & JobTwo are Executed Concurrently!");
    });
    ```
    - The `Task.Factory.StartNew` method is used to start the tasks concurrently. The `Task.WhenAll` method waits for all tasks to complete before printing a message to the console.

## Summary

This guide demonstrates how to handle concurrency issues when using `DbContext` in parallel tasks. By creating separate `DbContext` instances for each task or using asynchronous methods, you can avoid common concurrency issues and improve the performance of your application.
