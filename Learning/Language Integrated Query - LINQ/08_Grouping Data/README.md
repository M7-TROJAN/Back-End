### Grouping Data in LINQ

Grouping data is a powerful feature in LINQ that allows you to organize elements in a sequence into groups based on a specified key. This can be very useful for categorizing data, performing aggregations, and simplifying complex data structures.

We'll cover the following methods:
- `GroupBy`
- `Lookup`

#### Sample Data

We'll use the same sample data for employees:

```csharp
public class Employee
{
    public int Index { get; set; }
    public string EmployeeNo { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public List<string> Skills { get; set; } = new List<string>();

    public override string ToString()
    {
        return $"{Index.ToString().PadLeft(3, '0')}\t" +
               $"{EmployeeNo.PadRight(13, ' ')}\t" +
               $"{Name.PadRight(20, ' ')}\t" +
               $"{Email.PadRight(32, ' ')}\t" +
               $"{String.Format("{0:C0}", Salary)}";
    }
}

var emps = new List<Employee>
{
    new Employee { Index = 1, EmployeeNo = "EMP001", Name = "Alice", Email = "alice@example.com", Salary = 3000 },
    new Employee { Index = 2, EmployeeNo = "EMP002", Name = "Bob", Email = "bob@example.com", Salary = 4000 },
    new Employee { Index = 3, EmployeeNo = "EMP003", Name = "Charlie", Email = "charlie@example.com", Salary = 2500 },
    new Employee { Index = 4, EmployeeNo = "EMP004", Name = "David", Email = "david@example.com", Salary = 5000 },
    new Employee { Index = 5, EmployeeNo = "EMP005", Name = "Eve", Email = "eve@example.com", Salary = 4500 }
};
```

### 1. GroupBy

The `GroupBy` method groups the elements of a sequence according to a specified key selector function and projects the elements for each group.

**Example: Grouping employees by their salary range**

**Using Method Syntax**
```csharp
var groupedBySalaryRange = emps.GroupBy(e => 
{
    if (e.Salary < 3000)
        return "Low Salary";
    else if (e.Salary < 4000)
        return "Medium Salary";
    else
        return "High Salary";
});

foreach (var group in groupedBySalaryRange)
{
    Console.WriteLine($"Group: {group.Key}");
    foreach (var emp in group)
    {
        Console.WriteLine(emp);
    }
    Console.WriteLine();
}
```

**Using Query Syntax**
```csharp
var groupedBySalaryRange = from employee in emps
                           group employee by employee.Salary < 3000 ? "Low Salary" :
                                            employee.Salary < 4000 ? "Medium Salary" : "High Salary" into salaryGroup
                           select salaryGroup;

foreach (var group in groupedBySalaryRange)
{
    Console.WriteLine($"Group: {group.Key}");
    foreach (var emp in group)
    {
        Console.WriteLine(emp);
    }
    Console.WriteLine();
}
```

**Output:**
```
Group: Medium Salary
001	EMP001        	Alice               	alice@example.com                	$3,000

Group: High Salary
002	EMP002        	Bob                 	bob@example.com                  	$4,000
004	EMP004        	David               	david@example.com                	$5,000
005	EMP005        	Eve                 	eve@example.com                  	$4,500

Group: Low Salary
003	EMP003        	Charlie             	charlie@example.com              	$2,500
```

### 2. Lookup

A `Lookup` is similar to a `Dictionary`, but it can store multiple values for each key. Unlike `GroupBy`, a `Lookup` is immutable once created.

**Example: Creating a Lookup for employees by their first letter of the name**

```csharp
var lookupByNameInitial = emps.ToLookup(e => e.Name[0]);

foreach (var group in lookupByNameInitial)
{
    Console.WriteLine($"Group: {group.Key}");
    foreach (var emp in group)
    {
        Console.WriteLine(emp);
    }
    Console.WriteLine();
}
```

**Output:**
```
Group: A
001	EMP001        	Alice               	alice@example.com                	$3,000

Group: B
002	EMP002        	Bob                 	bob@example.com                  	$4,000

Group: C
003	EMP003        	Charlie             	charlie@example.com              	$2,500

Group: D
004	EMP004        	David               	david@example.com                	$5,000

Group: E
005	EMP005        	Eve                 	eve@example.com                  	$4,500
```

These examples demonstrate how to group and lookup data using LINQ, enabling efficient data categorization and retrieval. You can use these methods to organize and analyze your data effectively.
