In C#, `List<T>` is a generic collection provided by the `System.Collections.Generic` namespace. It is a dynamic array that provides a range of functionalities to manage a collection of objects. Here is an overview of `List<T>` including the operations like `Add`, `Insert`, `Remove`, as well as `Count` and `Capacity`.

### List<T> Overview

- `List<T>` is a strongly-typed list of objects.
- It automatically resizes itself when elements are added or removed.
- Provides methods to manipulate the collection of objects efficiently.

### Common Operations

#### Add
The `Add` method is used to add an object to the end of the list.

```csharp
List<int> numbers = new List<int>();
numbers.Add(1);
numbers.Add(2);
numbers.Add(3);
```

#### Insert
The `Insert` method is used to insert an object at a specified index in the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.Insert(1, 4); // Insert 4 at index 1
// Resulting list: { 1, 4, 2, 3 }
```

#### Remove
The `Remove` method is used to remove the first occurrence of a specific object from the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 2 };
numbers.Remove(2); // Remove the first occurrence of 2
// Resulting list: { 1, 3, 2 }
```

#### RemoveAt
The `RemoveAt` method is used to remove an object at a specified index in the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.RemoveAt(1); // Remove the element at index 1
// Resulting list: { 1, 3 }
```

#### Count
The `Count` property gets the number of elements contained in the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
int count = numbers.Count; // count = 3
```

#### Capacity
The `Capacity` property gets or sets the number of elements that the `List<T>` can store before resizing is required.

```csharp
List<int> numbers = new List<int>(10); // Initial capacity set to 10
int capacity = numbers.Capacity; // capacity = 10
numbers.Add(1);
numbers.Add(2);
numbers.TrimExcess(); // Reduces the capacity to match the count
capacity = numbers.Capacity; // capacity = 2
```

### Other Useful Methods

#### Contains
Determines whether an element is in the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
bool containsTwo = numbers.Contains(2); // containsTwo = true
```

#### IndexOf
Returns the zero-based index of the first occurrence of a specific object in the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 2 };
int index = numbers.IndexOf(2); // index = 1
```

#### Clear
Removes all elements from the list.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.Clear();
// Resulting list: { }
```

#### Sort
Sorts the elements in the list.

```csharp
List<int> numbers = new List<int> { 3, 1, 2 };
numbers.Sort();
// Resulting list: { 1, 2, 3 }
```

#### Reverse
Reverses the order of the elements in the entire list or a portion of it.

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.Reverse();
// Resulting list: { 3, 2, 1 }
```

### Example Usage

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        List<int> numbers = new List<int>();

        // Add elements
        numbers.Add(1);
        numbers.Add(2);
        numbers.Add(3);

        // Insert element at index 1
        numbers.Insert(1, 4);

        // Remove element with value 2
        numbers.Remove(2);

        // Remove element at index 1
        numbers.RemoveAt(1);

        // Get count and capacity
        int count = numbers.Count;
        int capacity = numbers.Capacity;

        // Output list details
        Console.WriteLine("Count: " + count); // Output: Count: 2
        Console.WriteLine("Capacity: " + capacity); // Output: Capacity: 4 (initial capacity might differ)
        
        // List contents
        foreach (var number in numbers)
        {
            Console.WriteLine(number); // Output: 1, 3
        }
    }
}
```

This example demonstrates adding, inserting, and removing elements from a list, as well as getting the count and capacity of the list. The output will vary depending on the initial capacity and how the list resizes itself.
