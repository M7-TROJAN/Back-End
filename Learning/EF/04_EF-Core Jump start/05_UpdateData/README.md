
# Updating Data with Entity Framework Core

This guide demonstrates how to update an existing entity in the database using Entity Framework Core in a C# console application. 

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
namespace _05_UpdateData
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

namespace _05_UpdateData
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

Here is the complete code for updating a wallet's balance with detailed comments explaining each step:

```csharp
namespace _05_UpdateData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Update the balance of the wallet with ID 13 to $5,000.00
                    // [13] Rana ($4,500.00) -> Rana ($5,000.00)

                    // 1. Find the entity by its primary key
                    var wallet = context.Wallets.Find(13);

                    // Check if the wallet exists
                    if (wallet == null)
                    {
                        Console.WriteLine("Wallet not found");
                        return;
                    }

                    // 2. Update the entity's property
                    wallet.Balance = 5000;

                    // 3. Save the changes to the database
                    context.SaveChanges();

                    Console.WriteLine("Data updated successfully!");
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
    var wallet = context.Wallets.Find(13);
    ```
    - This line retrieves the `Wallet` entity with the primary key value of 13 from the database. If the entity is found, it is tracked by the context.

2. **Checking if the Entity Exists**:
    ```csharp
    if (wallet == null)
    {
        Console.WriteLine("Wallet not found");
        return;
    }
    ```
    - This checks if the entity was found. If not, a message is displayed, and the program exits.

3. **Updating the Entity**:
    ```csharp
    wallet.Balance = 5000;
    ```
    - This updates the `Balance` property of the retrieved `Wallet` entity.

4. **Saving the Changes**:
    ```csharp
    context.SaveChanges();
    ```
    - This saves the changes made to the entity back to the database.

### Entity Tracking in Entity Framework Core

Entity Framework Core tracks the changes made to entities that are retrieved from the database. This means when you modify an entity's property, EF Core is aware of the change. When `SaveChanges()` is called, EF Core generates the appropriate SQL statements to update the database accordingly. This tracking mechanism allows EF Core to manage entity state transitions and ensure data consistency.

### Summary

This guide demonstrates how to update an entity in the database using Entity Framework Core. By following these steps, you can efficiently update records while leveraging EF Core's tracking capabilities to ensure data integrity and consistency.
