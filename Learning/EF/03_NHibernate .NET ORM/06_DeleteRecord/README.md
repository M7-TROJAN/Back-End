
# Deleting a Wallet Record Using NHibernate

This guide demonstrates how to set up NHibernate and delete a `Wallet` entity from a database.

## Prerequisites

- .NET SDK
- SQL Server
- `appsettings.json` file with the connection string
- Required NuGet packages:
  - `Microsoft.Extensions.Configuration`
  - `Microsoft.Extensions.Configuration.Json`
  - `Microsoft.Data.SqlClient`
  - `NHibernate`

## Step-by-Step Guide

### 1. Project Structure

Ensure your project structure includes the following files:
- `Program.cs`
- `appsettings.json`
- `Wallet.cs`
- `WalletMapping.cs`

### 2. Install NuGet Packages

You can install the required packages via the NuGet Package Manager:

```bash
Install-Package Microsoft.Extensions.Configuration
Install-Package Microsoft.Extensions.Configuration.Json
Install-Package Microsoft.Data.SqlClient
Install-Package NHibernate
```

### 3. `appsettings.json`

Create an `appsettings.json` file in the root of your project with the following content:

```json
{
  "constr": "Server=.;Database=DigitalCurrency;Integrated Security=SSPI;TrustServerCertificate=True"
}
```

### 4. Entity Definition

Create a `Wallet.cs` file to define the `Wallet` entity:

```csharp
namespace NHibernate_01_CreateSession
{
    public class Wallet
    {
        public virtual int Id { get; set; }
        public virtual string Holder { get; set; } = null!;
        public virtual decimal Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
```

### 5. Mapping Definition

Create a `WalletMapping.cs` file to define the mapping for the `Wallet` entity:

```csharp
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate_01_CreateSession
{
    public class WalletMapping : ClassMapping<Wallet>
    {
        public WalletMapping()
        {
            Id(x => x.Id, configuration => {
                configuration.Generator(Generators.Identity);
                configuration.Type(NHibernateUtil.Int32);
                configuration.Column("Id");
                configuration.UnsavedValue(0);
            });
            
            Property(x => x.Holder, c =>
            {
                c.Length(100);
                c.Type(NHibernateUtil.AnsiString);
                c.NotNullable(true);
            });

            Property(x => x.Balance, c =>
            {
                c.Type(NHibernateUtil.Decimal);
                c.NotNullable(true);
            });

            Table("Wallets");
        }
    }
}
```

### 6. Main Program

Create a `Program.cs` file with the following content to set up NHibernate and delete a `Wallet` entity:

```csharp
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using System;

namespace _06_DeleteRecord
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(var session = CreateSession())
            {
                using(var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var idToDelete = 11;

                        var wallet = session.Get<Wallet>(idToDelete);

                        if(wallet == null)
                        {
                            Console.WriteLine("Wallet not found");
                            return;
                        }

                        // else delete the wallet
                        session.Delete(wallet);

                        // Flush the session to apply the delete operation
                        session.Flush();

                        transaction.Commit();
                        Console.WriteLine($"Wallet with id {idToDelete} deleted");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        private static ISession CreateSession()
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetSection("constr").Value;

            var mapper = new ModelMapper();

            // List all of type mappings from the assembly
            mapper.AddMappings(typeof(Wallet).Assembly.ExportedTypes);

            // Compile class mapping
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            // Allow the application to specify properties and mapping documents to be used when creating
            var hbconfig = new NHibernate.Cfg.Configuration();

            // Settings from app to hibernate
            hbconfig.DataBaseIntegration(c =>
            {
                // Strategy to interact with the database
                c.Driver<MicrosoftDataSqlClientDriver>();

                // Dialect for the database
                c.Dialect<MsSql2012Dialect>();

                // Connection string
                c.ConnectionString = connectionString;

                // Log SQL statements to console (useful for debugging) optional
                // c.LogSqlInConsole = true;

                // Format logged SQL statements (optional)
                // c.LogFormattedSql = true;
            });

            // Add mapping to NHibernate configuration
            hbconfig.AddMapping(domainMapping);

            // Instantiate a new ISessionFactory (use properties, settings, and mappings)
            ISessionFactory sessionFactory = hbconfig.BuildSessionFactory();

            // Open a new session
            ISession session = sessionFactory.OpenSession();

            return session;
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

2. **Database Connection String**:
    ```csharp
    var connectionString = configuration.GetSection("constr").Value;
    ```
    - This retrieves the connection string from the configuration.

3. **Mapping Configuration**:
    ```csharp
    var mapper = new ModelMapper();
    mapper.AddMappings(typeof(Wallet).Assembly.ExportedTypes);
    HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
    ```
    - This sets up NHibernate to use the mappings defined in the assembly.

4. **NHibernate Configuration**:
    ```csharp
    var hbconfig = new NHibernate.Cfg.Configuration();
    hbconfig.DataBaseIntegration(c =>
    {
        c.Driver<MicrosoftDataSqlClientDriver>();
        c.Dialect<MsSql2012Dialect>();
        c.ConnectionString = connectionString;
    });
    hbconfig.AddMapping(domainMapping);
    ```
    - This configures NHibernate with database settings, including the driver, dialect, and connection string.

5. **Session Creation**:
    ```csharp
    ISessionFactory sessionFactory = hbconfig.BuildSessionFactory();
    ISession session = sessionFactory.OpenSession();
    ```
    - This builds the session factory and opens a new session for database operations.

6. **Deleting Data**:
    ```csharp
    using(var session = CreateSession())
    {
        using(var transaction = session.BeginTransaction())
        {
            try
            {
                var idToDelete = 11;

                var wallet = session.Get<Wallet>(idToDelete);

                if(wallet == null)
                {
                    Console.WriteLine("Wallet not found");
                    return;
                }

                // else delete the wallet
                session.Delete(wallet);

                // Flush the session to apply the delete operation
                session.Flush();

                transaction.Commit();
                Console.WriteLine($"Wallet with id {idToDelete} deleted");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
            }
        }
    }
    ```
    - This code retrieves an existing `Wallet` entity by its ID, deletes it, and commits the transaction to persist the changes in the database. It handles any exceptions that may occur during the process.

By following these steps, you can set up NHibernate in a .NET console application and delete existing data from a database.

### Conclusion

This guide provides a comprehensive overview of setting up NHibernate in a .NET console application, including configuration, mapping, session creation, and data deletion. With this setup, you can start integrating NHibernate into your projects to facilitate object-relational mapping and data access.

## Resources

- [NHibernate Documentation](https://nhibernate.info/doc/)
