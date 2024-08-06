# Eager Loading and Lazy Loading in Entity Framework

Entity Framework (EF) provides multiple ways to load related data, with Eager Loading and Lazy Loading being two primary methods. Understanding these concepts is crucial for optimizing data access and performance in your applications.

## Eager Loading

Eager Loading is a technique where related data is loaded from the database as part of the initial query. This is done using the `Include` method, which ensures that the related entities are loaded in a single database call.

### Advantages of Eager Loading

- **Reduced Database Roundtrips**: By loading related data in one go, it minimizes the number of database calls, which can be beneficial in scenarios with many related entities.
- **Performance**: Useful in scenarios where related data is frequently accessed, as it avoids multiple lazy loading triggers.
- **Consistency**: Ensures all related data is loaded upfront, providing a complete dataset immediately.

### Disadvantages of Eager Loading

- **Increased Initial Load**: Can result in larger datasets being loaded initially, which may impact performance if not managed correctly.
- **Complex Queries**: The generated SQL queries can become complex and may affect performance for deeply nested relationships.

### Example of Eager Loading

Assume you have the following models:

```csharp
public class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}

public class Section
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }
}
```

To eagerly load the `Sections` for each `Instructor`:

```csharp
using (var context = new AppDbContext())
{
    var instructors = context.Instructors
                             .Include(i => i.Sections)
                             .ToList();

    foreach (var instructor in instructors)
    {
        Console.WriteLine($"{instructor.Name}'s Sections:");
        foreach (var section in instructor.Sections)
        {
            Console.WriteLine($"\t{section.SectionName}");
        }
    }
}
```

## Lazy Loading

Lazy Loading is a technique where related data is loaded on-demand when the navigation property is accessed. This approach defers the loading of related entities until they are explicitly accessed, which can help in reducing the initial load.

### Advantages of Lazy Loading

- **Reduced Initial Load**: Loads only the primary entities initially, deferring the loading of related data until it's actually needed.
- **Simplicity**: Easy to implement without altering the initial query.

### Disadvantages of Lazy Loading

- **Increased Database Roundtrips**: Each access to a lazy-loaded property results in a separate database query, which can be inefficient.
- **Potential Performance Issues**: Can lead to the "N+1 query problem," where accessing a collection of related entities triggers multiple queries.

### Example of Lazy Loading

Assuming lazy loading is enabled, you can access related data without explicitly including it in the query:

```csharp
using (var context = new AppDbContext())
{
    var instructors = context.Instructors.ToList();

    foreach (var instructor in instructors)
    {
        Console.WriteLine($"{instructor.Name}'s Sections:");
        foreach (var section in instructor.Sections)
        {
            Console.WriteLine($"\t{section.SectionName}");
        }
    }
}
```

### Enabling Lazy Loading

To enable lazy loading in EF Core, you need to install the `Microsoft.EntityFrameworkCore.Proxies` package and configure your `DbContext` to use lazy loading proxies.

1. Install the package:

```sh
dotnet add package Microsoft.EntityFrameworkCore.Proxies
```

2. Configure the `DbContext`:

```csharp
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies()
                      .UseSqlServer("YourConnectionString");
    }

    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Section> Sections { get; set; }
}
```

3. Mark navigation properties as virtual:

```csharp
public class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}

public class Section
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public int InstructorId { get; set; }
    public virtual Instructor Instructor { get; set; }
}
```

## Comparison

| Aspect              | Eager Loading                                 | Lazy Loading                                 |
|---------------------|-----------------------------------------------|----------------------------------------------|
| Initial Load Time   | Longer (loads related data upfront)           | Shorter (loads only primary entities)        |
| Database Roundtrips | Fewer (single query for all related data)     | More (separate query for each access)        |
| Query Complexity    | More complex SQL queries                      | Simpler initial query                        |
| Performance         | Better for frequently accessed related data   | Can suffer from N+1 query problem            |
| Implementation      | Requires explicit `Include` statements        | Requires minimal changes, but needs proxy setup |

## Best Practices

- **Eager Loading**: Use when you know you will need the related data and want to minimize database roundtrips.
- **Lazy Loading**: Use when related data might not be needed immediately or at all, but be cautious of the N+1 query problem.

Understanding the differences between these loading strategies and when to use each can significantly impact the performance and efficiency of your application.
