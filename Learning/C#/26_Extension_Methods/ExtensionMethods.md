Extension methods in C# allow you to add new methods to existing types without modifying them, even if they are sealed or you don't have access to their source code. These methods are defined as static methods within a static class and are typically used to extend the functionality of classes without subclassing or modifying the original source code.

Here are some key points about extension methods:

1. **Defined in static classes**: Extension methods must be defined in a static class.

2. **Marked with the `this` keyword**: The first parameter of an extension method specifies the type being extended and is preceded by the `this` keyword.

3. **Called like instance methods**: Extension methods are called as if they were instance methods of the extended type.

4. **Accessible via namespace**: To use extension methods, you need to include the namespace where the static class containing the extension methods is defined.

a simple example of an extension method:
```csharp
using System;

namespace ExtensionMethodsExample
{
    // Define a static class containing extension methods
    public static class StringExtensions
    {
        // Define an extension method for strings to reverse their content
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string original = "Hello";
            string reversed = original.Reverse(); // Calling the extension method
            Console.WriteLine(reversed); // Output: "olleH"
        }
    }
}
```

In this example, the `Reverse` method extends the `string` type, allowing Us to call it on any string instance. The `this` keyword in the method's parameter list indicates that it is an extension method for the `string` type.


```csharp
using System;
namespace ExtensionMethods
{ 
    public class Program
    {
        public static void Main()
        {
            int percentage = 70;

            if (percentage.IsBetween(60, 100))
                Console.WriteLine("Pass");
            else
                Console.WriteLine("Fail");
        }
    }

    public static class IntExtensions
    {
        /// <summary>
        /// Checks if the value is between the specified <paramref name="minimum"/>  and <paramref name="maximum"/> values, inclusive.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="minimum">The minimum value (inclusive).</param>
        /// <param name="maximum">The maximum value (inclusive).</param>
        /// <returns>True if the value is between the specified range; otherwise, false.</returns>
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
```
In this example, the `IsBetween` method extends the `int` type, allowing Us to call it on any int instance. The `this` keyword in the method's parameter list indicates that it is an extension method for the `int` type.
