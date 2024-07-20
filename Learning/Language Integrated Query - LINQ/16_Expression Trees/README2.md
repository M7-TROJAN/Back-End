# Understanding Expression Trees in C#

Expression trees in C# represent code as data. This allows you to examine, modify, and execute code dynamically. They are particularly useful in scenarios like building dynamic queries or compiling code at runtime.

## Basic Example: Lambda Expressions vs. Expression Trees

Let's start by comparing a simple lambda expression to an expression tree.

```csharp
using System;
using System.Linq.Expressions;

internal class Program
{
    static void Main(string[] args)
    {
        // Lambda expression
        Func<int, bool> IsEven = (num) => num % 2 == 0;
        Console.WriteLine(IsEven(10)); // True
        Console.WriteLine(IsEven.Invoke(10)); // True

        // Expression tree
        Expression<Func<int, bool>> IsEvenExpression = (num) => num % 2 == 0;
        Func<int, bool> IsEvenV2 = IsEvenExpression.Compile();
        Console.WriteLine(IsEvenV2(10)); // True

        Console.ReadKey();
    }

    static bool IsEvenMethod(int num)
    {
        return num % 2 == 0;
    }
}
```

### Explanation

- **Lambda Expression**: `Func<int, bool> IsEven = (num) => num % 2 == 0;` is a simple lambda expression that checks if a number is even.
  - `IsEven(10)` and `IsEven.Invoke(10)` both invoke the lambda, returning `true` for even numbers.

- **Expression Tree**: `Expression<Func<int, bool>> IsEvenExpression = (num) => num % 2 == 0;` creates an expression tree.
  - `IsEvenExpression.Compile()` compiles the expression tree into executable code.
  - The compiled function `IsEvenV2(10)` also returns `true`.

## Decomposing an Expression Tree

An expression tree can be decomposed into its constituent parts, allowing inspection of its structure.

```csharp
Expression<Func<int, bool>> IsNegativeExpression = (num) => num < 0;

ParameterExpression numParam = IsNegativeExpression.Parameters[0];
BinaryExpression operation = (BinaryExpression)IsNegativeExpression.Body;
ParameterExpression left = (ParameterExpression)operation.Left;
ConstantExpression right = (ConstantExpression)operation.Right;

Console.WriteLine($"Decomposed Expression: " +
    $"{numParam.Name} => {left.Name} {operation.NodeType} {right.Value}");
```

### Explanation

- **IsNegativeExpression**: This expression tree checks if a number is negative.
  - `numParam` retrieves the parameter of the lambda expression (`num`).
  - `operation` retrieves the body of the expression, which is a binary operation (`num < 0`).
  - `left` and `right` represent the left and right sides of the binary operation, respectively.

- The output is:
  ```
  Decomposed Expression: num => num LessThan 0
  ```

## Building Expression Trees Manually

You can construct expression trees programmatically using the `Expression` class methods.

```csharp
// (num) => num % 2 == 0

ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
    
ConstantExpression zeroParam = Expression.Constant(0, typeof(int));
ConstantExpression twoParam = Expression.Constant(2, typeof(int));
BinaryExpression moduloBinaryExpression = Expression.Modulo(numParam, twoParam);
BinaryExpression isEvenBinaryExpression = Expression.Equal(moduloBinaryExpression, zeroParam);
Expression<Func<int, bool>> IsEvenExpression = Expression.Lambda<Func<int, bool>>(
    isEvenBinaryExpression, new ParameterExpression[] { numParam });

var isEven = IsEvenExpression.Compile();
Console.WriteLine(isEven(10)); // True
Console.WriteLine(isEven(9));  // False
```

### Explanation

- **ParameterExpression**: Represents a parameter `num` of type `int`.
- **ConstantExpression**: Represents constant values `0` and `2`.
- **BinaryExpression**:
  - `moduloBinaryExpression` represents the modulus operation (`num % 2`).
  - `isEvenBinaryExpression` represents the equality check (`num % 2 == 0`).
- **Expression.Lambda**: Combines these expressions into a lambda expression tree.
- **Compile**: Converts the expression tree into executable code.

### Output

- `isEven(10)` returns `true` because 10 is even.
- `isEven(9)` returns `false` because 9 is not even.

## Use Cases in Real-World Scenarios

### Dynamic Query Generation

Expression trees are commonly used to build dynamic queries. For instance, in ORMs like Entity Framework, LINQ queries are translated into SQL queries at runtime.

```csharp
using System;
using System.Linq;
using System.Linq.Expressions;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
}

class Program
{
    static void Main()
    {
        // Create a parameter expression for the Employee parameter.
        ParameterExpression param = Expression.Parameter(typeof(Employee), "e");
        
        // Create a property expression for the Salary property.
        MemberExpression property = Expression.Property(param, "Salary");
        
        // Create a constant expression for the value 5000.
        ConstantExpression constant = Expression.Constant(5000m);
        
        // Create a binary expression for the greater than operation.
        BinaryExpression greaterThan = Expression.GreaterThan(property, constant);
        
        // Create a lambda expression for the predicate (e => e.Salary > 5000).
        var lambda = Expression.Lambda<Func<Employee, bool>>(greaterThan, param).Compile();
        
        // Example data.
        var employees = new[]
        {
            new Employee { Id = 1, Name = "Alice", Salary = 4000 },
            new Employee { Id = 2, Name = "Bob", Salary = 6000 },
            new Employee { Id = 3, Name = "Charlie", Salary = 7000 }
        };
        
        // Filter employees using the compiled lambda expression.
        var highEarners = employees.Where(lambda);
        
        foreach (var employee in highEarners)
        {
            Console.WriteLine($"{employee.Name} earns more than 5000");
        }
        // Output:
        // Bob earns more than 5000
        // Charlie earns more than 5000
    }
}
```

### Expression Tree Serialization

Expression trees can be serialized and deserialized, allowing for remote execution or storage of expressions.

```csharp
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static void Main()
    {
        // Create a simple expression tree.
        Expression<Func<int, int>> expr = x => x * 2;
        
        // Serialize the expression tree to a file.
        using (FileStream stream = new FileStream("expression.dat", FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, expr);
        }
        
        // Deserialize the expression tree from the file.
        Expression<Func<int, int>> deserializedExpr;
        using (FileStream stream = new FileStream("expression.dat", FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            deserializedExpr = (Expression<Func<int, int>>)formatter.Deserialize(stream);
        }
        
        // Compile and execute the deserialized expression tree.
        var func = deserializedExpr.Compile();
        int result = func(5);
        
        Console.WriteLine($"Deserialized expression result: {result}"); // Output: Deserialized expression result: 10
    }
}
```

## Conclusion

Expression trees are a powerful feature in C# that allow you to represent and manipulate code as data. They are essential for building dynamic queries, performing remote execution, and enabling advanced scenarios in LINQ. Understanding expression trees and their various types and methods allows developers to leverage the full potential of LINQ and create more flexible and dynamic applications.