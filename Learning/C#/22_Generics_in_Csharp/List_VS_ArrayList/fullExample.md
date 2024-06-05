# C# Generics, Predicates, and Non-Generic Collections Example

This example showcases how to use generics, predicates, and handle non-generic collections in C#. It includes a generic `Print` method that filters and prints elements from various collections based on a provided predicate.

## Code Example

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Learning
{
    class Program
    {
        static void Main()
        {
            var list = new List<int> 
            { 
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20 
            };

            Predicate<int> evenPredicate = x => x % 2 == 0;

            Console.WriteLine("Even numbers in the list:");
            Print(list, evenPredicate);

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.WriteLine("\nOdd numbers in the array:");
            Print(array, x => x % 2 != 0);

            ArrayList arrayList = new ArrayList 
            { 
                1, "Mahmoud", false, new Person("Mahmoud", 25) 
            };

            Console.WriteLine("\nIntegers in the ArrayList:");
            Print(arrayList.Cast<object>(), x => x is int);
        }

        public static void Print<T>(IEnumerable<T> items, Predicate<T> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    Console.Write(item + ", ");
                }
            }
            Console.WriteLine();
        }
    }

    class Person
    {
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }
}
```

## Explanation

### Main Program

1. **List of Integers**:
   - A list of integers from 1 to 20 is created.
   - A predicate is defined to filter even numbers.
   - The `Print` method is called to print even numbers from the list.

2. **Array of Integers**:
   - An array of integers from 1 to 10 is created.
   - The `Print` method is called with a predicate to filter odd numbers.

3. **ArrayList**:
   - An `ArrayList` containing different types of elements is created.
   - The `Print` method is called with a predicate to filter integers.

### Generic Print Method

- The `Print` method takes an `IEnumerable<T>` collection and a `Predicate<T>`.
- It iterates over the collection and prints elements that satisfy the predicate.

### Person Class

- A simple `Person` class with `Name` and `Age` properties.
- Overrides the `ToString` method to provide a custom string representation.

## Output

```
Even numbers in the list:
2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 

Odd numbers in the array:
1, 3, 5, 7, 9, 

Integers in the ArrayList:
1, 
```

This example demonstrates the flexibility of using generics and predicates in C# to handle various types of collections and apply custom filtering logic. It also shows how to work with non-generic collections like `ArrayList` using casting.
