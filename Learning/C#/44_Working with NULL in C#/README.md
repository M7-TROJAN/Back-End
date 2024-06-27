# Working with Null in C#

## Table of Contents
1. [Compile Time vs Run Time](#compile-time-vs-run-time)
2. [Referencing vs Dereferencing](#referencing-vs-dereferencing)
3. [Nullable Value Types](#nullable-value-types)
4. [Nullable Reference Types](#nullable-reference-types)
5. [Nullable<T> (Nullable Value Types)](#nullable-t-nullable-value-types)
6. [Nullable Reference Types in .NET 6](#nullable-reference-types-in-net-6)
7. [Why Nullable Reference Types](#why-nullable-reference-types)
8. [Compiler Static Analysis](#compiler-static-analysis)
9. [Compiler Does Not Trace Method](#compiler-does-not-trace-method)
10. [Nullable Value Annotations](#nullable-value-annotations)
11. [Generics](#generics)
12. [Struct and Array](#struct-and-array)
13. [Nullable Context](#nullable-context)
14. [Default Keyword](#default-keyword)

## 1. Compile Time vs Run Time

**Compile Time** refers to the phase when the source code is translated into executable code by a compiler. Any errors detected during this phase, such as syntax errors or type mismatches, are compile-time errors.

**Run Time** refers to the phase when the compiled code is executed. Errors that occur during this phase, such as null reference exceptions or arithmetic errors, are run-time errors.

```csharp
// Compile-time error example
int number = "abc"; // This will cause a compile-time error due to type mismatch

// Run-time error example
string text = null;
Console.WriteLine(text.Length); // This will cause a run-time error (NullReferenceException)
```

## 2. Referencing vs Dereferencing

**Referencing** is when you assign an address to a pointer or reference variable.

**Dereferencing** is when you access the value at the address contained in a pointer or reference variable.

```csharp
string reference = "Hello, World!"; // Referencing a string
int length = reference.Length; // Dereferencing to get the length of the string
```

## 3. Nullable Value Types

Value types in C# (e.g., `int`, `float`, `bool`) cannot be null by default. However, you can create nullable value types using the `?` operator.

```csharp
int? nullableInt = null;
if (nullableInt.HasValue)
{
    Console.WriteLine(nullableInt.Value);
}
else
{
    Console.WriteLine("nullableInt is null");
}
```

## 4. Nullable Reference Types

With C# 8.0, you can enable nullable reference types to indicate that a reference type can be null. This helps in avoiding null reference exceptions.

```csharp
string? nullableString = null;
if (nullableString != null)
{
    Console.WriteLine(nullableString.Length);
}
```

## 5. Nullable<T> (Nullable Value Types)

`Nullable<T>` allows you to create nullable value types where `T` is the underlying value type (e.g., `int`, `float`, `bool`).

```csharp
Nullable<int> nullableInt = null;
if (nullableInt.HasValue)
{
    Console.WriteLine(nullableInt.Value);
}
else
{
    Console.WriteLine("nullableInt is null");
}
```

## 6. Nullable Reference Types in .NET 6

.NET 6 further improves support for nullable reference types with better annotations and static analysis.

```csharp
#nullable enable
string? name = null;
Console.WriteLine(name?.Length);
```

## 7. Why Nullable Reference Types

Using nullable reference types helps in identifying potential null references at compile time, thus reducing runtime errors.

```csharp
namespace L06.WhyNullableReferenceTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = null;
            string decision = IsLongName(name) ? "long" : "short";
            Console.WriteLine($"{name} is {decision}");
            Console.ReadKey();
        }

        static bool IsLongName(string? name)
        {
            if (name is null)
                return false;

            return name.Length > 10;
        }
    }
}
```

## 8. Compiler Static Analysis

The C# compiler performs static analysis to check for null references and other potential issues.

```csharp
namespace L07.CompilerStaticAnalysis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }

        private static string? GetName()
        {
            return null;
        }

        static bool Scenario1()
        {
            string name = string.Empty;
            return name.Length > 10;
        }

        static bool Scenario2()
        {
            string? name = GetName();
            if (name == null)
                return false;
            return name.Length > 10;
        }

        static bool MaybeNullScenario()
        {
            string? name = GetName();
            return name.Length > 10;
        }
    }
}
```

## 9. Compiler Does Not Trace Method

The compiler may not always trace the flow of data through methods, which can lead to null reference warnings.

```csharp
using System.Diagnostics.CodeAnalysis;

namespace L08.CompilerDoesNotTraceMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // var person = new Person(null, null);
            Console.ReadKey();
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class Student : Person
    {
        public string Major { get; set; }

        public Student(string firstName, string lastName, string major)
            : base(firstName, lastName)
        {
            SetMajor(major);
        }

        public Student(string firstName, string lastName) :
            base(firstName, lastName)
        {
            SetMajor();
        }

        [MemberNotNull(nameof(Major))]
        public void SetMajor(string? major = default)
        {
            Major = major ?? "Undeclared";
        }
    }
}
```

## 10. Nullable Value Annotations

Nullable value annotations help in indicating which values can be null and ensuring proper null checks.

```csharp
using System.Diagnostics.CodeAnalysis;

namespace L09.NullableValueAnnotations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fname = null!;
            string lname = null!;

            var person = new Person(fname, lname);

            Console.WriteLine(person.FirstName!.Length);

            Student st1 = new Student();
            Student? st2 = new Student();
            var st3 = new Student();

            Console.ReadKey();
        }
    }

    class Student { }

    public class Person
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName ?? "Anonymous";
            LastName = lastName ?? "Anonymous";
        }
    }
}
```

## 11. Generics

Generics allow you to define classes, methods, and structures with a placeholder for the data type.

```csharp
namespace L10.Generics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static T? DoSomething<T>(T source)
        {
            return source;
        }
    }
}
```

## 12. Struct and Array

Structs are value types that can contain reference types, and arrays can store multiple elements of the same type.

```csharp
using System.Diagnostics.CodeAnalysis;

namespace L11.StructAndArray
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] names = new string[10];
            var firstValue = names[0];
            Console.WriteLine(firstValue.ToUpper());
            Console.ReadLine();
        }

        static void Print(Student student)
        {
            Console.WriteLine($"First Name: {student.FirstName.ToUpper()}");
            Console.WriteLine($"Middle Name: {student.FirstName?.ToUpper()}");
            Console.WriteLine($"Last Name: {student.LastName.ToUpper()}");
        }
    }

    public struct Student
    {
        public string FirstName;
        public string? MiddleName;
        public string LastName;
    }
}
```

## 13. Nullable Context

The nullable context in C# can be enabled or disabled to control how the compiler treats nullable reference types.

```csharp
#nullable enable
string? nullableString = null;
if (nullableString != null)
{
    Console.WriteLine(nullableString.Length);
}
```

## 14. Default Keyword

The `default` keyword in C# is used to initialize variables or parameters with the default value of their type, including null for reference types.

```csharp
namespace L14.DefaultKeyword
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int defaultInt = default; // default value for int (0)
            string defaultString = default; // default value for string (null)
            Console.WriteLine(defaultInt);
            Console.WriteLine(defaultString);
            Console.ReadKey();
        }
    }
}
```

## Null-Forgiving Operator

The `null-forgiving operator` (`!`) is a feature in C# that tells the compiler that you are sure a nullable reference type is not null, even if it can't determine that on its own. This is useful when you have information the compiler does not, and it helps to avoid unnecessary null checks.

### Syntax

```csharp
string? nullableString = null;
string nonNullableString = nullableString!; // Telling the compiler that nullableString is not null
```

### Use Case

The null-forgiving operator is typically used when you're certain a variable isn't null at a specific point in your code, even if the compiler can't guarantee it.

### Example

```csharp
public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public void PrintName()
    {
        // Using the null-forgiving operator to tell the compiler that FirstName and LastName are not null
        Console.WriteLine(FirstName!.Length);
        Console.WriteLine(LastName!.Length);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Person person = new Person { FirstName = "John", LastName = "Doe" };
        person.PrintName();
    }
}
```

In this example, we know that `FirstName` and `LastName` are not null before calling `Length`, but the compiler doesn't. The `!` operator tells the compiler to treat them as non-nullable.

### Caution

- **Overuse**: Overusing the null-forgiving operator can lead to runtime null reference exceptions if you make a mistake.
- **Safety**: Always ensure that the value is genuinely not null before using the null-forgiving operator to avoid potential errors.

