## SortedSet in C#

### Definition
A **SortedSet** is a collection of unique elements that are automatically sorted in ascending order. It provides efficient operations for testing the existence of elements, adding elements, and removing elements while maintaining the sorted order.

### SortedSet Constructors

1. **Default Constructor**
    ```csharp
    public SortedSet();
    ```
    ```csharp
    SortedSet<int> set = new SortedSet<int>();
    ```
    - Initializes a new instance of the `SortedSet<T>` class that is empty and uses the default comparer for the set type.

2. **Constructor with Collection**
    ```csharp
    public SortedSet(IEnumerable<T> collection);
    ```
    ```csharp
    IEnumerable<int> numbers = new List<int> { 1, 2, 3 };
    SortedSet<int> set = new SortedSet<int>(numbers);
    ```
    - Initializes a new instance of the `SortedSet<T>` class that contains elements copied from the specified collection and uses the default comparer for the set type.

3. **Constructor with Comparer**
    ```csharp
    public SortedSet(IComparer<T> comparer);
    ```
    ```csharp
    IComparer<int> comparer = Comparer<int>.Default;
    SortedSet<int> set = new SortedSet<int>(comparer);
    ```
    - Initializes a new instance of the `SortedSet<T>` class that is empty and uses the specified comparer to order the elements.

4. **Constructor with Collection and Comparer**
    ```csharp
    public SortedSet(IEnumerable<T> collection, IComparer<T> comparer);
    ```
    ```csharp
    IEnumerable<int> numbers = new List<int> { 1, 2, 3 };
    IComparer<int> comparer = Comparer<int>.Default;
    SortedSet<int> set = new SortedSet<int>(numbers, comparer);
    ```
    - Initializes a new instance of the `SortedSet<T>` class that contains elements copied from the specified collection and uses the specified comparer to order the elements.

### SortedSet Methods

- **Add**
    ```csharp
    public bool Add(T item);
    ```
    ```csharp
    set.Add(4);
    ```
    - Adds the specified element to the `SortedSet<T>`, and returns `true` if the element is added successfully; otherwise, `false` if the element is already present.

- **Clear**
    ```csharp
    public void Clear();
    ```
    ```csharp
    set.Clear();
    ```
    - Removes all elements from the `SortedSet<T>`. This is useful when you need to reset the set to an empty state.

- **Contains**
    ```csharp
    public bool Contains(T item);
    ```
    ```csharp
    bool containsTwo = set.Contains(2); // true
    ```
    - Determines whether the `SortedSet<T>` contains the specified element. This method is useful for checking the existence of an element within the set.

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
    - Copies the elements of the `SortedSet<T>` to a specified array, starting at a particular array index. This is useful for converting the set to an array for easy manipulation.

- **Remove**
    ```csharp
    public bool Remove(T item);
    ```
    ```csharp
    set.Remove(2);
    ```
    - Removes the specified element from the `SortedSet<T>` and returns `true` if the element is successfully found and removed; otherwise, `false`.

- **ExceptWith**
    ```csharp
    public void ExceptWith(IEnumerable<T> other);
    ```
    ```csharp
    SortedSet<int> otherSet = new SortedSet<int> { 3, 4 };
    set.ExceptWith(otherSet);
    ```
    - Removes all elements in the specified collection from the current `SortedSet<T>`.

- **IntersectWith**
    ```csharp
    public void IntersectWith(IEnumerable<T> other);
    ```
    ```csharp
    SortedSet<int> otherSet = new SortedSet<int> { 2, 3, 4 };
    set.IntersectWith(otherSet);
    ```
    - Modifies the current `SortedSet<T>` so that it contains only elements that are also in the specified collection.

- **UnionWith**
    ```csharp
    public void UnionWith(IEnumerable<T> other);
    ```
    ```csharp
    SortedSet<int> otherSet = new SortedSet<int> { 4, 5, 6 };
    set.UnionWith(otherSet);
    ```
    - Modifies the current `SortedSet<T>` so that it contains all elements that are present in itself, the specified collection, or both.

