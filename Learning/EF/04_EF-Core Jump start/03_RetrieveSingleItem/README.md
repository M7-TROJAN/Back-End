In Entity Framework Core, there are several ways to retrieve a specific item from the database. The best method depends on your specific use case and performance considerations. Below are some common methods, including `Find`, `FirstOrDefault`, `SingleOrDefault`, `First`, and `Single`, along with their benefits and use cases.

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

### 1. `Find`

The `Find` method is used to retrieve an entity by its primary key. It is efficient because it can return the entity from the context's cache if it exists, avoiding a database query.

```csharp
var wallet = context.Wallets.Find(1);
```

- **Use Case**: When you need to retrieve an entity by its primary key.
- **Performance**: Efficient due to potential use of the context's cache.

### 2. `FirstOrDefault`

The `FirstOrDefault` method retrieves the first entity that matches the given criteria or returns `null` if no entity is found.

```csharp
var wallet = context.Wallets.FirstOrDefault(w => w.Id == 1);
```

- **Use Case**: When you want to retrieve the first entity matching specific criteria, with the possibility of no match.
- **Performance**: Efficient for simple queries, but can result in multiple database calls if not optimized.

### 3. `SingleOrDefault`

The `SingleOrDefault` method retrieves the only entity that matches the given criteria or returns `null` if no entity is found. It throws an exception if more than one entity matches the criteria.

```csharp
var wallet = context.Wallets.SingleOrDefault(w => w.Id == 1);
```

- **Use Case**: When you expect exactly one entity to match the criteria or no match.
- **Performance**: Efficient for ensuring uniqueness but can be slower than `Find` due to the additional check for multiple matches.

### 4. `First`

The `First` method retrieves the first entity that matches the given criteria and throws an exception if no entity is found.

```csharp
var wallet = context.Wallets.First(w => w.Id == 1);
```

- **Use Case**: When you want to retrieve the first entity matching specific criteria and expect a match.
- **Performance**: Similar to `FirstOrDefault`, but throws an exception if no match is found.

### 5. `Single`

The `Single` method retrieves the only entity that matches the given criteria and throws an exception if no entity or more than one entity matches the criteria.

```csharp
var wallet = context.Wallets.Single(w => w.Id == 1);
```

- **Use Case**: When you expect exactly one entity to match the criteria.
- **Performance**: Ensures uniqueness but can be slower due to the checks for no match and multiple matches.

### Example Comparison

Here's an example comparing the different methods:

```csharp
using (var context = new AppDbContext())
{
    try
    {
        // Using Find
        var walletFind = context.Wallets.Find(1);
        Console.WriteLine(walletFind is null ? "Wallet not found" : walletFind.ToString());

        // Using FirstOrDefault
        var walletFirstOrDefault = context.Wallets.FirstOrDefault(w => w.Id == 1);
        Console.WriteLine(walletFirstOrDefault is null ? "Wallet not found" : walletFirstOrDefault.ToString());

        // Using SingleOrDefault
        var walletSingleOrDefault = context.Wallets.SingleOrDefault(w => w.Id == 1);
        Console.WriteLine(walletSingleOrDefault is null ? "Wallet not found" : walletSingleOrDefault.ToString());

        // Using First
        var walletFirst = context.Wallets.First(w => w.Id == 1);
        Console.WriteLine(walletFirst);

        // Using Single
        var walletSingle = context.Wallets.Single(w => w.Id == 1);
        Console.WriteLine(walletSingle);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

### Best Method

- **For Primary Key Lookup**: Use `Find` if you are looking up an entity by its primary key, as it is the most efficient method due to possible use of the context's cache.
- **For Filtering with Criteria**:
  - Use `FirstOrDefault` if you expect zero or more entities and want the first match.
  - Use `SingleOrDefault` if you expect zero or one entity and want to ensure uniqueness.
  - Use `First` if you expect at least one entity and want the first match, with an exception thrown if no match is found.
  - Use `Single` if you expect exactly one entity and want to ensure this, with exceptions for no match or multiple matches.

In general, `Find` is often the best choice for primary key lookups due to its efficiency. For other criteria, choose the method that best matches your expected results and handles exceptions appropriately.