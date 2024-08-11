# Querying Data in EF Core

## Overview

In this lesson, we will explore different approaches to querying data in Entity Framework Core (EF Core). We'll focus on querying methods such as `Single`, `First`, `FirstOrDefault`, `SingleOrDefault`, and `Where`, along with the `ToQueryString` method. These methods allow you to retrieve data from the database in various ways, depending on your requirements.

## Sample Data Model: `Course`

We'll use the following `Course` class in our examples:

```csharp
public class Course : Entity
{
    public string? CourseName { get; set; }
    public decimal Price { get; set; }
    public int HoursToComplete { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>(); // Navigation property
}
```

This model represents a course with properties for `CourseName`, `Price`, `HoursToComplete`, and a collection of related `Sections`.

## Querying Data

### 1. Retrieving All Courses and Using `ToQueryString`

The `ToQueryString` method allows you to see the SQL query that EF Core generates when executing a LINQ query. This can be helpful for debugging or understanding how EF Core translates LINQ expressions into SQL.

Example:

```csharp
var courses = context.Courses;

Console.WriteLine(courses.ToQueryString());

foreach (var course in courses)
    Console.WriteLine($""course name: {course.CourseName}, {course.HoursToComplete} hrs., {course.Price.ToString("C")}"");
```

### 2. Retrieving a Single Course by Primary Key

The `Single` method is used to retrieve a single entity that matches a given condition. If no entity or more than one entity matches the condition, an exception is thrown.

Example:

```csharp
var course = context.Courses.Single(x => x.Id == 1);

Console.WriteLine($""course name: {course.CourseName}, {course.HoursToComplete} hrs., {course.Price.ToString("C")}"");
```

### 3. Retrieving a Single Course by a Condition

You can also use `Single` to retrieve an entity based on a condition other than the primary key:

```csharp
var course = context.Courses.Single(x => x.HoursToComplete == 25);

Console.WriteLine($""{course.CourseName}, {course.Price.ToString("C")}"");
```

### 4. Using `First` to Retrieve the First Matching Course

The `First` method returns the first entity that matches the given condition. If no entity matches, an exception is thrown.

Example:

```csharp
var course = context.Courses.First(x => x.HoursToComplete == 25);

Console.WriteLine($""{course.CourseName}, {course.Price.ToString("C")}"");
```

### 5. Handling No Matches with `FirstOrDefault`

If you want to avoid exceptions when no match is found, you can use `FirstOrDefault`, which returns `null` if no match is found:

```csharp
var course = context.Courses.FirstOrDefault(x => x.HoursToComplete == 999);

Console.WriteLine($""{course?.CourseName}, {course?.Price.ToString("C")}"");
```

### 6. Using `SingleOrDefault` to Handle No Matches

Similar to `FirstOrDefault`, `SingleOrDefault` returns `null` if no entity matches the condition, but it expects a single match.

Example:

```csharp
var course = context.Courses.SingleOrDefault(x => x.HoursToComplete == 999);

Console.WriteLine($""{course?.CourseName}, {course?.Price.ToString("C")}"");
```

### 7. Filtering Courses with `Where`

The `Where` method allows you to filter entities based on a condition. The result is an `IQueryable` that you can further query or enumerate.

Example:

```csharp
var courses = context.Courses.Where(x => x.Price > 3000);

Console.WriteLine(courses.ToQueryString());

foreach (var course in courses)
    Console.WriteLine($""course name: {course.CourseName}, {course.HoursToComplete} hrs., {course.Price.ToString("C")}"");
```

## Summary

In this lesson, we've covered various methods to query data in EF Core using the `Course` table as our example. Understanding these methods will help you retrieve data more effectively and ensure your queries are optimized for performance.

2024-08-11
