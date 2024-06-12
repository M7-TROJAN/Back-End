The `IComparer<T>` interface in C# is used to define a custom comparison for sorting or ordering objects. This interface provides a method for comparing two objects of the same type, allowing you to specify the criteria by which they should be ordered. This is particularly useful when you need to sort objects in a way that is not the default.

### Definition
The `IComparer<T>` interface is defined in the `System.Collections.Generic` namespace and has a single method:

```csharp
public interface IComparer<in T>
{
    int Compare(T x, T y);
}
```

### Method

- **Compare**
    ```csharp
    int Compare(T x, T y);
    ```
    - Compares two objects and returns an integer that indicates their relative position in the sort order:
        - Less than zero: `x` is less than `y`.
        - Zero: `x` is equal to `y`.
        - Greater than zero: `x` is greater than `y`.

### Example Usage

Here's an example demonstrating how to implement and use the `IComparer<T>` interface to sort a collection of custom objects:

```csharp
using System;
using System.Collections.Generic;

// Custom class
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"{Name}, Age: {Age}";
    }
}

// Custom comparer for the Person class
public class PersonAgeComparer : IComparer<Person>
{
    public int Compare(Person x, Person y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Arguments cannot be null");
        }

        return x.Age.CompareTo(y.Age);
    }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>
        {
            new Person("Alice", 30),
            new Person("Bob", 25),
            new Person("Charlie", 35)
        };

        // Sort using the custom comparer
        people.Sort(new PersonAgeComparer());

        foreach (var person in people)
        {
            Console.WriteLine(person);
        }
    }
}
```

### Explanation

1. **Custom Class `Person`**: This class has two properties, `Name` and `Age`, and a constructor to initialize these properties.

2. **Custom Comparer `PersonAgeComparer`**: This class implements `IComparer<Person>` and provides the logic for comparing two `Person` objects based on their `Age` property.

3. **Usage in `Program`**:
    - A list of `Person` objects is created and initialized.
    - The `Sort` method of the list is called with an instance of `PersonAgeComparer`, which sorts the list based on the age of the persons.
    - The sorted list is then printed to the console.

By implementing `IComparer<T>`, you can customize the sorting behavior for any type of objects according to your specific requirements. This is especially useful when dealing with collections of complex objects where the default comparison (e.g., lexicographical for strings, numerical for numbers) is not suitable.
