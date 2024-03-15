# Working with Assemblies in C#

In C#, an assembly is a logical unit that contains compiled code. It can be a DLL or an executable (EXE). Assemblies store metadata that describes the types, members, and references used in the code. Working with assemblies allows developers to access information about types, load and execute code dynamically, and manage dependencies.

This README provides an overview of working with assemblies in C#, including examples of accessing assembly information using reflection.

## Accessing Assembly Information

### Method 1: Using `typeof` Operator

```csharp
using System;
using System.Reflection;
using DemoLib;

namespace Assemblies
{
    class Program
    {
        static void Main()
        {
            // Retrieving assembly information from a specified type
            var type = typeof(Employee);
            var assembly = type.Assembly;
            Console.WriteLine($"Assembly Information from Type: {assembly}\n");

            // Additional example: Accessing assembly information of DateTime
            Console.WriteLine($"Assembly Information of DateTime: {typeof(DateTime).Assembly}\n");

            // Calling a method from the DemoLib project to trace assembly information
            Demo.Trace();
        }
    }

    class Employee
    {

    }
}
```

### Method 2: Using `Assembly` Class

```csharp
using System;
using System.Reflection;
using DemoLib;

namespace Assemblies
{
    class Program
    {
        static void Main()
        {
            // Retrieving assembly information from the currently executing assembly
            var assembly2 = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Assembly Information from Executing Assembly: {assembly2}\n");

            // Calling a method from the DemoLib project to trace assembly information
            Demo.Trace();
        }
    }
}
```

## DemoLib Project

The `DemoLib` project contains a method to trace assembly information:

```csharp
using System.Reflection;

namespace DemoLib
{
    public class Demo
    {
        public static void Trace()
        {
            Console.WriteLine("Tracing...\n");
            Console.WriteLine($"Executing Assembly: {Assembly.GetExecutingAssembly()}");
            Console.WriteLine($"Entry Assembly    : {Assembly.GetEntryAssembly()}");
            Console.WriteLine($"Calling Assembly  : {Assembly.GetCallingAssembly()}");
        }
    }
}
```

This method prints information about the executing, entry, and calling assemblies.

## Running the Examples

To run the examples:

1. Create a new C# project.
2. Copy and paste the code snippets into your project files.
3. Make sure to reference the `DemoLib` project or adjust the code accordingly.
4. Run the program to see the assembly information outputted to the console.

## Conclusion

Understanding how to work with assemblies in C# is essential for tasks such as dynamically loading types, managing dependencies, and accessing metadata. By leveraging reflection and the `Assembly` class, developers can gain insights into the structure and properties of assemblies, enabling more sophisticated and dynamic programming scenarios.