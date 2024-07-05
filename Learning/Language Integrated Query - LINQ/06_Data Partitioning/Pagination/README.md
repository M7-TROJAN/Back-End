### Pagination in LINQ

Pagination is a technique used to divide large sets of data into smaller, more manageable pages. This is especially useful in scenarios where displaying or processing the entire dataset at once is impractical or inefficient. In LINQ, pagination can be achieved using the `Skip` and `Take` methods.

The `Skip` method is used to bypass a specified number of elements in a sequence, while the `Take` method is used to return a specified number of contiguous elements from the sequence.

Here's how you can implement pagination in LINQ:

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
        return
            $"" +
            $" {Index.ToString().PadLeft(3, '0')}\t" +
            $" {EmployeeNo.PadRight(13, ' ')}\t" +
            $" {Name.PadRight(20, ' ')}\t" +
            $" {Email.PadRight(32, ' ')}\t" +
            $" {String.Format("{0:C0}", Salary)}";
    }
}

var emps = new List<Employee>
{
    new Employee { Index = 1, EmployeeNo = "EMP001", Name = "Alice", Email = "alice@example.com", Salary = 3000 },
    new Employee { Index = 2, EmployeeNo = "EMP002", Name = "Bob", Email = "bob@example.com", Salary = 4000 },
    new Employee { Index = 3, EmployeeNo = "EMP003", Name = "Charlie", Email = "charlie@example.com", Salary = 2500 },
    new Employee { Index = 4, EmployeeNo = "EMP004", Name = "David", Email = "david@example.com", Salary = 5000 },
    new Employee { Index = 5, EmployeeNo = "EMP005", Name = "Eve", Email = "eve@example.com", Salary = 4500 },
    new Employee { Index = 6, EmployeeNo = "EMP006", Name = "Frank", Email = "frank@example.com", Salary = 3500 },
    new Employee { Index = 7, EmployeeNo = "EMP007", Name = "Grace", Email = "grace@example.com", Salary = 6000 },
    new Employee { Index = 8, EmployeeNo = "EMP008", Name = "Hank", Email = "hank@example.com", Salary = 3200 },
    new Employee { Index = 9, EmployeeNo = "EMP009", Name = "Ivy", Email = "ivy@example.com", Salary = 2800 },
    new Employee { Index = 10, EmployeeNo = "EMP010", Name = "Jack", Email = "jack@example.com", Salary = 3600 }
};
```

### Implementing Pagination

Assume we want to display 3 employees per page. We can achieve this using the `Skip` and `Take` methods.

**Example: Getting the second page (elements 4 to 6)**

**Using Method Syntax**
```csharp
int pageSize = 3;
int pageNumber = 1; // Pages are zero-indexed, so this is the second page

var pagedEmps = emps.Skip(pageSize * pageNumber).Take(pageSize);
pagedEmps.Print("Page 2");
```

**Using Query Syntax**
```csharp
var pagedEmps = (from employee in emps
                 select employee).Skip(pageSize * pageNumber).Take(pageSize);
pagedEmps.Print("Page 2");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Page 2                                              │
└───────────────────────────────────────────────────────┘

004     EMP004          David                  david@example.com                      $5,000
005     EMP005          Eve                    eve@example.com                        $4,500
006     EMP006          Frank                  frank@example.com                      $3,500
```

### Dynamic Pagination

To make pagination dynamic, you can create a function that takes the page size and page number as parameters.

**Example: Dynamic Pagination Method**

```csharp
public static IEnumerable<Employee> GetPagedEmployees(List<Employee> employees, int pageSize, int pageNumber)
{
    return employees.Skip(pageSize * pageNumber).Take(pageSize);
}

// Usage
int pageSize = 3;
int pageNumber = 2; // Third page
var pagedEmps = GetPagedEmployees(emps, pageSize, pageNumber);
pagedEmps.Print($"Page {pageNumber + 1}");
```

**Output:**
```
┌───────────────────────────────────────────────────────┐
│   Page 3                                              │
└───────────────────────────────────────────────────────┘

007     EMP007          Grace                   grace@example.com                       $6,000
008     EMP008          Hank                    hank@example.com                        $3,200
009     EMP009          Ivy                     ivy@example.com                         $2,800
```

### Benefits of Pagination

- **Performance**: Pagination helps improve performance by loading only a subset of data, reducing memory usage and processing time.
- **User Experience**: By displaying a limited number of items per page, it enhances the user experience, making it easier to navigate and view data.

### Summary

Pagination in LINQ is efficiently managed using the `Skip` and `Take` methods. By combining these methods, you can easily implement pagination to handle large datasets in a more manageable way, improving both performance and user experience.
