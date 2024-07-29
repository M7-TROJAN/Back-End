# Basic Setup and Usage of Entity Framework Core with SQL Server

This guide demonstrates how to set up and use Entity Framework Core (EF Core) with SQL Server in a C# console application.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- EF Core libraries (can be installed via NuGet)

### NuGet Packages

Make sure to install the following NuGet packages:
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Configuration.Json`

## Step-by-Step Guide

### 1. Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `appsettings.json`
- `Wallet.cs`
- `AppDbContext.cs`

### 2. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=M7_TROJAN;Database=DigitalCurrency;Integrated Security=SSPI;TrustServerCertificate=True"
  }
}
```
This file contains the database connection string required by EF Core to connect to the SQL Server database.

### 3. `Wallet.cs` File

Create a `Wallet.cs` file to represent a table in the database:

```csharp
namespace BasicEFSetup
{
    public class Wallet
    {
        // Property to store the wallet ID
        public int Id { get; set; } 
        
        // Property to store the wallet holder's name
        public string Holder { get; set; } = null!; 
        
        // Property to store the wallet balance
        public decimal Balance { get; set; } 

        // Override the ToString method to provide a string representation of the Wallet object
        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

### 4. `AppDbContext.cs` File

Create an `AppDbContext.cs` file to represent the database context:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BasicEFSetup
{
    internal class AppDbContext : DbContext
    {
        // Represent the collection of all Wallet entities in the database
        public DbSet<Wallet> Wallets { get; set; } 

        // DbSet<T> is a property that represents a table in the database.
        // If you have 60 tables, you need to have 60 DbSet<T> properties.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

### 5. `Program.cs` File

Create a `Program.cs` file to perform basic CRUD operations:

```csharp
using System;
using System.Linq;

namespace BasicEFSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            // Create a new Wallet object
            var newWallet = new Wallet { Holder = "Mansour", Balance = 7400m };

            // Add the new Wallet object to the DbContext
            context.Wallets.Add(newWallet);

            // Save changes to the database
            context.SaveChanges();

            // Retrieve all Wallet objects from the database
            var wallets = context.Wallets.ToList();

            // Print out each Wallet object
            foreach (var wallet in wallets)
            {
                Console.WriteLine(wallet);
            }
        }
    }
}
```

### Explanation of Key Points

1. **Configuration Setup**:
    ```csharp
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - This sets up the configuration to read settings from `appsettings.json`.

2. **Database Connection**:
    ```csharp
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    optionsBuilder.UseSqlServer(connectionString);
    ```
    - This gets the connection string from the configuration and configures the `DbContext` to use SQL Server with that connection string.

3. **DbSet<T> Property**:
    ```csharp
    public DbSet<Wallet> Wallets { get; set; }
    ```
    - `DbSet<T>` is a property that represents a table in the database. If you have multiple tables, you need multiple `DbSet<T>` properties.

4. **Creating Wallet Object**:
    ```csharp
    var newWallet = new Wallet { Holder = "Mansour", Balance = 7400m };
    ```
    - This defines a `Wallet` object with the values to be inserted into the database.

5. **Adding and Saving Changes**:
    ```csharp
    context.Wallets.Add(newWallet);
    context.SaveChanges();
    ```
    - `Add` method adds the new `Wallet` object to the `DbContext`, and `SaveChanges` method saves the changes to the database.

6. **Retrieving Data**:
    ```csharp
    var wallets = context.Wallets.ToList();
    ```
    - This retrieves all `Wallet` objects from the database.

7. **Printing Data**:
    ```csharp
    foreach (var wallet in wallets)
    {
        Console.WriteLine(wallet);
    }
    ```
    - This loops through the retrieved `Wallet` objects and prints each one.

This guide provides a basic setup and usage of Entity Framework Core with SQL Server, including configuration, context setup, and performing basic CRUD operations. Feel free to expand on this with more advanced features and operations as you continue learning EF Core. If you have any specific questions or need further examples, feel free to ask!
```