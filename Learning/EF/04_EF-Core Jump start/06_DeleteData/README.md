# Deleting Data with Entity Framework Core

This guide demonstrates how to delete an existing entity from the database using Entity Framework Core in a C# console application.

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
    "DefaultConnection": "Server=M7_TROJAN;Database=DigitalCurrency;Integrated Security=SSPI;TrustServerCertificate=True"
  }
}
```

### 3. Entity Class

The `Wallet` class represents the entity in the database.

```csharp
namespace _06_DeleteData
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

The `AppDbContext` class represents the database context and is used to interact with the database.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _06_DeleteData
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; } // Represents the Wallets table in the database

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Load configuration from appsettings.json
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString); // Configure DbContext to use SQL Server
        }
    }
}
```

### 5. C# Code Explanation

Here is the complete code for deleting a wallet with detailed comments explaining each step:

```csharp
namespace _06_DeleteData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Delete wallet with ID 9
                    // [9] Target wallet to be deleted

                    // 1. Find the entity by its primary key
                    var wallet = context.Wallets.Find(9);

                    // Check if the wallet exists
                    if (wallet != null)
                    {
                        // 2. Remove the entity from the context
                        context.Wallets.Remove(wallet);

                        // 3. Save the changes to the database
                        context.SaveChanges();

                        Console.WriteLine("Wallet deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Wallet not found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
```

### Explanation of Key Points

1. **Finding the Entity**:
    ```csharp
    var wallet = context.Wallets.Find(9);
    ```
    - This retrieves the `Wallet` entity with the primary key value of 9 from the database.

2. **Checking if the Entity Exists**:
    ```csharp
    if (wallet != null)
    {
        // ...
    }
    else
    {
        Console.WriteLine("Wallet not found");
    }
    ```
    - This checks if the entity was found. If not, a message is displayed indicating the wallet was not found.

3. **Removing the Entity**:
    ```csharp
    context.Wallets.Remove(wallet);
    ```
    - This removes the `Wallet` entity from the context. The entity is marked for deletion.

4. **Saving the Changes**:
    ```csharp
    context.SaveChanges();
    ```
    - This commits the changes made to the context to the database, effectively deleting the entity.

### Entity Tracking in Entity Framework Core

Entity Framework Core tracks entities that are retrieved from the database. When you call `context.Wallets.Remove(wallet)`, EF Core marks the entity as `Deleted` in the context. Calling `SaveChanges()` then generates and executes the necessary SQL commands to remove the entity from the database.

### Summary

This guide demonstrates how to delete an entity in the database using Entity Framework Core. By following these steps, you can efficiently remove records while leveraging EF Core’s change tracking and entity state management capabilities.
