### Sorting Data in LINQ

We'll cover the following methods:
1. **OrderBy**
2. **OrderByDescending**
3. **OrderBy with Custom Comparer**
4. **Reverse**
5. **ThenBy**
6. **ThenByDescending**

#### Sample Data

Let's define a sample list of employees to use in our examples:

```csharp
public class Employee
{
    public string Name { get; set; }
    public int Salary { get; set; }
}

var emps = new List<Employee>
{
    new Employee { Name = "Alice", Salary = 4000 },
    new Employee { Name = "Bob", Salary = 3500 },
    new Employee { Name = "Charlie", Salary = 2500 },
    new Employee { Name = "David", Salary = 5000 },
    new Employee { Name = "Alice", Salary = 3000 }
};
```

### 1. OrderBy

The `OrderBy` method sorts the elements of a sequence in ascending order according to a key.

**Example: Sorting by Name**

**Using Method Syntax**
```csharp
var sortedByName = emps.OrderBy(e => e.Name);
sortedByName.Print("Sorted by Name");
```

**Using Query Syntax**
```csharp
var sortedByName = from employee in emps
                   orderby employee.Name
                   select employee;
sortedByName.Print("Sorted by Name");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Sorted by Name                                      │
└───────────────────────────────────────────────────────┘

Alice (3000)
Alice (4000)
Bob (3500)
Charlie (2500)
David (5000)
```

### 2. OrderByDescending

The `OrderByDescending` method sorts the elements of a sequence in descending order according to a key.

**Example: Sorting by Salary**

**Using Method Syntax**
```csharp
var sortedBySalaryDesc = emps.OrderByDescending(e => e.Salary);
sortedBySalaryDesc.Print("Sorted by Salary Desc");
```

**Using Query Syntax**
```csharp
var sortedBySalaryDesc = from employee in emps
                         orderby employee.Salary descending
                         select employee;
sortedBySalaryDesc.Print("Sorted by Salary Desc");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Sorted by Salary Desc                                │
└───────────────────────────────────────────────────────┘

David (5000)
Alice (4000)
Bob (3500)
Alice (3000)
Charlie (2500)
```

### 3. OrderBy with Custom Comparer

You can provide a custom comparer to the `OrderBy` method to define your own comparison logic.

**Example: Sorting by Name Length**

**Using Method Syntax**
```csharp
var sortedByNameLength = emps.OrderBy(e => e.Name.Length);
sortedByNameLength.Print("Sorted by Name Length");
```

**Using Query Syntax**
```csharp
var sortedByNameLength = from employee in emps
                         orderby employee.Name.Length
                         select employee;
sortedByNameLength.Print("Sorted by Name Length");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Sorted by Name Length                               │
└───────────────────────────────────────────────────────┘

Bob (3500)
Alice (3000)
Alice (4000)
David (5000)
Charlie (2500)
```

### 4. Reverse

The `Reverse` method inverts the order of the elements in a sequence.

**Example: Reversing the Order of Employees**

**Using Method Syntax**
```csharp
var reversedEmps = emps.Reverse();
reversedEmps.Print("Reversed Employees");
```

**Using Query Syntax**
```csharp
var reversedEmps = (from employee in emps
                    select employee).Reverse();
reversedEmps.Print("Reversed Employees");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Reversed Employees                                  │
└───────────────────────────────────────────────────────┘

David (5000)
Charlie (2500)
Bob (3500)
Alice (3000)
Alice (4000)
```

### 5. ThenBy

The `ThenBy` method performs a subsequent ordering of the elements in a sequence in ascending order according to a second key.

**Example: Sorting by Salary, then by Name**

**Using Method Syntax**
```csharp
var sortedBySalaryThenName = emps.OrderBy(e => e.Salary).ThenBy(e => e.Name);
sortedBySalaryThenName.Print("Sorted by Salary, then Name");
```

**Using Query Syntax**
```csharp
var sortedBySalaryThenName = from employee in emps
                             orderby employee.Salary, employee.Name
                             select employee;
sortedBySalaryThenName.Print("Sorted by Salary, then Name");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Sorted by Salary, then Name                         │
└───────────────────────────────────────────────────────┘

Charlie (2500)
Alice (3000)
Bob (3500)
Alice (4000)
David (5000)
```

### 6. ThenByDescending

The `ThenByDescending` method is used for secondary sorting in descending order according to a second key.

**Example: Sorting by Name, then by Salary in Descending Order**

**Using Method Syntax**
```csharp
var sortedByNameThenSalaryDesc = emps.OrderBy(e => e.Name).ThenByDescending(e => e.Salary);
sortedByNameThenSalaryDesc.Print("Sorted by Name, Then by Salary Descending");
```

**Using Query Syntax**
```csharp
var sortedByNameThenSalaryDesc = from employee in emps
                                 orderby employee.Name, employee.Salary descending
                                 select employee;
sortedByNameThenSalaryDesc.Print("Sorted by Name, Then by Salary Descending");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Sorted by Name, Then by Salary Descending           │
└───────────────────────────────────────────────────────┘

Alice (4000)
Alice (3000)
Bob (3500)
Charlie (2500)
David (5000)
```

These examples demonstrate the full range of sorting capabilities in LINQ, including primary and secondary sorting, ascending and descending order, and custom comparers. This allows for flexible and powerful data sorting in various scenarios.