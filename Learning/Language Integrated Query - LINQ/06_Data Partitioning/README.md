### Data Partitioning in LINQ

Data partitioning refers to the process of dividing a large set of data into smaller, more manageable parts based on specific criteria. In LINQ, various methods are provided to achieve this, allowing for efficient data manipulation and retrieval.
In this lesson, we'll explore various data partitioning methods in LINQ, such as `First`, `FirstOrDefault`, `Last`, `LastOrDefault`, `Skip`, `SkipLast`, `SkipWhile`, and more. We'll use the following sample data:

#### Sample Data

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

### 1. First

The `First` method returns the first element of a sequence.

**Example: Getting the first employee**

**Using Method Syntax**
```csharp
var firstEmployee = emps.First();
Console.WriteLine(firstEmployee);
```

**Using Query Syntax**
```csharp
var firstEmployee = (from employee in emps select employee).First();
Console.WriteLine(firstEmployee);
```

**Output:**
```
001	EMP001        	Alice               	alice@example.com                	$3,000
```

### 2. FirstOrDefault

The `FirstOrDefault` method returns the first element of a sequence, or a default value if no element is found.

**Example: Getting the first employee with salary greater than 6000**

**Using Method Syntax**
```csharp
var firstHighSalaryEmployee = emps.FirstOrDefault(e => e.Salary > 6000);
Console.WriteLine(firstHighSalaryEmployee ?? "No employee found");
```

**Using Query Syntax**
```csharp
var firstHighSalaryEmployee = (from employee in emps where employee.Salary > 6000 select employee).FirstOrDefault();
Console.WriteLine(firstHighSalaryEmployee ?? "No employee found");
```

**Output:**
```
No employee found
```

### 3. Last

The `Last` method returns the last element of a sequence.

**Example: Getting the last employee**

**Using Method Syntax**
```csharp
var lastEmployee = emps.Last();
Console.WriteLine(lastEmployee);
```

**Using Query Syntax**
```csharp
var lastEmployee = (from employee in emps select employee).Last();
Console.WriteLine(lastEmployee);
```

**Output:**
```
005	EMP005        	Eve                 	eve@example.com                  	$4,500
```

### 4. LastOrDefault

The `LastOrDefault` method returns the last element of a sequence, or a default value if no element is found.

**Example: Getting the last employee with salary less than 2000**

**Using Method Syntax**
```csharp
var lastLowSalaryEmployee = emps.LastOrDefault(e => e.Salary < 2000);
Console.WriteLine(lastLowSalaryEmployee ?? "No employee found");
```

**Using Query Syntax**
```csharp
var lastLowSalaryEmployee = (from employee in emps where employee.Salary < 2000 select employee).LastOrDefault();
Console.WriteLine(lastLowSalaryEmployee ?? "No employee found");
```

**Output:**
```
No employee found
```

### 5. Skip

The `Skip` method bypasses a specified number of elements in a sequence and then returns the remaining elements.

**Example: Skipping the first two employees**

**Using Method Syntax**
```csharp
var skippedEmps = emps.Skip(2);
skippedEmps.Print("Skipped First Two Employees");
```

**Using Query Syntax**
```csharp
var skippedEmps = (from employee in emps select employee).Skip(2);
skippedEmps.Print("Skipped First Two Employees");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Skipped First Two Employees                         │
└───────────────────────────────────────────────────────┘

003	EMP003        	Charlie             	charlie@example.com               	$2,500
004	EMP004        	David               	david@example.com                 	$5,000
005	EMP005        	Eve                 	eve@example.com                   	$4,500
```

### 6. SkipLast

The `SkipLast` method bypasses a specified number of elements at the end of a sequence and returns the remaining elements.

**Example: Skipping the last two employees**

**Using Method Syntax**
```csharp
var skippedLastEmps = emps.SkipLast(2);
skippedLastEmps.Print("Skipped Last Two Employees");
```

**Using Query Syntax**
```csharp
var skippedLastEmps = (from employee in emps select employee).SkipLast(2);
skippedLastEmps.Print("Skipped Last Two Employees");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Skipped Last Two Employees                          │
└───────────────────────────────────────────────────────┘

001	EMP001        	Alice               	alice@example.com                 	$3,000
002	EMP002        	Bob                 	bob@example.com                   	$4,000
003	EMP003        	Charlie             	charlie@example.com               	$2,500
```

### 7. SkipWhile

The `SkipWhile` method bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.

**Example: Skipping employees while salary is less than 4000**

**Using Method Syntax**
```csharp
var skippedWhileEmps = emps.SkipWhile(e => e.Salary < 4000);
skippedWhileEmps.Print("Skipped While Salary < 4000");
```

**Using Query Syntax**
```csharp
var skippedWhileEmps = (from employee in emps select employee).SkipWhile(e => e.Salary < 4000);
skippedWhileEmps.Print("Skipped While Salary < 4000");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Skipped While Salary < 4000                         │
└───────────────────────────────────────────────────────┘

002	EMP002        	Bob                 	bob@example.com                   	$4,000
004	EMP004        	David               	david@example.com                 	$5,000
005	EMP005        	Eve                 	eve@example.com                   	$4,500
```

### 8. Chaining: Skip().Last()

You can chain multiple LINQ methods to perform complex operations. 

