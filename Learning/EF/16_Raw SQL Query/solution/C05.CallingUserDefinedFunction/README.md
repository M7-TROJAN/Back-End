### Calling User-Defined Functions (UDFs) in Entity Framework Core

User-Defined Functions (UDFs) in SQL Server are custom functions that allow you to encapsulate reusable logic, returning either a single value (scalar-valued) or a result set (table-valued). In Entity Framework Core (EF Core), you can map these functions to methods in your code, allowing you to call them as part of your LINQ queries. This is particularly useful for encapsulating complex logic or reusing database logic that is already implemented.

### Types of User-Defined Functions

1. **Scalar-Valued Functions**: Return a single value (e.g., `int`, `nvarchar`).
2. **Table-Valued Functions**: Return a table (similar to a view but can take parameters).

In this lesson, we'll focus on **Scalar-Valued Functions**.

### Scalar-Valued Function Example

#### SQL Server Function Definition

Here's the SQL code for a scalar-valued function that checks an instructor's availability:

```sql
ALTER FUNCTION [dbo].[fn_InstructorAvailability](
    @InstructorId INT,
    @StartDate DATE,
    @EndDate DATE,
    @StartTime TIME,
    @EndTime TIME
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Result NVARCHAR(50)

    IF EXISTS(
        SELECT 1
        FROM Sections
        WHERE InstructorId = @InstructorId
        AND (
            (StartDate BETWEEN @StartDate AND @EndDate) OR
            (EndDate BETWEEN @StartDate AND @EndDate) OR
            (StartDate <= @StartDate AND EndDate >= @EndDate)
        )
        AND (
            (StartTime BETWEEN @StartTime AND @EndTime) OR
            (EndTime BETWEEN @StartTime AND @EndTime) OR
            (StartTime <= @StartTime AND EndTime >= @EndTime)
        )
    )
        SET @Result = 'Unavailable'
    ELSE
        SET @Result = 'Available'

    RETURN @Result
END
```

This function checks if an instructor is available for a specific time range and returns either "Available" or "Unavailable".

### Mapping the Function in EF Core

To call this function from EF Core, you'll need to map it to a static method in your `DbContext`.

#### Setting Up the `DbContext`

```csharp
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    [DbFunction("fn_InstructorAvailability", Schema = "dbo")]
    public static string GetInstructorAvailability(int instructorId, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
    {
        // EF Core will replace this method call with the corresponding SQL function call.
        throw new NotImplementedException();
    }
}
```

- **[DbFunction] Attribute**: Maps the `GetInstructorAvailability` method to the SQL function `fn_InstructorAvailability`.
- **Schema**: Specifies the schema where the function is located (in this case, `dbo`).

### Using the Function in a Query

Now that the function is mapped, you can use it in LINQ queries:

```csharp
using (var context = new AppDbContext())
{
    var startDate = new DateTime(2023, 09, 24);
    var endDate = new DateTime(2023, 12, 26);
    var startTime = new TimeSpan(08, 00, 00);
    var endTime = new TimeSpan(11, 00, 00);

    var result = context.Instructors.Select(x =>
    new
    {
        Id = x.Id,
        FullName = x.FullName,
        DateRange = $"{startDate.ToShortDateString()}-{endDate.ToShortDateString()}",
        TimeRange = $"{startTime.ToString(@"hh\:mm")}-{endTime.ToString(@"hh\:mm")}",
        Status = AppDbContext.GetInstructorAvailability(x.Id, startDate, endDate, startTime, endTime)
    }).ToList();

    foreach (var item in result)
    {
        Console.WriteLine($"{item.Id}\t{item.FullName,-20}\t{item.DateRange}\t{item.TimeRange}\t{item.Status}");
    }
}
```

### Example 2: Checking Instructor Availability with a Different Scenario

#### SQL Function Definition

Let's assume you have a different function that checks if an instructor is available only on specific days:

```sql
ALTER FUNCTION [dbo].[fn_InstructorAvailableOnDays](
    @InstructorId INT,
    @DayOfWeek NVARCHAR(10)
)
RETURNS BIT
AS
BEGIN
    IF EXISTS(
        SELECT 1
        FROM Sections
        JOIN Schedules ON Sections.ScheduleId = Schedules.Id
        WHERE Sections.InstructorId = @InstructorId
        AND (
            (@DayOfWeek = 'SUN' AND Schedules.SUN = 1) OR
            (@DayOfWeek = 'MON' AND Schedules.MON = 1) OR
            (@DayOfWeek = 'TUE' AND Schedules.TUE = 1) OR
            (@DayOfWeek = 'WED' AND Schedules.WED = 1) OR
            (@DayOfWeek = 'THU' AND Schedules.THU = 1) OR
            (@DayOfWeek = 'FRI' AND Schedules.FRI = 1) OR
            (@DayOfWeek = 'SAT' AND Schedules.SAT = 1)
        )
    )
        RETURN 1
    ELSE
        RETURN 0
END
```

#### Mapping the Function

```csharp
public class AppDbContext : DbContext
{
    [DbFunction("fn_InstructorAvailableOnDays", Schema = "dbo")]
    public static bool IsInstructorAvailableOnDay(int instructorId, string dayOfWeek)
    {
        throw new NotImplementedException();
    }
}
```

#### Query Using the Function

```csharp
using (var context = new AppDbContext())
{
    var availableInstructors = context.Instructors
        .Where(i => AppDbContext.IsInstructorAvailableOnDay(i.Id, "MON"))
        .Select(i => new { i.Id, i.FullName })
        .ToList();

    foreach (var instructor in availableInstructors)
    {
        Console.WriteLine($"{instructor.Id}\t{instructor.FullName}");
    }
}
```

### Best Practices

1. **Encapsulation of Logic**: Use UDFs to encapsulate complex business logic or reusable calculations, keeping your application code clean.
   
2. **Performance Considerations**: Be mindful of performance when using UDFs, especially in large queries. Scalar functions can slow down queries because they are executed row by row.

3. **Mapping Only What's Necessary**: Only map UDFs in EF Core that are necessary for your application logic. Unused mappings can add unnecessary complexity.

4. **Error Handling**: UDFs don't handle errors in the same way as stored procedures. Ensure that the function logic is robust and accounts for edge cases.

### Summary

Calling user-defined functions in EF Core involves mapping the SQL function to a method in your `DbContext` class. You can then use this method within your LINQ queries, just as you would with any other method. This approach helps to encapsulate logic in the database, maintain a clean application codebase, and can potentially improve performance by reducing the complexity of the LINQ queries.