### Grouping Data in LINQ

Grouping data is a powerful feature in LINQ that allows you to organize elements in a sequence into groups based on a specified key. This can be very useful for categorizing data, performing aggregations, and simplifying complex data structures.

We'll cover the following methods:
- `GroupBy`
- `Lookup`
- `GroupJoin`

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


### 3. GroupJoin

The `GroupJoin` method performs a group join, which correlates the elements of two sequences based on a key and groups the results. It's particularly useful for hierarchical data structures, where you want to group related data from one collection based on elements from another collection.

#### Sample Data

In addition to the employee data, let's add a collection of departments:

```csharp
public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }

    public override string ToString()
    {
        return $"{DepartmentId.ToString().PadLeft(3, '0')}\t{DepartmentName}";
    }
}

var departments = new List<Department>
{
    new Department { DepartmentId = 1, DepartmentName = "HR" },
    new Department { DepartmentId = 2, DepartmentName = "IT" },
    new Department { DepartmentId = 3, DepartmentName = "Finance" }
};

var emps = new List<Employee>
{
    new Employee { Index = 1, EmployeeNo = "EMP001", Name = "Alice", Email = "alice@example.com", Salary = 3000, Skills = new List<string>{"HR"}},
    new Employee { Index = 2, EmployeeNo = "EMP002", Name = "Bob", Email = "bob@example.com", Salary = 4000, Skills = new List<string>{"IT"}},
    new Employee { Index = 3, EmployeeNo = "EMP003", Name = "Charlie", Email = "charlie@example.com", Salary = 2500, Skills = new List<string>{"Finance"}},
    new Employee { Index = 4, EmployeeNo = "EMP004", Name = "David", Email = "david@example.com", Salary = 5000, Skills = new List<string>{"IT"}},
    new Employee { Index = 5, EmployeeNo = "EMP005", Name = "Eve", Email = "eve@example.com", Salary = 4500, Skills = new List<string>{"Finance"}}
};
```

### Using GroupJoin

**Example: Grouping employees by their departments**

**Using Method Syntax**

```csharp
var groupJoin = departments.GroupJoin(emps,
                                      dept => dept.DepartmentName,
                                      emp => emp.Skills.FirstOrDefault(),
                                      (dept, empsGroup) => new
                                      {
                                          Department = dept,
                                          Employees = empsGroup
                                      });

foreach (var item in groupJoin)
{
    Console.WriteLine($"Department: {item.Department.DepartmentName}");
    foreach (var emp in item.Employees)
    {
        Console.WriteLine($"  {emp}");
    }
}
```

**Using Query Syntax**

```csharp
var groupJoin = from dept in departments
                join emp in emps on dept.DepartmentName equals emp.Skills.FirstOrDefault() into empGroup
                select new
                {
                    Department = dept,
                    Employees = empGroup
                };

foreach (var item in groupJoin)
{
    Console.WriteLine($"Department: {item.Department.DepartmentName}");
    foreach (var emp in item.Employees)
    {
        Console.WriteLine($"  {emp}");
    }
}
```

**Output:**
```
Department: HR
  001	EMP001        	Alice               	alice@example.com                	$3,000
Department: IT
  002	EMP002        	Bob                 	bob@example.com                  	$4,000
  004	EMP004        	David               	david@example.com                	$5,000
Department: Finance
  003	EMP003        	Charlie             	charlie@example.com              	$2,500
  005	EMP005        	Eve                 	eve@example.com                  	$4,500
```

In this example:
- We grouped employees by their department.
- Each department has a list of employees belonging to that department.
- `GroupJoin` allows for hierarchical data organization, making it easy to navigate and present related data.

### Explanation

- **Outer sequence (departments):** The first collection (`departments`) represents the outer sequence.
- **Inner sequence (employees):** The second collection (`emps`) represents the inner sequence.
- **Outer key selector (dept.DepartmentName):** The key selector for the outer sequence to match elements from the inner sequence.
- **Inner key selector (emp.Skills.FirstOrDefault()):** The key selector for the inner sequence to match elements from the outer sequence.
- **Result selector ((dept, empsGroup) => new { ... }):** Defines how to project the results into a new form, in this case, an anonymous type with the department and its employees.

This provides a comprehensive way to group and present related data efficiently.

