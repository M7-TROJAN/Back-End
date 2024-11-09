The **LINQ Dynamic Core** package is an extension for .NET that enables dynamic LINQ queries. It provides the ability to construct LINQ queries at runtime using strings to define query expressions. This can be particularly useful in scenarios where query parameters are not known at compile time, such as when building search or filtering functionality based on user input.

### Key Features of LINQ Dynamic Core

1. **Dynamic Query Expressions**:
   - Supports writing queries as strings that are parsed at runtime.
   - Expressions like `"Age > 30"` or `"Name.Contains(@0)"` can be used directly as strings in queries.

2. **String-Based Query Parameters**:
   - Allows parameterized queries using `@0`, `@1`, etc., for variable replacement, making it easier to inject parameters without hardcoding.

3. **Queryable Extensions**:
   - Supports LINQ methods (`Where`, `OrderBy`, `Select`, etc.) for `IQueryable` data sources, making it well-suited for querying databases via Entity Framework or similar ORMs.

4. **Compatibility**:
   - Works with LINQ providers such as Entity Framework Core, allowing dynamic filtering, sorting, and selection in database queries.

5. **Strongly Typed Support**:
   - Supports both dynamic (string-based) queries and strongly typed queries using `Expression<Func<T, bool>>` or other delegate types for more flexibility.

### Installing LINQ Dynamic Core

You can install it via NuGet Package Manager with the following command:

```bash
dotnet add package System.Linq.Dynamic.Core
```

### Basic Examples of LINQ Dynamic Core

1. **Filtering Data with `Where`**:
   ```csharp
   var users = dbContext.Users.Where("Age > @0", 30);
   ```

   #### Explanation:
   - This query selects users where `Age` is greater than 30.
   - `@0` is replaced with `30` at runtime.

2. **Sorting Data with `OrderBy`**:
   ```csharp
   var sortedUsers = dbContext.Users.OrderBy("Name desc");
   ```

   #### Explanation:
   - This query sorts users by the `Name` property in descending order.

3. **Selecting Specific Properties with `Select`**:
   ```csharp
   var names = dbContext.Users.Select("new (Name, Age)");
   ```

   #### Explanation:
   - This query projects only `Name` and `Age` properties into an anonymous type.

4. **Combining `Where` and `OrderBy` with Parameters**:
   ```csharp
   var result = dbContext.Users
                .Where("Name.Contains(@0) AND Age >= @1", "John", 18)
                .OrderBy("Age");
   ```

   #### Explanation:
   - This query filters users where `Name` contains "John" and `Age` is greater than or equal to 18, and then orders them by `Age`.

### Use Cases for LINQ Dynamic Core

- **Dynamic Filtering**: Useful for building filters based on user input or dynamic criteria.
- **Dynamic Sorting**: Enables sorting by different fields without hardcoding them, ideal for customizable views.
- **Data Shaping**: Allows selecting specific fields dynamically, which can reduce data transfer and improve performance.
  
### Considerations

While `LINQ Dynamic Core` is powerful, remember that:

- Queries are string-based, which means no compile-time checking.
- Mistakes in query strings won’t be caught until runtime.
- Use cautiously when working with sensitive data, as dynamic queries can introduce SQL injection risks if not handled properly.

### Further Reading

For more detailed documentation, visit the [official LINQ Dynamic Core GitHub repository](https://github.com/StefH/System.Linq.Dynamic.Core).
