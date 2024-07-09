### Join Operations in LINQ

Join operations in LINQ are used to associate elements from different collections based on matching keys. There are two main types of joins in LINQ: `Join` and `GroupJoin`. 

#### Sample Data

We will use the following sample data:

```csharp
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime HireDate { get; set; }
    public string Gender { get; set; }
    public int DepartmentId { get; set; }
    public bool HasHealthInsurance { get; set; }
    public bool HasPensionPlan { get; set; }
    public decimal Salary { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, FirstName = "John", LastName = "Doe", HireDate = new DateTime(2010, 1, 1), Gender = "M", DepartmentId = 1, HasHealthInsurance = true, HasPensionPlan = true, Salary = 50000 },
    new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", HireDate = new DateTime(2012, 2, 2), Gender = "F", DepartmentId = 2, HasHealthInsurance = true, HasPensionPlan = false, Salary = 60000 },
    new Employee { Id = 3, FirstName = "Jim", LastName = "Brown", HireDate = new DateTime(2014, 3, 3), Gender = "M", DepartmentId = 1, HasHealthInsurance = false, HasPensionPlan = true, Salary = 55000 },
    new Employee { Id = 4, FirstName = "Jill", LastName = "Jones", HireDate = new DateTime(2016, 4, 4), Gender = "F", DepartmentId = 3, HasHealthInsurance = true, HasPensionPlan = true, Salary = 65000 }
};

var departments = new List<Department>
{
    new Department { Id = 1, Name = "HR" },
    new Department { Id = 2, Name = "IT" },
    new Department { Id = 3, Name = "Finance" }
};
```

### 1. Join

The `Join` method performs an inner join on two sequences based on matching keys.

**Example: Joining Employees with Departments**

**Using Query Syntax**
```csharp
var query = from emp in employees
            join dept in departments on emp.DepartmentId equals dept.Id
            select new { emp.FullName, dept.Name };

foreach (var item in query)
{
    Console.WriteLine($"{item.FullName} [{item.Name}]");
}
```

**Using Method Syntax**
```csharp
var query = employees.Join(departments,
                           emp => emp.DepartmentId,
                           dept => dept.Id,
                           (emp, dept) => new { emp.FullName, dept.Name });

foreach (var item in query)
{
    Console.WriteLine($"{item.FullName} [{item.Name}]");
}
```

**Output:**
```
John Doe [HR]
Jim Brown [HR]
Jane Smith [IT]
Jill Jones [Finance]
```

### 2. GroupJoin

The `GroupJoin` method correlates elements from two sequences by matching keys and groups the results.

**Example: Grouping Employees by Department**

**Using Query Syntax**
```csharp
var empGroups = from dept in departments
                join emp in employees on dept.Id equals emp.DepartmentId into empGroup
                select new { Department = dept.Name, Employees = empGroup };

foreach (var group in empGroups)
{
    Console.WriteLine($"Department: {group.Department}");
    foreach (var employee in group.Employees)
    {
        Console.WriteLine($"  {employee.FullName}");
    }
}
```

**Using Method Syntax**
```csharp
var query = departments.GroupJoin(employees,
                                  dept => dept.Id,
                                  emp => emp.DepartmentId,
                                  (dept, emps) => new { Department = dept.Name, Employees = emps });

foreach (var group in query)
{
    Console.WriteLine($"Department: {group.Department}");
    foreach (var employee in group.Employees)
    {
        Console.WriteLine($"  {employee.FullName}");
    }
}
```

**Output:**
```
Department: HR
  John Doe
  Jim Brown
Department: IT
  Jane Smith
Department: Finance
  Jill Jones
```