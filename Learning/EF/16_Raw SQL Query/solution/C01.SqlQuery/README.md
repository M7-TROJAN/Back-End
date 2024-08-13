### Raw SQL Queries in EF Core

In some scenarios, it can be challenging to express certain queries using LINQ, either because the query is too complex or because LINQ might lead to performance issues. For these cases, Entity Framework Core (EF Core) provides the ability to execute raw SQL queries directly against the database.

Raw SQL queries allow you to leverage SQL's full power and flexibility directly within your EF Core applications. This can be particularly useful when you need to execute complex SQL queries or optimize performance beyond what LINQ can offer.

### Methods for Executing Raw SQL Queries

EF Core provides several methods to execute raw SQL queries:

#### 1. `FromSqlRaw` (EF Core 3.0+)
- **Purpose**: Executes a raw SQL query that returns an entity or entities from the database.
- **Usage**: Use `FromSqlRaw` when you have a pre-defined SQL query string and want to execute it without any parameter interpolation.
- **Example**:
  ```csharp
  var sections = context.Sections
      .FromSqlRaw("SELECT * FROM Sections WHERE CourseId = {0}", courseId)
      .ToList();
  ```

#### 2. `FromSqlInterpolated` (EF Core 3.0+)
- **Purpose**: Similar to `FromSqlRaw`, but allows you to safely interpolate variables into the SQL string.
- **Usage**: Use `FromSqlInterpolated` when you need to include variables in your SQL query and want to avoid SQL injection vulnerabilities.
- **Example**:
  ```csharp
  var sections = context.Sections
      .FromSqlInterpolated($"SELECT * FROM Sections WHERE CourseId = {courseId}")
      .ToList();
  ```

#### 3. `FromSql` (EF Core 7.0+)
- **Purpose**: A more generalized method that allows for raw SQL queries in EF Core 7.0+ and replaces both `FromSqlRaw` and `FromSqlInterpolated`.
- **Usage**: Use `FromSql` with either raw or interpolated SQL depending on your scenario. It simplifies the API by providing one method to handle both cases.
- **Example**:
  ```csharp
  var sections = context.Sections
      .FromSql($"SELECT * FROM Sections WHERE CourseId = {courseId}")
      .ToList();
  ```

### Why Use Raw SQL Queries?

- **Complex Queries**: Sometimes, LINQ can't express the full complexity of your query, especially when dealing with advanced SQL features.
- **Performance**: Raw SQL can be more performant in certain cases, allowing you to write optimized queries that EF Core might not generate by default.
- **Leverage Database Features**: Raw SQL lets you use specific database features or syntax that might not be supported directly in LINQ.

### Examples With Explanation

Sometimes, it can be challenging to create a LINQ expression that accurately represents the SQL query you want to execute. In other cases, using LINQ can lead to performance issues. For these situations, EF Core provides several methods to execute raw SQL queries, allowing you to implement complex queries directly and optimize performance as needed.

### Simulation of Output (Example)

Let's consider an example:

```csharp
var courseId = 1;
var sections = context.Sections
    .FromSqlRaw("SELECT * FROM Sections WHERE CourseId = {0}", courseId)
    .ToList();
```

**SQL Query Generated**:
```sql
SELECT * FROM Sections WHERE CourseId = 1
```

**Expected Output**:
- If there are three sections (`Section1`, `Section2`, `Section3`) under the course with `CourseId = 1`, you will get a list of these sections.

- **Example Output**:
  ```
  Section1
  Section2
  Section3
  ```

This example shows how you can directly query the database using SQL while still working within the EF Core framework.


### Why `FromSql` is the Best Method (EF Core 7.0+)

With the introduction of EF Core 7.0, the `FromSql` method has become the preferred way to execute raw SQL queries. Here's why `FromSql` is considered the best option:

1. **Unified API**: Before EF Core 7.0, you had to choose between `FromSqlRaw` and `FromSqlInterpolated` based on whether you were using a raw SQL string or an interpolated SQL string. The `FromSql` method unifies these two approaches into a single, more intuitive method. This reduces complexity and makes your code easier to read and maintain.

2. **Flexibility**: `FromSql` can handle both raw SQL and interpolated SQL, giving you the flexibility to choose the best approach for your specific scenario without having to switch between different methods.

3. **Security**: When using interpolated SQL, `FromSql` helps prevent SQL injection vulnerabilities by safely handling parameters. It gives you the power of raw SQL while maintaining security best practices.

4. **Consistency**: By standardizing on a single method (`FromSql`), your codebase becomes more consistent. This is particularly beneficial in large projects where multiple developers might be working with raw SQL queries.

### Example of a Complex Query Using `FromSql`

Let's consider a more complex scenario where you want to retrieve sections along with their associated instructors and courses, filtered by specific criteria.

#### Scenario:
You want to retrieve all sections that are taught by instructors who have more than three sections and that are part of a specific course. You also want to order the results by the instructor's last name.

#### Complex SQL Query Example:

```sql
SELECT s.*, c.*, i.*
FROM Sections s
JOIN Courses c ON s.CourseId = c.Id
JOIN Instructors i ON s.InstructorId = i.Id
WHERE i.Id IN (
    SELECT InstructorId
    FROM Sections
    GROUP BY InstructorId
    HAVING COUNT(*) > 3
)
AND s.CourseId = @CourseId
ORDER BY i.LName;
```

#### Using `FromSql` in EF Core:

```csharp
var courseId = 5;

var sections = context.Sections
    .FromSql($@"
        SELECT s.*, c.*, i.*
        FROM Sections s
        JOIN Courses c ON s.CourseId = c.Id
        JOIN Instructors i ON s.InstructorId = i.Id
        WHERE i.Id IN (
            SELECT InstructorId
            FROM Sections
            GROUP BY InstructorId
            HAVING COUNT(*) > 3
        )
        AND s.CourseId = {courseId}
        ORDER BY i.LName
    ")
    .Include(s => s.Course)  // Ensure related data is properly loaded into the entities
    .Include(s => s.Instructor)
    .ToList();
```

### Explanation of the Query

- **JOIN Operations**: The query joins the `Sections`, `Courses`, and `Instructors` tables to retrieve the necessary data.
  
- **Subquery with Group By**: The subquery in the `WHERE` clause uses `GROUP BY` and `HAVING` to filter instructors who have more than three sections.

- **Parameter Filtering**: The `courseId` variable is used to filter sections by a specific course.

- **Ordering**: The results are ordered by the instructor's last name (`LName`).

### Why `FromSql` is Ideal for This Query

- **Complexity**: The query is too complex to be easily expressed using LINQ, especially with the subquery and multiple joins. `FromSql` allows you to directly use SQL, making it easier to write and understand complex queries.

- **Performance**: Since this is a complex query, writing it directly in SQL can help you optimize performance. You can ensure the query is executed as efficiently as possible without worrying about LINQ translation.

- **Flexibility**: `FromSql` gives you the flexibility to write SQL that exactly matches your requirements, using all the features and syntax of SQL without being constrained by the limitations of LINQ.

### Conclusion

The `FromSql` method in EF Core 7.0+ is the best choice for executing raw SQL queries because it provides a unified, flexible, and secure API for working with complex queries. It allows you to write SQL queries directly, ensuring both performance and clarity in your code.
