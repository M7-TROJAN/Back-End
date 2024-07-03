
# LINQ Tutorial: Projection Operation

## Introduction
Projection in LINQ refers to the operation of transforming an object into a new form that is going to be used. This can involve constructing a new type, projecting a new property, or performing mathematical operations.

### Key Concepts:
- **Constructing a New Type**
- **Projecting a New Property**
- **Performing Mathematical Operations**

### LINQ Methods Covered:
- **Select**
- **SelectMany**
- **Zip**

## Table of Contents
1. [Constructing a New Type](#constructing-a-new-type)
2. [Projecting a New Property](#projecting-a-new-property)
3. [Performing Mathematical Operations](#performing-mathematical-operations)
4. [Using Select](#using-select)
5. [Using SelectMany](#using-selectmany)
6. [Using Zip](#using-zip)
7. [Conclusion](#conclusion)

## Constructing a New Type

### Description
Constructing a new type involves creating a new object with a different structure or properties from the original object. This is often done using anonymous types or creating instances of custom classes.

### Example
```csharp
var employees = Repository.LoadEmployees();

var employeeDetails = employees.Select(e => new
{
    FullName = $"{e.FirstName} {e.LastName}",
    e.Department,
    AnnualSalary = e.Salary * 12
});

employeeDetails.Print("Employee Details with Annual Salary");
```

### Output
```
Employee Details with Annual Salary:
- { FullName = "John Doe", Department = "IT", AnnualSalary = 72000 }
- { FullName = "Jane Smith", Department = "HR", AnnualSalary = 60000 }
- { FullName = "Michael Johnson", Department = "Finance", AnnualSalary = 84000 }
```

### Explanation
In this example, a new anonymous type is created with properties `FullName`, `Department`, and `AnnualSalary`.

## Projecting a New Property

### Description
Projecting a new property involves selecting specific properties from the original object or creating new properties based on the original object's properties.

### Example
```csharp
var employees = Repository.LoadEmployees();

var employeeNames = employees.Select(e => e.FirstName);

employeeNames.Print("Employee First Names");
```

### Output
```
Employee First Names:
- John
- Jane
- Michael
```

### Explanation
Here, only the `FirstName` property of each employee is projected into the result.

## Performing Mathematical Operations

### Description
Mathematical operations can be performed within the projection to create new values based on calculations.

### Example
```csharp
var employees = Repository.LoadEmployees();

var employeeBonuses = employees.Select(e => new
{
    e.FirstName,
    e.LastName,
    Bonus = e.Salary * 0.1m
});

employeeBonuses.Print("Employee Bonuses");
```

### Output
```
Employee Bonuses:
- { FirstName = "John", LastName = "Doe", Bonus = 500 }
- { FirstName = "Jane", LastName = "Smith", Bonus = 400 }
- { FirstName = "Michael", LastName = "Johnson", Bonus = 600 }
```

### Explanation
A new property `Bonus` is created by multiplying the `Salary` by 0.1.

## Using Select

### Description
The `Select` method projects each element of a sequence into a new form.

### Example
```csharp
var employees = Repository.LoadEmployees();

var employeeDepartments = employees.Select(e => e.Department).Distinct();

employeeDepartments.Print("Employee Departments");
```

### Output
```
Employee Departments:
- IT
- HR
- Finance
```

### Explanation
The `Select` method is used to project the `Department` property of each employee, and `Distinct` is used to remove duplicates.

## Using SelectMany

### Description
The `SelectMany` method projects each element of a sequence to an `IEnumerable<T>` and flattens the resulting sequences into one sequence.

### Example
```csharp
var employees = Repository.LoadEmployees();
var projects = new List<string[]> {
    new string[] { "Project A", "Project B" },
    new string[] { "Project C", "Project D" },
    new string[] { "Project E" }
};

var allProjects = projects.SelectMany(p => p);

allProjects.Print("All Projects");
```

### Output
```
All Projects:
- Project A
- Project B
- Project C
- Project D
- Project E
```

### Explanation
`SelectMany` is used to flatten the list of project arrays into a single sequence of projects.

## Using Zip

### Description
The `Zip` method applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.

### Example
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var words = new List<string> { "One", "Two", "Three", "Four", "Five" };

var zipped = numbers.Zip(words, (n, w) => $"{n}: {w}");

zipped.Print("Numbers and Words");
```

### Output
```
Numbers and Words:
- 1: One
- 2: Two
- 3: Three
- 4: Four
- 5: Five
```

### Explanation
`Zip` is used to combine elements from `numbers` and `words` into a new sequence where each element is a string combining the number and the word.

## Conclusion

Projection operations in LINQ provide powerful ways to transform data into different forms. Including outputs under each example helps illustrate how each LINQ method affects the data and what results to expect.
