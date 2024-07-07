Great! Here’s a detailed lesson on the quantifiers in LINQ, including `Any`, `All`, and `Contains`:

### Quantifiers in LINQ

Quantifiers in LINQ are methods that return a boolean value based on the elements of a sequence. They help in determining whether some or all elements of a sequence satisfy a condition, or if a specific element is present in the sequence.

#### Sample Data

We'll use the following sample data for our examples:

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
    new Employee { Index = 1, EmployeeNo = "EMP001", Name = "Alice", Email = "alice@example.com", Salary = 3000, Skills = new List<string> { "C#", "SQL" } },
    new Employee { Index = 2, EmployeeNo = "EMP002", Name = "Bob", Email = "bob@example.com", Salary = 4000, Skills = new List<string> { "Java", "SQL" } },
    new Employee { Index = 3, EmployeeNo = "EMP003", Name = "Charlie", Email = "charlie@example.com", Salary = 2500, Skills = new List<string> { "Python", "Django" } },
    new Employee { Index = 4, EmployeeNo = "EMP004", Name = "David", Email = "david@example.com", Salary = 5000, Skills = new List<string> { "C#", "Azure" } },
    new Employee { Index = 5, EmployeeNo = "EMP005", Name = "Eve", Email = "eve@example.com", Salary = 4500, Skills = new List<string> { "JavaScript", "React" } }
};
```

### 1. Any

The `Any` method determines whether any element of a sequence satisfies a condition.

**Example: Checking if any employee has a salary greater than 4500**

**Using Method Syntax**
```csharp
bool hasHighSalary = emps.Any(e => e.Salary > 4500);
Console.WriteLine(hasHighSalary); // Output: True
```

**Using Query Syntax**
```csharp
bool hasHighSalary = (from e in emps select e).Any(e => e.Salary > 4500);
Console.WriteLine(hasHighSalary); // Output: True
```

### 2. All

The `All` method determines whether all elements of a sequence satisfy a condition.

**Example: Checking if all employees have a salary greater than 2000**

**Using Method Syntax**
```csharp
bool allHighSalary = emps.All(e => e.Salary > 2000);
Console.WriteLine(allHighSalary); // Output: True
```

**Using Query Syntax**
```csharp
bool allHighSalary = (from e in emps select e).All(e => e.Salary > 2000);
Console.WriteLine(allHighSalary); // Output: True
```

### 3. Contains

The `Contains` method determines whether a sequence contains a specified element.

**Example: Checking if a specific employee is in the list**

First, we create an employee object to check for its presence:

```csharp
var empToCheck = new Employee { Index = 3, EmployeeNo = "EMP003", Name = "Charlie", Email = "charlie@example.com", Salary = 2500, Skills = new List<string> { "Python", "Django" } };
```

**Using Method Syntax**
```csharp
bool containsEmployee = emps.Contains(empToCheck);
Console.WriteLine(containsEmployee); // Output: False
```

**Using Query Syntax**
```csharp
bool containsEmployee = (from e in emps select e).Contains(empToCheck);
Console.WriteLine(containsEmployee); // Output: False
```

> Note: The `Contains` method uses the default equality comparer, which might not work as expected with complex objects like `Employee`. You may need to override the `Equals` and `GetHashCode` methods in the `Employee` class or use a custom comparer.

### Using Quantifiers in LINQ

Quantifiers are powerful tools in LINQ for quickly evaluating sequences. Here are more detailed examples for each quantifier:

#### Any with Complex Conditions

**Example: Checking if any employee has both C# and SQL skills**

```csharp
bool hasCSharpAndSQL = emps.Any(e => e.Skills.Contains("C#") && e.Skills.Contains("SQL"));
Console.WriteLine(hasCSharpAndSQL); // Output: True
```

#### All with Multiple Conditions

**Example: Checking if all employees have an email address containing "example.com"**

```csharp
bool allEmailsCorrect = emps.All(e => e.Email.Contains("example.com"));
Console.WriteLine(allEmailsCorrect); // Output: True
```

#### Contains with a Custom Comparer

**Example: Checking if a specific employee is in the list using a custom comparer**

First, we create a custom comparer:

```csharp
public class EmployeeComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee x, Employee y)
    {
        return x.EmployeeNo == y.EmployeeNo;
    }

    public int GetHashCode(Employee obj)
    {
        return obj.EmployeeNo.GetHashCode();
    }
}
```

Then, we use the comparer with the `Contains` method:

```csharp
bool containsEmployeeWithComparer = emps.Contains(empToCheck, new EmployeeComparer());
Console.WriteLine(containsEmployeeWithComparer); // Output: True
```

These examples should give you a thorough understanding of how to use quantifiers in LINQ. Let me know if you have any questions or need further clarification!
