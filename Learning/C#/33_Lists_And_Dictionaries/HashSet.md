A `HashSet` in C# is a collection type that stores unique elements. It is part of the `System.Collections.Generic` namespace. Here are some key characteristics of a `HashSet`:

1. **Uniqueness**: A `HashSet` does not allow duplicate elements. If you try to add an element that already exists in the set, it will be ignored.

2. **Performance**: Operations such as adding, removing, and checking for the existence of elements in a `HashSet` have near-constant time complexity on average (O(1)).

3. **No Guaranteed Ordering**: Elements in a `HashSet` are not stored in any particular order. If you need to maintain the order of elements, you should consider using other collection types like `List` or `LinkedList`.

4. **Uses Hashing**: Internally, a `HashSet` uses a hash table to store elements, which allows for efficient lookup and manipulation of elements.

Here's a simple example demonstrating the usage of a `HashSet`:

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Create a new HashSet of integers
        HashSet<int> numbers = new HashSet<int>();

        // Add elements to the HashSet
        numbers.Add(10);
        numbers.Add(20);
        numbers.Add(30);

        // Trying to add a duplicate element
        numbers.Add(10); // This will be ignored and the Add method will return false

        // Check if an element exists in the HashSet
        bool containsTwenty = numbers.Contains(20);
        Console.WriteLine("Contains 20: " + containsTwenty); // Output: True

        // Remove an element from the HashSet
        numbers.Remove(30);

        // Iterate over the elements of the HashSet
        foreach (int number in numbers)
        {
            Console.WriteLine(number);
        }
    }
}
```

In this example, we create a `HashSet<int>` called `numbers` and add some integers to it. We attempt to add a duplicate element (`10`), which is ignored because `HashSet` does not allow duplicates. We then check if the set contains the number `20`, remove `30` from the set, and finally iterate over the elements using a foreach loop.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
