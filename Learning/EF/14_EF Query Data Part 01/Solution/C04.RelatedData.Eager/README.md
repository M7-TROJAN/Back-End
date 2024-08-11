### Understanding Eager Loading in Entity Framework Core

**Eager loading** is a technique in Entity Framework Core used to load related data along with the main entity data in a single query. This is achieved using the `Include` and `ThenInclude` methods.

#### Purpose of Eager Loading
The main purpose of eager loading is to reduce the number of database queries required to fetch related data. Instead of performing multiple queries to load an entity and its related data, eager loading retrieves everything in a single query. This improves performance and ensures that all related data is loaded upfront.

### `Include` Method

The `Include` method is used to specify which related entities should be loaded along with the main entity. It works for loading related data of one-to-one, one-to-many, and many-to-many relationships.

#### Example 1: Basic `Include`

Let's consider loading a `Section` along with its related `Participants`:

```csharp
using (var context = new AppDbContext())
{
    var sectionId = 1;

    var sectionQuery = context.Sections
        .Include(x => x.Participants)
        .Where(x => x.Id == sectionId);

    Console.WriteLine(sectionQuery.ToQueryString());

    var section = sectionQuery.FirstOrDefault();
    Console.WriteLine($"Section: {section.SectionName}");
    Console.WriteLine($"--------------------");
    foreach (var participant in section.Participants)
        Console.WriteLine($"[{participant.Id}] {participant.FName} {participant.LName}");
}
```

- **Query Explanation:**
  - The `Include(x => x.Participants)` method specifies that along with the `Section`, its related `Participants` should also be loaded.
  - The resulting SQL query joins the `Sections` table with the `Participants` table, retrieving data in a single query.

### `ThenInclude` Method

The `ThenInclude` method is used in conjunction with `Include` to load related entities from a child or grandchild relationship.

#### Example 2: Using `ThenInclude`

Let's extend our previous example to load the `Instructor` along with the `Office`:

```csharp
using (var context = new AppDbContext())
{
    var sectionId = 1;

    var sectionQuery = context.Sections
        .Include(x => x.Instructor)
        .ThenInclude(x => x.Office)
        .Where(x => x.Id == sectionId);

    Console.WriteLine(sectionQuery.ToQueryString());

    var section = sectionQuery.FirstOrDefault();
    Console.WriteLine($"Section: {section.SectionName} " +
        $"[{section.Instructor.FName} " +
        $"{section.Instructor.LName} " +
        $"({section.Instructor.Office.OfficeName})]");
}
```

- **Query Explanation:**
  - `Include(x => x.Instructor)` specifies that the `Instructor` related to the `Section` should be loaded.
  - `ThenInclude(x => x.Office)` further specifies that the `Office` related to the `Instructor` should also be loaded.
  - The resulting SQL query joins the `Sections`, `Instructors`, and `Offices` tables to fetch all the necessary data in a single query.

### Key Points to Remember

- **Eager Loading vs. Lazy Loading:**
  - **Eager Loading:** Loads related data upfront in a single query. It is useful when you know you will need the related data immediately.
  - **Lazy Loading:** Loads related data only when it is accessed, which can result in multiple queries to the database.

- **Performance Considerations:**
  - Eager loading can improve performance by reducing the number of database queries, but it may also retrieve more data than necessary if not used carefully.
  - The choice between eager loading and lazy loading depends on the specific use case and the expected data access patterns.

### Conclusion

Eager loading, through the `Include` and `ThenInclude` methods, provides a powerful way to optimize data retrieval by minimizing the number of queries and ensuring that related data is loaded together. Understanding when and how to use these methods is essential for building efficient and maintainable applications with Entity Framework Core.