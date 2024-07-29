
# Querying Data with Entity Framework Core

This guide demonstrates how to query data from a database using Entity Framework Core in a C# console application.

## Prerequisites

- .NET SDK
- Entity Framework Core with SQL Server
- `AppDbContext` class configured for the database

## Sample Data

Assume we have a `Wallet` class representing a table in the database:

```csharp
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
```

### Code Example

Here is a C# console application demonstrating how to query wallets with a balance greater than 4000:

```csharp
namespace _07_QueryData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Ensure the database schema is created
                context.Database.EnsureCreated();

                // Query to get wallets where balance > 4000
                // Method syntax example
                var wallets = context.Wallets.Where(w => w.Balance > 4000);

                // Alternative query using LINQ query syntax
                // var wallets = from w in context.Wallets
                //               where w.Balance > 4000
                //               select w;

                // Optional: Order the result by Holder
                // var wallets = context.Wallets
                //                        .Where(w => w.Balance > 4000)
                //                        .OrderBy(w => w.Holder);

                Console.WriteLine("Wallets with balance > 4000:");

                // Iterate over the result set and display each wallet
                foreach (var wallet in wallets)
                {
                    Console.WriteLine(wallet);
                }
            }
        }
    }
}
```

### Explanation of Key Points

1. **Ensure Database Creation**
   ```csharp
   context.Database.EnsureCreated();
   ```
   - Ensures that the database schema is created if it does not already exist. This is useful for initial setup but should be replaced with migrations for ongoing schema changes.

2. **Query Data**
   ```csharp
   var wallets = context.Wallets.Where(w => w.Balance > 4000);
   ```
   - Retrieves wallets where the balance is greater than 4000. This uses LINQ method syntax to construct the query.

   ```csharp
   // Alternative LINQ query syntax
   // var wallets = from w in context.Wallets
   //               where w.Balance > 4000
   //               select w;
   ```
   - An alternative way to write the same query using LINQ query syntax.

3. **Ordering Results**
   ```csharp
   // Optional: Order the result by Holder
   // var wallets = context.Wallets
   //                        .Where(w => w.Balance > 4000)
   //                        .OrderBy(w => w.Holder);
   ```
   - This example shows how to order the results by the `Holder` property. The `OrderBy` method is used to sort the results.

4. **Display Results**
   ```csharp
   foreach (var wallet in wallets)
   {
       Console.WriteLine(wallet);
   }
   ```
   - Iterates over the queried wallets and prints each one. The `ToString` method of the `Wallet` class is used to format the output.

### Best Practices

- **Use Migrations**: For evolving schema changes, use EF Core Migrations instead of `EnsureCreated()`.
- **Optimize Queries**: Be mindful of performance; use projection (`Select`), filtering, and ordering to retrieve only the necessary data.
- **Error Handling**: Consider adding error handling for database operations to manage exceptions and provide better user feedback.

This example demonstrates querying data with EF Core and handling various query requirements. Adjust the queries and methods according to your application's specific needs.