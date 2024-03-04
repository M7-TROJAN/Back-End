Extension methods in C# allow you to add new methods to existing types without modifying them, even if they are sealed or you don't have access to their source code. These methods are defined as static methods within a static class and are typically used to extend the functionality of classes without subclassing or modifying the original source code.

Here are some key points about extension methods:

1. **Defined in static classes**: Extension methods must be defined in a static class.

2. **Marked with the `this` keyword**: The first parameter of an extension method specifies the type being extended and is preceded by the `this` keyword.

3. **Called like instance methods**: Extension methods are called as if they were instance methods of the extended type.

4. **Accessible via namespace**: To use extension methods, you need to include the namespace where the static class containing the extension methods is defined.

## Simple examples of an extension method:
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

```csharp
using System;
using System.Text;
namespace ExtensionMethods
{ 
    public class Program
    {
        public static void Main()
        {
            DateTime dt = DateTime.Now;

            Console.WriteLine($"Is WeekEnd: {dt.IsWeekEnd()}");
            Console.WriteLine($"Is WeekDay: {dt.IsWeekDay()}");
        }
    }


    /// <summary>
    /// Provides extension methods for working with dates and times.
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Determines whether the specified date falls on a weekend (Friday).
        /// </summary>
        /// <param name="dt">The date to check.</param>
        /// <returns>True if the date is a Friday (weekend); otherwise, false.</returns>
        public static bool IsWeekEnd(this DateTime dt) => dt.DayOfWeek == DayOfWeek.Friday;

        /// <summary>
        /// Determines whether the specified date falls on a weekday (not Friday).
        /// </summary>
        /// <param name="dt">The date to check.</param>
        /// <returns>True if the date is not a Friday (weekday); otherwise, false.</returns>
        public static bool IsWeekDay(this DateTime dt) => !dt.IsWeekEnd();
    }


}
```
In this example, the `IsWeekEnd` and `IsWeekDay` methods extend the `DateTime` type, allowing Us to call it on any DateTime instance. The `this` keyword in the method's parameter list indicates that it is an extension method for the `DateTime` type.

## some real-world scenarios for using extension methods in C#.

### Example 1: Formatting and Manipulating Strings

```csharp
using System;

public static class StringExtensions
{
    /// <summary>
    /// Capitalizes the first letter of each word in a string.
    /// </summary>
    public static string CapitalizeWords(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
        }
        return string.Join(" ", words);
    }

    /// <summary>
    /// Capitalizes the first letter of each word in a string.
    /// </summary>
    public static string CapitalizeWordsUsingStringBuilder(this string input) // Best Performance
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new StringBuilder();
        
        foreach (string word in words)
        {
            result.Append(char.ToUpper(word[0]));
            result.Append(word.Substring(1));
            result.Append(' '); // Add space after each word
        }
        
        // Remove the trailing space
        if (result.Length > 0)
            result.Length--; 

        return result.ToString();
    }

}

class Program
{
    static void Main(string[] args)
    {
        string original = "hello world";
        string capitalized = original.CapitalizeWords();
        Console.WriteLine(capitalized); // Output: "Hello World"
    }
}
```

**Real-World Scenario:** In a web application, you might have a scenario where you need to format user input. For example, when displaying user-submitted comments or messages, you can use an extension method like `CapitalizeWords` to ensure that the text is properly capitalized for better readability.

### Example 2: Working with Collections

```csharp
using System;
using System.Collections.Generic;

public static class CollectionExtensions
{
    /// <summary>
    /// Checks if a list contains any duplicate elements.
    /// </summary>
    public static bool HasDuplicates<T>(this IEnumerable<T> source)
    {
        HashSet<T> set = new HashSet<T>();
        foreach (var item in source)
        {
            if (!set.Add(item))
                return true;
        }
        return false;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        bool hasDuplicates = numbers.HasDuplicates();
        Console.WriteLine(hasDuplicates); // Output: False
    }
}
```

**Real-World Scenario:** In an e-commerce application, when processing a customer's shopping cart, you may need to ensure that the cart does not contain duplicate items. You can use an extension method like `HasDuplicates` to check for duplicates in the list of items before proceeding with the checkout process.

These examples illustrate how extension methods can be used to enhance the functionality of existing types and provide convenience in various real-world scenarios.
