---

## HashSet in C#

### Definition
A **HashSet** is a collection of unique elements that provides high-performance operations for testing the existence of elements, adding elements, and removing elements. Unlike lists, it does not maintain the order of elements, and duplicates are not allowed.

### HashSet Constructors

1. **Default Constructor**
    ```csharp
    public HashSet();
    ```
    ```csharp
    HashSet<int> set = new HashSet<int>();
    ```
    - Initializes a new instance of the `HashSet<T>` class that is empty and uses the default equality comparer for the set type.

2. **Constructor with Collection**
    ```csharp
    public HashSet(IEnumerable<T> collection);
    ```
    ```csharp
    IEnumerable<int> numbers = new List<int> { 1, 2, 3 };
    HashSet<int> set = new HashSet<int>(numbers);
    ```
    - Initializes a new instance of the `HashSet<T>` class that contains elements copied from the specified collection and uses the default equality comparer for the set type.

3. **Constructor with EqualityComparer**
    ```csharp
    public HashSet(IEqualityComparer<T> comparer);
    ```
    ```csharp
    IEqualityComparer<int> comparer = EqualityComparer<int>.Default;
    HashSet<int> set = new HashSet<int>(comparer);
    ```
    - Initializes a new instance of the `HashSet<T>` class that is empty and uses the specified equality comparer for the set type.

4. **Constructor with Collection and EqualityComparer**
    ```csharp
    public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer);
    ```
    ```csharp
    IEnumerable<int> numbers = new List<int> { 1, 2, 3 };
    IEqualityComparer<int> comparer = EqualityComparer<int>.Default;
    HashSet<int> set = new HashSet<int>(numbers, comparer);
    ```
    - Initializes a new instance of the `HashSet<T>` class that contains elements copied from the specified collection and uses the specified equality comparer for the set type.

### HashSet Methods

- **Add**
    ```csharp
    public bool Add(T item);
    ```
    ```csharp
    set.Add(4);
    ```
    - Adds the specified element to the `HashSet<T>`, and returns `true` if the element is added successfully; otherwise, `false` if the element is already present.

- **Clear**
    ```csharp
    public void Clear();
    ```
    ```csharp
    set.Clear();
    ```
    - Removes all elements from the `HashSet<T>`. This is useful when you need to reset the set to an empty state.

- **Contains**
    ```csharp
    public bool Contains(T item);
    ```
    ```csharp
    bool containsTwo = set.Contains(2); // true
    ```
    - Determines whether the `HashSet<T>` contains the specified element. This method is useful for checking the existence of an element within the set.

- **CopyTo**
    ```csharp
    public void CopyTo(T[] array);
    public void CopyTo(T[] array, int arrayIndex);
    public void CopyTo(T[] array, int arrayIndex, int count);
    ```
    ```csharp
    int[] array = new int[set.Count];
    set.CopyTo(array);
    ```
    - Copies the elements of the `HashSet<T>` to a specified array, starting at a particular array index. This is useful for converting the set to an array for easy manipulation.

- **Remove**
    ```csharp
    public bool Remove(T item);
    ```
    ```csharp
    set.Remove(2);
    ```
    - Removes the specified element from the `HashSet<T>` and returns `true` if the element is successfully found and removed; otherwise, `false`.

- **ExceptWith**
    ```csharp
    public void ExceptWith(IEnumerable<T> other);
    ```
    ```csharp
    HashSet<int> otherSet = new HashSet<int> { 3, 4 };
    set.ExceptWith(otherSet);
    ```
    - Removes all elements in the specified collection from the current `HashSet<T>`.

- **IntersectWith**
    ```csharp
    public void IntersectWith(IEnumerable<T> other);
    ```
    ```csharp
    HashSet<int> otherSet = new HashSet<int> { 2, 3, 4 };
    set.IntersectWith(otherSet);
    ```
    - Modifies the current `HashSet<T>` so that it contains only elements that are also in the specified collection.

- **UnionWith**
    ```csharp
    public void UnionWith(IEnumerable<T> other);
    ```
    ```csharp
    HashSet<int> otherSet = new HashSet<int> { 4, 5, 6 };
    set.UnionWith(otherSet);
    ```
    - Modifies the current `HashSet<T>` so that it contains all elements that are present in itself, the specified collection, or both.

- **IsSubsetOf**
    ```csharp
    public bool IsSubsetOf(IEnumerable<T> other);
    ```
    ```csharp
    bool isSubset = set.IsSubsetOf(new List<int> { 1, 2, 3, 4 });
    ```
    - Determines whether the current `HashSet<T>` is a subset of the specified collection.

- **IsSupersetOf**
    ```csharp
    public bool IsSupersetOf(IEnumerable<T> other);
    ```
    ```csharp
    bool isSuperset = set.IsSupersetOf(new List<int> { 2, 3 });
    ```
    - Determines whether the current `HashSet<T>` is a superset of the specified collection.

