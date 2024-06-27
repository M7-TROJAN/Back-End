### `using` in C#

The `using` directive in C# simplifies code by making it easier to access classes and methods from other namespaces without needing to specify the fully qualified name every time. There are several types of `using` directives in C#:

1. **Namespace Import**
2. **Alias Directive**
3. **Static Import**
4. **Global Using Directive** (introduced in C# 10)

Let's go through each type with examples.

#### 1. Namespace Import

The most common use of `using` is to import namespaces. This allows you to use the classes and methods within those namespaces without specifying the full namespace.

**Example:**
```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");
    }
}
```
In this example, `using System;` allows you to use `Console.WriteLine` without needing to write `System.Console.WriteLine`.

#### 2. Alias Directive

You can use the `using` directive to create an alias for a long namespace or a class name. This is useful when you have long or conflicting names.

**Example:**
```csharp
using EGY = Continent.Region.Area.Country.Egypt;

class Program
{
    static void Main()
    {
        EGY.Cairo city = new EGY.Cairo();
        city.Greet();
    }
}

namespace Continent.Region.Area.Country.Egypt
{
    public class Cairo
    {
        public void Greet()
        {
            Console.WriteLine("Welcome to Cairo!");
        }
    }
}
```
In this example, `using EGY = Continent.Region.Area.Country.Egypt;` allows you to use `EGY` as a shorthand for the long namespace.

#### 3. Static Import

The `using static` directive allows you to import static members (methods, properties, etc.) from a class so that you can use them without the class name.

**Example:**
```csharp
using static System.Math;

class Program
{
    static void Main()
    {
        double result = Sqrt(16); // Instead of Math.Sqrt(16)
        Console.WriteLine(result); // Outputs 4
    }
}
```
In this example, `using static System.Math;` allows you to use `Sqrt` without needing to prefix it with `Math.`.

#### 4. Global Using Directive

The `global using` directive, introduced in C# 10, allows you to define `using` directives that apply to all files in the project. This is useful for commonly used namespaces.

**Example:**
```csharp
// In a file called GlobalUsings.cs
global using System;
global using EGY = Continent.Region.Area.Country.Egypt;
global using static System.Math;
```

With the `global using` directives defined, you can simplify the code in other files:

**Program.cs:**
```csharp
class Program
{
    static void Main()
    {
        // Using the alias directive
        EGY.Cairo city = new EGY.Cairo();
        city.Greet();

        // Using the static import directive
        double result = Sqrt(25); // Instead of Math.Sqrt(25)
        Console.WriteLine(result); // Outputs 5
    }
}

namespace Continent.Region.Area.Country.Egypt
{
    public class Cairo
    {
        public void Greet()
        {
            Console.WriteLine("Welcome to Cairo!");
        }
    }
}
```

### Combining `using` with Alias and Static Import

You can combine these `using` directives in a single program to simplify and clarify your code.

**Combined Example:**
```csharp
using System;
using EGY = Continent.Region.Area.Country.Egypt;
using static System.Math;

class Program
{
    static void Main()
    {
        // Using the alias directive
        EGY.Cairo city = new EGY.Cairo();
        city.Greet();

        // Using the static import directive
        double result = Sqrt(25); // Instead of Math.Sqrt(25)
        Console.WriteLine(result); // Outputs 5
    }
}

namespace Continent.Region.Area.Country.Egypt
{
    public class Cairo
    {
        public void Greet()
        {
            Console.WriteLine("Welcome to Cairo!");
        }
    }
}
```

In this combined example, you can see the use of the alias directive for a long namespace and the static import for mathematical functions, making the code more readable and concise. The `global using` directives ensure that these imports are available throughout the entire project, reducing redundancy.
