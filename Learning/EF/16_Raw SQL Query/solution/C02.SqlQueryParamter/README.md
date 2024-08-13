# SQL Query Parameters in EF Core

## Overview

When executing raw SQL queries in Entity Framework Core (EF Core), it's crucial to manage SQL parameters correctly to prevent SQL injection attacks. EF Core provides several methods to execute raw SQL queries while protecting against these vulnerabilities. The three main methods are:

- `FromSqlRaw`
- `FromSqlInterpolated`
- `FromSql`

### Why Use SQL Parameters?

SQL parameters allow you to safely insert user input into SQL queries. Without them, your application may be vulnerable to SQL injection, where malicious users can manipulate the SQL query to execute unintended commands on the database.

### Methods Overview

1. **`FromSqlRaw` (EF Core 3.0 and later):**
   - Executes a raw SQL query directly.
   - You need to explicitly pass SQL parameters to prevent SQL injection.
   - Best used when the SQL query is fully predefined, and you have parameters to pass.

2. **`FromSqlInterpolated` (EF Core 3.0 and later):**
   - Uses string interpolation to create the SQL query.
   - Automatically adds SQL parameters to the query for you, protecting against SQL injection.
   - Best used when you need to dynamically construct the query with parameters.

3. **`FromSql` (EF Core 7.0 and later):**
   - Combines the functionality of both `FromSqlRaw` and `FromSqlInterpolated`.
   - It allows for a more concise and secure way to execute raw SQL queries with parameters.

## Detailed Explanation and Examples

### `FromSqlInterpolated`

This method is a safe way to execute raw SQL queries using string interpolation. The SQL parameters are implicitly added to the query, which EF Core translates into a secure query with parameters.

**Example:**

```csharp
var c1 = context.Courses
    .FromSqlInterpolated($"SELECT * FROM dbo.Courses WHERE Id = {1}")
    .FirstOrDefault();
Console.WriteLine($"{c1.CourseName} ({c1.HoursToComplete})");
```

### SQL Query:

```sql
SELECT TOP(1) [c].[Id], [c].[CourseName], [c].[HoursToComplete], [c].[Price]
FROM (
    SELECT * FROM dbo.Courses WHERE Id = @p0
) AS [c]
```

Here, `@p0` is a parameter that EF Core adds to the query to safely insert the value `1`.

### `FromSqlRaw`

This method executes a raw SQL query without automatically adding SQL parameters. If you use string interpolation without manually adding parameters, the query will not be protected against SQL injection.

**Example Without Parameters:**

```csharp
var c3 = context.Courses
    .FromSqlRaw($"SELECT * FROM dbo.Courses WHERE Id = {1}")
    .FirstOrDefault();
Console.WriteLine($"{c3.CourseName} ({c3.HoursToComplete})");
```

### SQL Query:

```sql
SELECT TOP(1) [c].[Id], [c].[CourseName], [c].[HoursToComplete], [c].[Price]
FROM (
    SELECT * FROM dbo.Courses WHERE Id = 1
) AS [c]
```

Since no SQL parameter is used here, the query is vulnerable to SQL injection if user input is involved.

**Example With Parameters:**

To avoid this, you should explicitly add SQL parameters:

```csharp
var courseIdParam = new SqlParameter("@courseId", 1);
var c3 = context.Courses
    .FromSqlRaw("SELECT * FROM dbo.Courses WHERE Id = @courseId", courseIdParam)
    .FirstOrDefault();
Console.WriteLine($"{c3.CourseName} ({c3.HoursToComplete})");
```

### SQL Query:

```sql
SELECT TOP(1) [c].[Id], [c].[CourseName], [c].[HoursToComplete], [c].[Price]
FROM (
    SELECT * FROM dbo.Courses WHERE Id = @courseId
) AS [c]
```

In this case, `@courseId` is a SQL parameter that protects against SQL injection.

### `FromSql`

Starting with EF Core 7.0, `FromSql` serves as a more generalized method that can be used similarly to `FromSqlRaw` and `FromSqlInterpolated`.

## Which Method Is Best?

Among the three methods, `FromSqlInterpolated` is often the most convenient and safest choice because it automatically adds SQL parameters when using string interpolation, thus minimizing the risk of SQL injection. However, for more complex scenarios where you want full control over the SQL parameters or are dealing with a completely static query, `FromSqlRaw` might be more appropriate.

**Complex Query Example Using `FromSql`:**

```csharp
var instructorId = 1;
var courseName = "Introduction to Programming";

var courses = context.Courses
    .FromSqlInterpolated($@"
        SELECT c.*
        FROM dbo.Courses c
        INNER JOIN dbo.Sections s ON c.Id = s.CourseId
        WHERE s.InstructorId = {instructorId} 
        AND c.CourseName = {courseName}")
    .ToList();

foreach (var course in courses)
{
    Console.WriteLine($"{course.CourseName} ({course.HoursToComplete})");
}
```

### SQL Query:

```sql
SELECT c.*
FROM dbo.Courses c
INNER JOIN dbo.Sections s ON c.Id = s.CourseId
WHERE s.InstructorId = @p0
AND c.CourseName = @p1
```

In this example, `@p0` and `@p1` are the parameters that EF Core generates for `instructorId` and `courseName`, respectively.

## Conclusion

In summary:
- **Use `FromSqlInterpolated`** for dynamic queries where you want automatic SQL parameter protection.
- **Use `FromSqlRaw`** when you need full control over the query and parameters.
- **Use `FromSql`** (EF Core 7.0+) as a versatile alternative for either scenario.

Understanding the correct usage of these methods will help you write more secure and efficient queries in your EF Core applications.