- **Overlaps**
    ```csharp
    public bool Overlaps(IEnumerable<T> other);
    ```
    ```csharp
    bool overlaps = set.Overlaps(new List<int> { 2, 3 });
    ```
    - Determines whether the current `HashSet<T>` overlaps with the specified collection.

- **SetEquals**
    ```csharp
    public bool SetEquals(IEnumerable<T> other);
    ```
    ```csharp
    bool equals = set.SetEquals(new List<int> { 1, 2, 3 });
    ```
    - Determines whether the current `HashSet<T>` and the specified collection contain the same elements.

### HashSet Properties

- **Count**
    ```csharp
    public int Count { get; }
    ```
    ```csharp
    int count = set.Count;
    ```
    - Gets the number of elements that are contained in the `HashSet<T>`. This property is useful for obtaining the size of the set.

- **Comparer**
    ```csharp
    public IEqualityComparer<T> Comparer { get; }
    ```
    ```csharp
    IEqualityComparer<int> comparer = set.Comparer;
    ```
    - Gets the equality comparer object that is used to determine equality for the values in the `HashSet<T>`.

### Example Usage of HashSet

Here’s an example demonstrating some of the common operations using `HashSet<T>`:

```csharp
using System;
using System.Collections.Generic;

namespace HashSetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a HashSet
            HashSet<int> set = new HashSet<int>();

            // Add elements to the HashSet
            set.Add(1);
            set.Add(2);
            set.Add(3);

            // Display the elements in the HashSet
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            // Check if the HashSet contains a specific value
            bool containsTwo = set.Contains(2); // true
            Console.WriteLine("Contains 2: " + containsTwo);

            // Remove a specific value
            set.Remove(2);

            // Display the elements in the HashSet after removal
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            // Clear the HashSet
            set.Clear();

            // Display the count of elements in the HashSet
            Console.WriteLine("Count: " + set.Count);
        }
    }
}
```

### Very Important Note
To ensure that a HashSet correctly identifies duplicates, follow these guidelines:
- For primitive types, the HashSet can automatically detect duplicates.
- For reference types, you need to override the `GetHashCode` and `Equals` methods to ensure proper comparison of objects.

---

### Real-World Scenarios

1. **Unique Usernames**
    - A HashSet can be used to maintain a collection of unique usernames, ensuring that each username is unique and providing fast lookup times.

2. **Tags or Keywords**
    - HashSet is ideal for storing tags or keywords where duplicates are not allowed, and quick existence checks are required.

3. **Configuration Settings**
    - Store configuration settings where each setting must be unique, and quick lookups are necessary.

4. **Student Enrollment**
    - Maintain a list of enrolled students where each student ID is unique, ensuring fast addition and lookup operations.

### Summary

- **HashSet**:
    - Provides fast lookups, insertions, and deletions.
    - Ensures that all elements are unique.
    - Ideal for collections where duplicates are not allowed and order does not matter.

Understanding and utilizing HashSet can greatly enhance the performance and efficiency of your applications, especially when dealing with collections of unique elements.


### HashSet vs SortedSet

| Feature                 | HashSet                              | SortedSet                            |
|-------------------------|--------------------------------------|--------------------------------------|
| **Order**               | Unordered                            | Ordered (ascending by default)       |
| **Duplicates**          | Not allowed                          | Not allowed                          |
| **Performance**         | O(1) average for add, remove, and contains | O(log n) for add, remove, and contains |
| **Underlying Structure**| Hash table                           | Binary search tree (e.g., red-black tree) |
| **Use Case**            | Fast lookups and operations where order is not important | Ordered collections where operations need to maintain order |

### Explanation

1. **Order**:
   - **HashSet**: Does not maintain any order of elements. The elements are stored in an unordered fashion.
   - **SortedSet**: Maintains elements in ascending order by default. 

2. **Duplicates**:
   - Both **HashSet** and **SortedSet** do not allow duplicate elements.

3. **Performance**:
   - **HashSet**: Provides O(1) average time complexity for add, remove, and contains operations, meaning these operations are generally performed in constant time regardless of the number of elements.
   - **SortedSet**: Provides O(log n) time complexity for add, remove, and contains operations, meaning the time required for these operations grows logarithmically with the number of elements.

4. **Underlying Structure**:
   - **HashSet**: Uses a hash table as the underlying data structure, which allows for fast access, addition, and removal of elements.
   - **SortedSet**: Uses a binary search tree (like a red-black tree) as the underlying data structure, which keeps elements sorted and allows for efficient range queries.

5. **Use Case**:
   - **HashSet**: Ideal for scenarios where fast lookups, insertions, and deletions are needed without any concern for the order of elements.
   - **SortedSet**: Ideal for scenarios where maintaining a sorted collection is necessary and order-based operations like range queries are required.

By understanding these differences, you can choose the appropriate collection type based on your specific requirements, such as performance needs and whether element order is important.

---
## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
