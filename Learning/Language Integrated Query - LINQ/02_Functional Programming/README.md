
### Functional Programming Basics

Functional programming is a paradigm that treats computation as the evaluation of mathematical functions and avoids changing state and mutable data. It emphasizes the use of pure functions, immutability, and higher-order functions.

### Key Concepts

1. **Pure Functions**:
   - Definition: Functions that always produce the same output for the same input and do not have side effects.
   - Benefits: Easier to reason about, test, and debug.

2. **Immutability**:
   - Definition: Data that cannot be changed once created.
   - Benefits: Avoids unintended side effects and makes programs easier to understand and maintain.

3. **First-Class and Higher-Order Functions**:
   - First-Class Functions: Functions are treated as first-class citizens, meaning they can be passed as arguments to other functions, returned as values from other functions, and assigned to variables.
   - Higher-Order Functions: Functions that take other functions as arguments or return them as results.

4. **Function Composition**:
   - Definition: Combining simple functions to build more complex ones.
   - Benefits: Encourages code reuse and modular design.

5. **Declarative Programming**:
   - Definition: Describing what to do, rather than how to do it.
   - Benefits: Leads to more concise and readable code.

### Functional Programming in C#

In C#, functional programming concepts can be applied using LINQ, lambda expressions, and delegates.

#### Examples

1. **Pure Function Example**:
   ```csharp
   public int Add(int x, int y)
   {
       return x + y; // Always produces the same output for the same input
   }
   ```

2. **Immutability Example**:
   ```csharp
   var numbers = new List<int> { 1, 2, 3 };
   var newNumbers = numbers.Select(n => n * 2).ToList(); // Does not modify the original list
   ```

3. **Higher-Order Function Example**:
   ```csharp
   public List<int> Filter(List<int> numbers, Func<int, bool> predicate)
   {
       return numbers.Where(predicate).ToList();
   }

   var evenNumbers = Filter(numbers, n => n % 2 == 0); // Passes a function as an argument
   ```

4. **Function Composition Example**:
   ```csharp
   Func<int, int> addOne = x => x + 1;
   Func<int, int> square = x => x * x;

   Func<int, int> addOneThenSquare = x => square(addOne(x));

   var result = addOneThenSquare(2); // 9
   ```

### Practical Application with LINQ

LINQ (Language Integrated Query) is a powerful feature in C# that brings functional programming concepts to the language, allowing for expressive and readable code for querying collections.

#### Example Using LINQ

Let's use LINQ to filter and transform a collection of `Person` objects.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalProgrammingExample
{
    public class Program
    {
        public static void Main()
        {
            List<Person> people = new List<Person>
            {
                new Person { Name = "Ali Saleh", Age = 34, Telephone = "+1(123)456-7890" },
                new Person { Name = "Rim Salem", Age = 19, Telephone = "+1(123)456-7891" },
                new Person { Name = "Ola Salam", Age = 44, Telephone = "+1(123)456-7892" },
                new Person { Name = "Huda Mohd", Age = 32, Telephone = "+1(123)456-7893" },
                new Person { Name = "Omar Kadi", Age = 28, Telephone = "+1(123)456-7894" }
            };

            // Filter people aged 30 and above
            var adults = people.Where(p => p.Age >= 30);

            // Select their names and telephone numbers
            var contactInfo = adults.Select(p => new { p.Name, p.Telephone });

            // Print the contact information
            foreach (var contact in contactInfo)
            {
                Console.WriteLine($"Name: {contact.Name}, Telephone: {contact.Telephone}");
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Telephone { get; set; }
    }
}
```

### Summary

- **Pure Functions**: Ensure no side effects and consistent output.
- **Immutability**: Prevents accidental state changes.
- **Higher-Order Functions**: Functions that operate on other functions.
- **Function Composition**: Building complex functions from simpler ones.
- **Declarative Programming**: Focuses on what to do, not how to do it.

By understanding and applying these concepts, you can write more predictable, maintainable, and expressive code, especially when using LINQ in C#.