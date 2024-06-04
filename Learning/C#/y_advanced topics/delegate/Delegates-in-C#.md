# Delegates in C#

Delegates in C# are type-safe pointers to methods. They allow methods to be passed as parameters and enable callback functionality. C# provides three main built-in delegates: `Action`, `Func`, and `Predicate`. This document explains these delegates in detail and provides examples of their usage in different scenarios.

## Table of Contents
1. [Introduction to Delegates](#introduction-to-delegates)
2. [Action Delegate](#action-delegate)
3. [Func Delegate](#func-delegate)
4. [Predicate Delegate](#predicate-delegate)

## Introduction to Delegates

Delegates are used to reference methods with a specific signature. They are particularly useful for implementing callback methods, event handling, and functional programming techniques.

### Basic Syntax

```csharp
public delegate void MyDelegate(string message);
```

A delegate can be instantiated and used to call a method.

```csharp
MyDelegate del = new MyDelegate(SomeMethod);
del("Hello, World!");
```

## Action Delegate

The `Action` delegate represents a method that performs an action but does not return a value. It can take up to 16 parameters.

### Syntax

```csharp
public delegate void Action();
public delegate void Action<T>(T arg);
public delegate void Action<T1, T2>(T1 arg1, T2 arg2);
// ... up to 16 parameters
```

### Examples

#### Basic Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        Action greet = () => Console.WriteLine("Hello, World!");
        greet(); // Output: Hello, World!
    }
}
```

#### With Parameters

```csharp
using System;

class Program
{
    static void Main()
    {
        Action<string> greetWithName = name => Console.WriteLine($"Hello, {name}!");
        greetWithName("Alice"); // Output: Hello, Alice!
    }
}
```

#### With Multiple Parameters

```csharp
using System;

class Program
{
    static void Main()
    {
        Action<string, int> displayInfo = (name, age) => Console.WriteLine($"{name} is {age} years old.");
        displayInfo("Bob", 30); // Output: Bob is 30 years old.
    }
}
```

## Func Delegate

The `Func` delegate represents a method that can take zero or more parameters and returns a value. The last type parameter represents the return type.

### Syntax

```csharp
public delegate TResult Func<TResult>();
public delegate TResult Func<T, TResult>(T arg);
public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
// ... up to 16 parameters
```

### Examples

#### Basic Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        Func<int> getRandomNumber = () => new Random().Next(1, 100);
        int number = getRandomNumber();
        Console.WriteLine(number); // Output: Random number between 1 and 99
    }
}
```

#### With Parameters

```csharp
using System;

class Program
{
    static void Main()
    {
        Func<int, int, int> add = (x, y) => x + y;
        int result = add(5, 3);
        Console.WriteLine(result); // Output: 8
    }
}
```

#### With Multiple Parameters

```csharp
using System;

class Program
{
    static void Main()
    {
        Func<string, int, string> formatInfo = (name, age) => $"{name} is {age} years old.";
        string info = formatInfo("Charlie", 25);
        Console.WriteLine(info); // Output: Charlie is 25 years old.
    }
}
```

## Predicate Delegate

The `Predicate` delegate represents a method that takes a single parameter and returns a Boolean value. It is typically used for methods that perform a test or check.

### Syntax

```csharp
public delegate bool Predicate<T>(T obj);
```

### Examples

#### Basic Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        bool result = isEven(4);
        Console.WriteLine(result); // Output: True
    }
}
```

#### With Collections

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        List<int> evenNumbers = numbers.FindAll(isEven);
        Console.WriteLine(string.Join(", ", evenNumbers)); // Output: 2, 4, 6
    }
}
```

#### With Custom Types

```csharp
using System;
using System.Collections.Generic;

class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Salary { get; set; }
}

class Program
{
    static void Main()
    {
        List<Employee> employees = new List<Employee>
        {
            new Employee { Name = "Alice", Age = 30, Salary = 50000 },
            new Employee { Name = "Bob", Age = 40, Salary = 60000 },
            new Employee { Name = "Charlie", Age = 35, Salary = 55000 }
        };

        Predicate<Employee> isHighEarner = emp => emp.Salary > 55000;
        List<Employee> highEarners = employees.FindAll(isHighEarner);
        
        foreach (var emp in highEarners)
        {
            Console.WriteLine($"{emp.Name}, Salary: {emp.Salary}");
        }
        // Output: Bob, Salary: 60000
    }
}
```

## Conclusion

Delegates in C# provide a powerful mechanism for encapsulating method references and enabling functional programming techniques. The built-in `Action`, `Func`, and `Predicate` delegates cover most common scenarios for working with methods, making the code more concise and readable. By understanding these delegates and their applications, you can write more flexible and maintainable code.
```

This markdown file provides a comprehensive explanation of the delegates in C# with examples demonstrating different scenarios. Save it as `delegates_in_csharp.md` and use it for reference or documentation purposes.
