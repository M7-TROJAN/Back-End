When considering the best practices and performance for querying data in Entity Framework Core, it's important to understand the implications of each approach. Here's a detailed comparison of the three options you've mentioned:

## Sample Data

Assume that this class represents a table in the database:

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

```csharp
internal class AppDbContext : DbContext
{
    // Represent the collection of all entities in the database
    public DbSet<Wallet> Wallets { get; set; }

    // DbSet<T> is a property that represents a table in the database.
    // if you have 60 tables, you need to have 60 DbSet<T> properties.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);
    }
}
```

### 1. `var wallets = context.Wallets;`

This approach returns an `IQueryable<Wallet>` without executing the query against the database immediately. The query will only be executed when the data is actually accessed (e.g., during enumeration or when calling a method like `ToList()`).

- **Best Practice**: This approach is useful if you plan to further filter, sort, or project the data using LINQ before executing the query. This way, you can compose more complex queries before hitting the database.
- **Performance**: This can be efficient as it delays the execution until necessary, potentially reducing the amount of data retrieved from the database if additional LINQ operators are applied.

### 2. `var wallets = context.Wallets.ToList();`

This approach executes the query against the database immediately and retrieves all the records from the `Wallets` table into memory as a `List<Wallet>`.

- **Best Practice**: Use this approach when you need to work with the entire set of data in memory immediately, and you're certain the amount of data is manageable.
- **Performance**: This can be less efficient if the `Wallets` table contains a large number of records, as it retrieves all the data into memory at once. It is best to avoid this when dealing with large datasets to prevent high memory usage and potential performance issues.

### 3. `var wallets = context.Wallets.AsQueryable();`

This approach also returns an `IQueryable<Wallet>` similar to the first option, allowing further composition of queries before execution.

- **Best Practice**: This is essentially the same as the first option in terms of usage and benefits. Use it when you want to build complex queries dynamically.
- **Performance**: Similar to the first option, it delays execution and allows for efficient query composition.

### Recommendation

For the best performance and flexibility, you should generally prefer to work with `IQueryable<Wallet>` until you need to execute the query. Here are some guidelines:

- **Start with `IQueryable`**: Begin with `var wallets = context.Wallets;` or `var wallets = context.Wallets.AsQueryable();`. This allows you to build and compose your query dynamically.
- **Filter Early**: Apply any filtering, sorting, and projection operations before executing the query. This ensures that only the necessary data is retrieved from the database.
- **Execute Query When Needed**: Use `ToList()`, `FirstOrDefault()`, `SingleOrDefault()`, or other methods that execute the query only when you need to work with the data.

### Example

Here's an example combining best practices:

```csharp
using (var context = new AppDbContext())
{
    // Start with IQueryable to build a query
    var walletsQuery = context.Wallets;

    // Apply filtering and sorting
    walletsQuery = walletsQuery.Where(w => w.Balance > 1000).OrderBy(w => w.Holder);

    // Execute the query and retrieve data into memory only when needed
    var wallets = walletsQuery.ToList();

    // Process the retrieved data
    foreach (var wallet in wallets)
    {
        Console.WriteLine(wallet);
    }
}
```

### Key Points

- **`IQueryable` for Query Composition**: Start with `IQueryable` to compose your query dynamically.
- **Apply Filters Early**: Use LINQ methods to filter and sort the data before executing the query.
- **Execute When Necessary**: Use methods like `ToList()` to execute the query and retrieve data only when you need to work with it.

By following these guidelines, you can achieve both best performance and best practice in your Entity Framework Core queries.