- **IsSubsetOf**
    ```csharp
    public bool IsSubsetOf(IEnumerable<T> other);
    ```
    ```csharp
    bool isSubset = set.IsSubsetOf(new List<int> { 1, 2, 3, 4 });
    ```
    - Determines whether the current `SortedSet<T>` is a subset of the specified collection.

- **IsSupersetOf**
    ```csharp
    public bool IsSupersetOf(IEnumerable<T> other);
    ```
    ```csharp
    bool isSuperset = set.IsSupersetOf(new List<int> { 2, 3 });
    ```
    - Determines whether the current `SortedSet<T>` is a superset of the specified collection.

- **Overlaps**
    ```csharp
    public bool Overlaps(IEnumerable<T> other);
    ```
    ```csharp
    bool overlaps = set.Overlaps(new List<int> { 2, 3 });
    ```
    - Determines whether the current `SortedSet<T>` overlaps with the specified collection.

- **SetEquals**
    ```csharp
    public bool SetEquals(IEnumerable<T> other);
    ```
    ```csharp
    bool equals = set.SetEquals(new List<int> { 1, 2, 3 });
    ```
    - Determines whether the current `SortedSet<T>` and the specified collection contain the same elements.

### SortedSet Properties

- **Count**
    ```csharp
    public int Count { get; }
    ```
    ```csharp
    int count = set.Count;
    ```
    - Gets the number of elements that are contained in the `SortedSet<T>`. This property is useful for obtaining the size of the set.

- **Comparer**
    ```csharp
    public IComparer<T> Comparer { get; }
    ```
    ```csharp
    IComparer<int> comparer = set.Comparer;
    ```
    - Gets the comparer object that is used to order the elements in the `SortedSet<T>`.

- **Min**
    ```csharp
    public T Min { get; }
    ```
    ```csharp
    int min = set.Min;
    ```
    - Gets the minimum element in the `SortedSet<T>` according to the comparer.

- **Max**
    ```csharp
    public T Max { get; }
    ```
    ```csharp
    int max = set.Max;
    ```
    - Gets the maximum element in the `SortedSet<T>` according to the comparer.

### Example Usage of SortedSet

Here’s an example demonstrating some of the common operations using `SortedSet<T>`:

```csharp
using System;
using System.Collections.Generic;

namespace SortedSetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a SortedSet
            SortedSet<int> set = new SortedSet<int>();

            // Add elements to the SortedSet
            set.Add(3);
            set.Add(1);
            set.Add(2);

            // Display the elements in the SortedSet
            foreach (var item in set)
            {
                Console.WriteLine(item); // Output will be sorted: 1, 2, 3
            }

            // Check if the SortedSet contains a specific value
            bool containsTwo = set.Contains(2); // true
            Console.WriteLine("Contains 2: " + containsTwo);

            // Remove a specific value
            set.Remove(2);

            // Display the elements in the SortedSet after removal
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            // Clear the SortedSet
            set.Clear();

            // Display the count of elements in the SortedSet
            Console.WriteLine("Count: " + set.Count);
        }
    }
}
```

### Real-World Scenarios

1. **Leaderboard**
    - A SortedSet can be used to maintain a sorted leaderboard where scores are kept in order.

2. **Priority Scheduling**
    - For scheduling tasks based on priority, a SortedSet ensures tasks are executed in the correct order.

3. **Unique Ordered Data**
    - Storing unique ordered data such as timestamps or unique identifiers in ascending order.

4. **Range Queries**
    - Performing range queries where elements within a specific range are needed, leveraging the sorted nature of the set.

### HashSet vs SortedSet

| Feature                 | HashSet                              | SortedSet                            |
|-------------------------|--------------------------------------|--------------------------------------|
| **Order**               | Unordered                            | Ordered (ascending by default)       |
| **Duplicates**          | Not allowed                          | Not allowed                          |
| **Performance**         | O
