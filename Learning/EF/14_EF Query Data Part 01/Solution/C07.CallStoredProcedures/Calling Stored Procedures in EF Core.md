Here's a refined structure for your Markdown document, providing a clear, organized, and easy-to-follow layout:

---

# Working with Stored Procedures in Entity Framework Core

## Overview
Using stored procedures in Entity Framework (EF) Core can be beneficial in various scenarios such as handling complex queries, optimizing performance, and encapsulating business logic within the database. However, it's important to understand when and how to use them effectively in EF Core.

---

## Stored Procedure Example

### Structure of `sp_GetSectionDetails`
Below is the SQL script for the stored procedure `sp_GetSectionDetails`, which retrieves details for a specific section:

```sql
USE [EF015]
GO
ALTER PROCEDURE [dbo].[sp_GetSectionDetails]
    @SectionId INT
AS
BEGIN
    SELECT
        S.Id,
        C.CourseName,
        C.HoursToComplete AS TotalHours,
        S.SectionName,
        (I.FName + ' ' + I.LName) AS Instructor,
        FORMAT(S.[StartDate], 'yyyy-MM-dd') + ' - ' + FORMAT(S.[EndDate], 'yyyy-MM-dd') AS Period,
        COALESCE(CONVERT(VARCHAR, S.[StartTime], 108), 'N/A') 
            + ' - ' + 
            COALESCE(CONVERT(VARCHAR, S.[EndTime], 108), 'N/A') 
            + ' (' + 
            CAST(DATEDIFF(HOUR, S.[StartTime], S.[EndTime]) AS VARCHAR(2)) + ' hrs)' AS Timeslot,
        SC.SUN,
        SC.MON,
        SC.Tue,
        SC.WED,
        SC.THU,
        SC.FRI,
        SC.SAT,
        CAST((DATEDIFF(MINUTE, S.StartTime, S.EndTime) / 60.0) * 
            CASE 
                WHEN SC.ScheduleType = 'Daily' THEN 7
                WHEN SC.ScheduleType = 'DayAfterDay' THEN 3
                WHEN SC.ScheduleType = 'TwiceAWeek' THEN 2
                WHEN SC.ScheduleType = 'Weekend' THEN 2
                WHEN SC.ScheduleType = 'Compact' THEN 7
                ELSE 0
            END AS INT) AS HoursPerWeek
    FROM Sections S
    JOIN Courses C ON S.CourseId = C.Id
    JOIN Schedules SC ON S.ScheduleId = SC.Id
    JOIN Instructors I ON S.InstructorId = I.Id
    WHERE S.Id = @SectionId
END;
```

---

## Calling Stored Procedures in EF Core

To call a stored procedure in EF Core, you can use the `FromSqlRaw` or `FromSqlInterpolated` methods. This requires creating a model class to map the results of the stored procedure.

### 1. **Create a DTO Class to Hold the Result**
Create a class that will represent the structure of the result set returned by the stored procedure.

```csharp
public class SectionDetailsDto
{
    public int Id { get; set; }
    public string CourseName { get; set; }
    public int TotalHours { get; set; }
    public string SectionName { get; set; }
    public string Instructor { get; set; }
    public string Period { get; set; }
    public string Timeslot { get; set; }
    public bool SUN { get; set; }
    public bool MON { get; set; }
    public bool Tue { get; set; }
    public bool WED { get; set; }
    public bool THU { get; set; }
    public bool FRI { get; set; }
    public bool SAT { get; set; }
    public int HoursPerWeek { get; set; }
}
```

### 2. **Call the Stored Procedure Using `FromSqlRaw`**

Use `FromSqlRaw` to execute the stored procedure:

```csharp
using (var context = new AppDbContext())
{
    int sectionId = 1;
    var sectionDetails = context.Set<SectionDetailsDto>()
        .FromSqlRaw("EXEC sp_GetSectionDetails @SectionId", new SqlParameter("@SectionId", sectionId))
        .AsNoTracking()
        .ToList();

    foreach (var detail in sectionDetails)
    {
        Console.WriteLine($"Section: {detail.SectionName}, Instructor: {detail.Instructor}, Period: {detail.Period}");
    }
}
```

### 3. **Call the Stored Procedure Using `FromSqlInterpolated`**

For safer parameter handling, you can use `FromSqlInterpolated`:

```csharp
using (var context = new AppDbContext())
{
    int sectionId = 1;
    var sectionDetails = context.Set<SectionDetailsDto>()
        .FromSqlInterpolated($"EXEC sp_GetSectionDetails @SectionId = {sectionId}")
        .AsNoTracking()
        .ToList();

    foreach (var detail in sectionDetails)
    {
        Console.WriteLine($"Section: {detail.SectionName}, Instructor: {detail.Instructor}, Period: {detail.Period}");
    }
}
```

---

## Best Practices for Using Stored Procedures in EF Core

### When to Use Stored Procedures
- **Complex Business Logic**: Ideal for encapsulating complex logic that’s better executed on the database side.
- **Performance Optimizations**: Useful when specific operations are more efficient when run as stored procedures.
- **Legacy Systems**: When working with existing databases that already use stored procedures.

### Best Practices
1. **Use DTOs (Data Transfer Objects)**:
   - Map the result of your stored procedure to a DTO to avoid directly mapping to your entity classes, which might not align perfectly.

2. **Prefer `FromSqlInterpolated` for Safety**:
   - Use `FromSqlInterpolated` to prevent SQL injection by safely handling user input.

3. **Use `AsNoTracking` for Read-Only Data**:
   - Improve performance by avoiding change tracking for data that doesn’t need to be updated.

4. **Encapsulate Stored Procedure Calls**:
   - Encapsulate the logic in a repository or service class to keep your DbContext clean and maintainable.

5. **Avoid Overuse**:
   - Use stored procedures only when necessary, relying on EF Core's native LINQ capabilities for simpler queries.

### Example: Encapsulating Stored Procedure Call in a Repository

```csharp
public class SectionRepository
{
    private readonly AppDbContext _context;

    public SectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<SectionDetailsDto> GetSectionDetails(int sectionId)
    {
        return _context.Set<SectionDetailsDto>()
            .FromSqlInterpolated($"EXEC sp_GetSectionDetails @SectionId = {sectionId}")
            .AsNoTracking()
            .ToList();
    }
}
```

**Usage**:

```csharp
var sectionRepo = new SectionRepository(context);
var details = sectionRepo.GetSectionDetails(1);

foreach (var detail in details)
{
    Console.WriteLine($"Section: {detail.SectionName}, Instructor: {detail.Instructor}, Period: {detail.Period}");
}
```

---

## Conclusion

Stored procedures can be a powerful tool in EF Core for handling complex queries, optimizing performance, and working with legacy databases. However, they should be used judiciously, with a preference for EF Core’s LINQ capabilities for simpler tasks. By following best practices, you can effectively integrate stored procedures into your EF Core projects.

---

This structure organizes the content logically, making it easier to follow and understand the usage of stored procedures in EF Core.