```markdown
# LINQ Tutorial: Filtering, Lazy Loading, and Immediate Execution

## Overview
This tutorial covers various LINQ operations, including filtering data, understanding lazy loading versus immediate execution, and using extension methods for LINQ queries. We will work with a sample set of `Employee` objects and a list of integers to demonstrate these concepts.

## Table of Contents
1. [Setup](#setup)
2. [Filtering Data](#filtering-data)
   - [Using Custom Filter Method](#using-custom-filter-method)
   - [Using `Where` Method](#using-where-method)
3. [Lazy Loading vs Immediate Execution](#lazy-loading-vs-immediate-execution)
4. [Different LINQ Syntax](#different-linq-syntax)
   - [Method Syntax](#method-syntax)
   - [Query Syntax](#query-syntax)
5. [Combining LINQ Queries](#combining-linq-queries)
6. [Conclusion](#conclusion)

## Setup

### Sample Data and Helper Classes
Ensure you have the following classes and data to work with:
- `Employee` class with properties like `Id`, `FirstName`, `LastName`, `HireDate`, `Gender`, `Department`, `HasHealthInsurance`, `HasPensionPlan`, and `Salary`.
- `Repository` class with a method `LoadEmployees()` that returns a list of sample employees.
- Extension methods in `ExtensionFunctional` for filtering and printing data.

### Sample Code
```csharp
// Employee Class
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime HireDate { get; set; }
    public string Gender { get; set; }
    public string Department { get; set; }
    public bool HasHealthInsurance { get; set; }
    public bool HasPensionPlan { get; set; }
    public decimal Salary { get; set; }

    public override string ToString()
    {
        return $"{Id}\t{LastName}, {FirstName}\t{HireDate.ToShortDateString()}\t{Gender}\t{Department}\t{HasHealthInsurance}\t{HasPensionPlan}\t${Salary:0.00}";
    }
}

// Repository Class
public static class Repository
{
    public static IEnumerable<Employee> LoadEmployees()
    {
        return new List<Employee>
        {
            // Sample Employee Data
            new Employee { Id = 1001, FirstName = "Cochran", LastName = "Cole", HireDate = new DateTime(2017, 11, 2), Gender = "male", Department = "FIMAMCE", HasHealthInsurance = false, HasPensionPlan = true, Salary = 103200m },
            new Employee { Id = 1002, FirstName = "Jaclyn", LastName = "Wolfe", HireDate = new DateTime(2018, 4, 14), Gender = "female", Department = "FIMAMCE", HasHealthInsurance = true, HasPensionPlan = false, Salary = 192400m },
            // Add more employees as needed
        };
    }
}

// Extension Methods
public static class ExtensionFunctional
{
    public static IEnumerable<Employee> Filter(this IEnumerable<Employee> source, Func<Employee, bool> predicate)
    {
        foreach (var employee in source)
        {
            if (predicate(employee))
            {
                yield return employee;
            }
        }
    }

    public static void Print<T>(this IEnumerable<T> source, string title)
    {
        if (source == null) return;
        Console.WriteLine($"\n{title}");
        foreach (var item in source)
        {
            Console.WriteLine(item);
        }
    }
}
```

## Filtering Data

### Using Custom Filter Method
The custom `Filter` method is an extension method for filtering data based on a predicate.

```csharp
var employees = Repository.LoadEmployees();
var femaleWithFnameStartsWithS01 = employees.Filter(x => x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));
femaleWithFnameStartsWithS01.Print("Female Employees with First Name Starting with 'S' - Custom Filter");
```

### Using `Where` Method
The `Where` method is a standard LINQ method for filtering data.

```csharp
var femaleWithFnameStartsWithS02 = employees.Where(x => x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));
femaleWithFnameStartsWithS02.Print("Female Employees with First Name Starting with 'S' - Where Method");
```

## Lazy Loading vs Immediate Execution

### Example
Lazy loading defers the execution of a query until the data is actually accessed, while immediate execution runs the query and stores the results immediately.

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
IEnumerable<int> evenNumbers = numbers.Where(x => x % 2 == 0); // Lazy loading
numbers.Add(10);
numbers.Add(12);
numbers.Remove(4);

foreach (var n in evenNumbers) // Immediate execution
{
    Console.Write($"{n} ");
}
```

## Different LINQ Syntax

### Method Syntax
Using extension methods for LINQ queries.

```csharp
var evenNumbersUsingExtensionWhere = numbers.Where(x => x % 2 == 0);
evenNumbersUsingExtensionWhere.Print("Even Numbers using Extension Where");
```

### Query Syntax
Using query syntax for LINQ queries.

```csharp
var evenNumbersUsingQuerySyntax = from n in numbers where n % 2 == 0 select n;
evenNumbersUsingQuerySyntax.Print("Even Numbers using Query Syntax");
```

## Combining LINQ Queries
Combining multiple LINQ queries to refine data selection.

```csharp
var employees = Repository.LoadEmployees();
var empMale = employees.Where(x => x.Gender == "male");
var empsSalaryOver300K = employees.Where(x => x.Salary >= 300_000);

empMale.Print("Male Employees");
empsSalaryOver300K.Print("Employees with Salary >= 300K");

var empMaleInHRDepartment = empMale.Where(x => x.Department.ToLowerInvariant() == "hr");
empMaleInHRDepartment.Print("Male Employees in HR Department");
```

## Conclusion
This tutorial demonstrated various aspects of LINQ, including filtering data, understanding lazy loading versus immediate execution, and using different syntax for LINQ queries. These concepts are essential for effectively querying and manipulating data using LINQ in C#.
```
