# Calling Stored Procedures in Entity Framework Core

## Overview

Entity Framework Core (EF Core) allows you to execute raw SQL queries, including calling stored procedures, using several methods. This guide will walk you through the process of calling a stored procedure in EF Core, with a focus on:

- Creating a Data Transfer Object (DTO) to represent the stored procedure's result set.
- Configuring your `DbContext` to handle the result set.
- Calling the stored procedure using the `FromSql` method.

## Stored Procedure Example

### Stored Procedure: `sp_GetSectionWithninDateRange`

This stored procedure retrieves detailed information about sections within a specified date range.

```sql
ALTER PROCEDURE [dbo].[sp_GetSectionWithninDateRange]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
        S.Id,
        C.CourseName,
        C.HoursToComplete TotalHours,
        S.SectionName,
        (I.FName + ' ' + I.LName) As Instructor,
        FORMAT(S.[StartDate], 'yyyy-MM-dd') + ' - ' + FORMAT(S.[EndDate], 'yyyy-MM-dd') As Period,
        COALESCE(CONVERT(VARCHAR, S.[StartTime], 108), 'N/A') 
            + ' - ' + 
            COALESCE(CONVERT(VARCHAR, S.[EndTime], 108), 'N/A') 
            + ' (' + 
            CAST(DATEDIFF(HOUR, S.[StartTime], S.[EndTime]) AS VARCHAR(2)) + ' hrs)' As Timeslot,
        SC.SUN,
        SC.MON,
        SC.TUE,
        SC.WED,
        SC.THU,
        SC.FRI,
        SC.SAT,
        CAST((DATEDIFF(MINUTE, S.StartTime, S.EndTime)/60.0) * 
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
    WHERE (S.StartDate BETWEEN @StartDate AND @EndDate) 
        AND (S.EndDate BETWEEN @StartDate AND @EndDate)
END;
```

## Step 1: Creating the Data Transfer Object (DTO)

We need a class that will represent the structure of the data returned by the stored procedure.

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
    public bool TUE { get; set; }
    public bool WED { get; set; }
    public bool THU { get; set; }
    public bool FRI { get; set; }
    public bool SAT { get; set; }
    public int HoursPerWeek { get; set; }

    public override string ToString()
    {
        return $"{Id,5} {CourseName,-34} {Instructor,-22} {Timeslot,-24} " +
               $"{string.Join(" ",
                   SUN ? "SUN" : "   ",
                   MON ? "MON" : "   ",
                   TUE ? "TUE" : "   ",
                   WED ? "WED" : "   ",
                   THU ? "THU" : "   ",
                   FRI ? "FRI" : "   ",
                   SAT ? "SAT" : "   "),-37}  ({HoursPerWeek}) hrs/week";
    }
}
```

This DTO will hold the results from the stored procedure.

## Step 2: Configuring the DbContext

In the `DbContext`, you need to add a `DbSet` representing the stored procedure's result set and configure it.

### Example:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<SectionDetailsDto> SectionWithDetails { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        builder.Properties<DateOnly>()
            .HaveConversion<Config.DateOnlyConverter>()
            .HaveColumnType("date");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("YourConnectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the DTO class to have no key (since it is a result set)
        modelBuilder.Entity<SectionDetailsDto>().HasNoKey();
    }
}
```

Here, `SectionWithDetails` is the `DbSet` that will store the results from the stored procedure.

## Step 3: Calling the Stored Procedure

You can call the stored procedure using the `FromSql` method with parameters.

### Example Using `FromSql`:

```csharp
public static void Main(string[] args)
{
    using (var context = new AppDbContext())
    {
        var startDateParam = new SqlParameter("@StartDate", System.Data.SqlDbType.Date)
        {
            Value = new DateTime(2023, 01, 01)
        };
        var endDateParam = new SqlParameter("@EndDate", System.Data.SqlDbType.Date)
        {
            Value = new DateTime(2023, 06, 30)
        };

        var sections = context.SectionWithDetails
            .FromSql($"Exec dbo.sp_GetSectionWithninDateRange {startDateParam}, {endDateParam}")
            .ToList();

        foreach (var section in sections)
        {
            Console.WriteLine(section);
        }
    }
}
```

### Explanation:

- **Parameters:** `startDateParam` and `endDateParam` are SQL parameters passed to the stored procedure.
- **`FromSql` Method:** Executes the stored procedure and maps the result to the `SectionDetailsDto` class.

### Example Output Simulation:

```
1     Programming 101                   John Doe               09:00 - 11:00 (2 hrs)  SUN MON TUE     FRI     (10) hrs/week
2     Advanced Databases                Jane Smith             14:00 - 16:00 (2 hrs)  SUN     TUE     THU     (6) hrs/week
...
```

## Summary

- **`FromSql`:** The main method used to execute a stored procedure and map the results to a DTO in EF Core.
- **DTO Class:** Represents the structure of the data returned by the stored procedure.
- **DbContext Configuration:** Includes a `DbSet` for the stored procedure result and a configuration to treat it as having no key.
- **Executing the Procedure:** You can pass parameters to the stored procedure and execute it using `FromSql`, mapping the results to the DTO.