**Example: Skipping the first employee and getting the last of the remaining employees**

```csharp
var emp = emps.Skip(1).Last();
Console.WriteLine(emp);
```

**Output:**
```
005	EMP005        	Eve                 	eve@example.com                  	$4,500
```

### 9. Aggregate

The `Aggregate` method applies an accumulator function over a sequence.

**Example: Concatenating employee names**

```csharp
var concatenatedNames = emps.Aggregate("", (current, next) => current + next.Name + ", ");
Console.WriteLine(concatenatedNames.TrimEnd(',', ' '));
```

**Output:**
```
Alice, Bob, Charlie, David, Eve
```

### 10. Take

The `Take` method returns a specified number of contiguous elements from the start of a sequence.

**Example: Taking the first three employees**

**Using Method Syntax**
```csharp
var takenEmps = emps.Take(3);
takenEmps.Print("Taken First Three Employees");
```

**Using Query Syntax**
```csharp
var takenEmps = (from employee in emps select employee).Take(3);
takenEmps.Print("Taken First Three Employees");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Taken First Three Employees                         │
└───────────────────────────────────────────────────────┘

001	EMP001        	Alice               	alice@example.com                	$3,000
002	EMP002        	Bob                 	bob@example.com                  	$4,000
003	EMP003        	Charlie             	charlie@example.com               	$2,500
```

### 11. TakeLast

The `TakeLast` method returns a specified number of contiguous elements from the end of a sequence.

**Example: Taking the last three employees**

**Using Method Syntax**
```csharp
var takenLastEmps = emps.TakeLast(3);
takenLastEmps.Print("Taken Last Three Employees");
```

**Using Query Syntax**
```csharp
var takenLastEmps = (from employee in emps select employee).TakeLast(3);
takenLastEmps.Print("Taken Last Three Employees");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Taken Last Three Employees                          │
└───────────────────────────────────────────────────────┘

003	EMP003        	Charlie             	charlie@example.com               	$2,500
004	EMP004        	David               	david@example.com                 	$5,000
005	EMP005        	Eve                 	eve@example.com                   	$4,500
```

### 12. TakeWhile

The `TakeWhile` method returns elements from the start of a sequence as long as a specified condition is true.

**Example: Taking employees while salary is less than 4000**

**Using Method Syntax**
```csharp
var takenWhileEmps = emps.TakeWhile(e => e.Salary < 4000);
takenWhileEmps.Print("Taken While Salary < 4000");
```

**Using Query Syntax**
```csharp
var takenWhileEmps = (from employee in emps select employee).TakeWhile(e => e.Salary < 4000);
takenWhileEmps.Print("Taken While Salary < 4000");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Taken While Salary < 4000                           │
└───────────────────────────────────────────────────────┘

001	EMP001        	Alice               	alice@example.com                	$3,000
```

### 13. Pagination

Pagination is the process of dividing a large set of data into smaller chunks (pages). The combination of `Skip` and `Take` is commonly used for pagination.

**Example: Getting the second page of employees with 2 employees per page**

**Using Method Syntax**
```csharp
int pageSize = 2;
int pageNumber = 2;
var pagedEmps = emps.Skip((pageNumber - 1) * pageSize).Take(pageSize);
pagedEmps.Print($"Page {pageNumber} with {pageSize} employees per page");
```

**Using Query Syntax**
```csharp
int pageSize = 2;
int pageNumber = 2;
var pagedEmps = (from employee in emps select employee)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize);
pagedEmps.Print($"Page {pageNumber} with {pageSize} employees per page");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Page 2 with 2 employees per page                    │
└───────────────────────────────────────────────────────┘

003	EMP003        	Charlie             	charlie@example.com               	$2,500
004	EMP004        	David               	david@example.com                 	$5,000
```

### 14. Chunk

The `Chunk` method divides a sequence into chunks of a specified size.

**Example: Chunking employees into groups of 2**

**Using Method Syntax**
```csharp
var chunkedEmps = emps.Chunk(2);
foreach (var chunk in chunkedEmps)
{
    Console.WriteLine("Chunk:");
    chunk.Print("");
}
```

**Using Query Syntax**
```csharp
var chunkedEmps = from employee in emps select employee into chunks select chunks.Chunk(2);
foreach (var chunk in chunkedEmps)
{
    Console.WriteLine("Chunk:");
    chunk.Print("");
}
```

**Output:**
```
Chunk:
┌───────────────────────────────────────────────────────┐
│                                                       │
└───────────────────────────────────────────────────────┘

001	EMP001        	Alice               	alice@example.com                	$3,000
002	EMP002        	Bob                 	bob@example.com                  	$4,000

Chunk:
┌───────────────────────────────────────────────────────┐
│                                                       │
└───────────────────────────────────────────────────────┘

003	EMP003        	Charlie             	charlie@example.com               	$2,500
004	EMP004        	David               	david@example.com                 	$5,000

Chunk:
┌───────────────────────────────────────────────────────┐
│                                                       │
└───────────────────────────────────────────────────────┘

005	EMP005        	Eve                 	eve@example.com                   	$4,500
```

These examples cover a variety of data partitioning techniques in LINQ, demonstrating how to extract, skip, and partition data in different ways using both method syntax and query syntax.
