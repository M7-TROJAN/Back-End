### Calling Table-Valued Functions in Entity Framework Core

Table-Valued Functions (TVFs) in SQL Server are functions that return a table data type, allowing you to perform a `SELECT` operation on them. They are similar to views, but they can accept parameters and thus provide more flexibility in querying.

#### Purpose of Table-Valued Functions

The primary purpose of TVFs is to encapsulate complex querying logic that returns a result set. By creating a TVF, you can:

- Reuse the query logic without repeating the code.
- Improve the maintainability of your database queries.
- Parameterize queries that would otherwise require complex SQL.

TVFs can be particularly useful when you need to filter data based on input parameters or when you want to perform calculations or manipulations on the data before returning it.

#### Example Table-Valued Function: `GetSectionsExceedingParticipantCount`

The provided SQL function `GetSectionsExceedingParticipantCount` returns sections where the number of participants meets or exceeds a specified threshold:

```sql
ALTER FUNCTION [dbo].[GetSectionsExceedingParticipantCount](@numberOfParticipants int)
RETURNS TABLE
AS
RETURN
(
    SELECT s.*
    FROM Sections AS s
    WHERE 
    (
        SELECT COUNT(*) 
        FROM Enrollments AS e
        WHERE s.Id = e.SectionId
    ) >= @numberOfParticipants
)
```

### Using Table-Valued Functions in EF Core

To use a TVF in EF Core, you typically need to map it in your `DbContext`. Let's go through the steps to do this.

#### Step 1: Map the TVF in `DbContext`

You need to register the TVF in your `OnModelCreating` method within the `DbContext` class:

```csharp
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Registering the table-valued function
        modelBuilder.HasDbFunction(
            typeof(AppDbContext)
            .GetMethod(nameof(GetSectionsExceedingParticipantCount), new[] { typeof(int) }));
    }

    public IQueryable<Section> GetSectionsExceedingParticipantCount(int participantThreshold)
    {
        return FromExpression(() => GetSectionsExceedingParticipantCount(participantThreshold));
    }
}
```

In this example:

- `HasDbFunction` is used to map the TVF in EF Core.
- `FromExpression` is a method that helps EF Core understand that this method is linked to a database function and should not be executed in-memory.

#### Step 2: Calling the TVF in Your Application Code

Once the TVF is mapped, you can use it just like a LINQ query:

```csharp
using (var context = new AppDbContext())
{
    var sections = context.GetSectionsExceedingParticipantCount(21).ToList();

    foreach (var section in sections)
    {
        Console.WriteLine($"{section.Id}\t{section.SectionName}\t{section.StartDate.ToShortDateString()}\t{section.EndDate.ToShortDateString()}");
    }
}
```

This code fetches sections where the number of participants is greater than or equal to 21 and prints their details.

### Another Example in a Different Scenario

Let's consider a scenario where you want to get all sections scheduled on a particular day of the week. The TVF could look like this:

```sql
ALTER FUNCTION [dbo].[GetSectionsByDayOfWeek](@dayOfWeek NVARCHAR(10))
RETURNS TABLE
AS
RETURN
(
    SELECT s.*
    FROM Sections AS s
    JOIN Schedules sch ON s.ScheduleId = sch.Id
    WHERE 
    (
        (@dayOfWeek = 'Sun' AND sch.SUN = 1) OR
        (@dayOfWeek = 'Mon' AND sch.MON = 1) OR
        (@dayOfWeek = 'Tue' AND sch.TUE = 1) OR
        (@dayOfWeek = 'Wed' AND sch.WED = 1) OR
        (@dayOfWeek = 'Thu' AND sch.THU = 1) OR
        (@dayOfWeek = 'Fri' AND sch.FRI = 1) OR
        (@dayOfWeek = 'Sat' AND sch.SAT = 1)
    )
)
```

#### Mapping and Calling the New TVF in EF Core

1. **Map the TVF in `DbContext`:**

```csharp
public IQueryable<Section> GetSectionsByDayOfWeek(string dayOfWeek)
{
    return FromExpression(() => GetSectionsByDayOfWeek(dayOfWeek));
}
```

2. **Call the TVF in the application:**

```csharp
using (var context = new AppDbContext())
{
    var sections = context.GetSectionsByDayOfWeek("Mon").ToList();

    foreach (var section in sections)
    {
        Console.WriteLine($"{section.Id}\t{section.SectionName}\t{section.StartDate.ToShortDateString()}\t{section.EndDate.ToShortDateString()}");
    }
}
```

This code fetches all sections scheduled on Mondays and prints their details.

### Best Practices

- **Validation:** Ensure that the SQL logic inside the TVF is well-validated to avoid runtime errors or security issues.
- **Performance:** Use TVFs judiciously as they might introduce performance overhead, especially when dealing with large datasets.
- **Mapping:** Always map TVFs correctly in EF Core to ensure they are executed on the server-side.
- **Maintenance:** Keep the logic inside TVFs simple and maintainable. Complex logic should be broken down into smaller functions if possible.

### Conclusion

Table-Valued Functions in EF Core allow you to encapsulate complex querying logic in a reusable, maintainable way. By mapping these functions in your `DbContext`, you can leverage the full power of LINQ while still taking advantage of the performance benefits of SQL Server.

Would you like me to prepare the structured markdown file summarizing this topic for you?