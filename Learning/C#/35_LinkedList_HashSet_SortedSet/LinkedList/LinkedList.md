## LinkedList in C#

### Definition
A **LinkedList** is a collection of elements, called nodes, where each node contains a reference (link) to the next node in the sequence. Unlike arrays, linked lists do not store elements in contiguous memory locations, allowing for efficient insertions and deletions.

### LinkedList Constructors

1. **Default Constructor**
    ```csharp
   public LinkedList();
   ```
   ```csharp
   LinkedList<int> list = new LinkedList<int>();
   ```
   - Initializes a new instance of the `LinkedList<T>` class that is empty.

2. **Constructor with Collection**
    ```csharp
   public LinkedList(IEnumerable<T> collection);
   ```
   ```csharp
   IEnumerable<int> numbers = new List<int> { 1, 2, 3 };
   LinkedList<int> list = new LinkedList<int>(numbers);
   ```
   - Initializes a new instance of the `LinkedList<T>` class that contains elements copied from the specified collection.

### LinkedList Methods

- **AddAfter**
  ```csharp
  public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value);
  public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);
  ```
  ```csharp
  LinkedListNode<int> node = list.Find(2);
  list.AddAfter(node, 3);
  ```
  - Adds a new node containing the specified value after the specified existing node in the `LinkedList<T>`.

- **AddBefore**
  ```csharp
  public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);
  public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);
  ```
  ```csharp
  LinkedListNode<int> node = list.Find(2);
  list.AddBefore(node, 1);
  ```
  - Adds a new node containing the specified value before the specified existing node in the `LinkedList<T>`.

- **AddFirst**
  ```csharp
  public LinkedListNode<T> AddFirst(T value);
  public void AddFirst(LinkedListNode<T> node);
  ```
  ```csharp
  list.AddFirst(0);
  ```
  - Adds a new node containing the specified value at the start of the `LinkedList<T>`.

- **AddLast**
  ```csharp
  public LinkedListNode<T> AddLast(T value);
  public void AddLast(LinkedListNode<T> node);
  ```
  ```csharp
  list.AddLast(4);
  ```
  - Adds a new node containing the specified value at the end of the `LinkedList<T>`.

- **Clear**
  ```csharp
  list.Clear();
  ```
  - Removes all nodes from the `LinkedList<T>`.

- **Contains**
  ```csharp
  bool containsTwo = list.Contains(2); // true
  ```
  - Determines whether the `LinkedList<T>` contains a specific value.

- **CopyTo**
```csharp
  public void CopyTo(T[] array, int index);
  ```
  ```csharp
  int[] array = new int[list.Count];
  list.CopyTo(array, 0);
  ```
  - Copies the entire `LinkedList<T>` to a compatible one-dimensional array, starting at the specified array index.

- **Find**
  ```csharp
  public LinkedListNode<T> Find(T value);
  ```
  ```csharp
  LinkedListNode<int> node = list.Find(2);
  ```
  - Finds the first node that contains the specified value.

- **FindLast**
  ```csharp
  public LinkedListNode<T> FindLast(T value);
  ```
  ```csharp
  LinkedListNode<int> lastNode = list.FindLast(2);
  ```
  - Finds the last node that contains the specified value.

- **Remove (Node)**
  ```csharp
  public void Remove(LinkedListNode<T> node);
  ```
  ```csharp
  LinkedListNode<int> node = list.Find(2);
  list.Remove(node);
  ```
  - Removes the specified node from the `LinkedList<T>`.

- **Remove (Value)**
  ```csharp
  public bool Remove(T value);
  ```
  ```csharp
  list.Remove(2);
  ```
  - Removes the first occurrence of the specified value from the `LinkedList<T>`.

- **RemoveFirst**
```csharp
  public void RemoveFirst();
  ```
  ```csharp
  list.RemoveFirst();
  ```
  - Removes the node at the start of the `LinkedList<T>`.

- **RemoveLast**
```csharp
  public void RemoveLast();
  ```
  ```csharp
  list.RemoveLast();
  ```
  - Removes the node at the end of the `LinkedList<T>`.

### LinkedList Properties

- **Count**
  ```csharp
  public int Count { get; }
  ```
  ```csharp
  int count = list.Count;
  ```
  - Gets the number of nodes actually contained in the `LinkedList<T>`.

### Example Usage of LinkedList

Here’s an example demonstrating some of the common operations using `LinkedList<T>`:

```csharp
using System;
using System.Collections.Generic;

namespace LinkedListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a LinkedList
            LinkedList<int> list = new LinkedList<int>();

            // Add elements to the LinkedList
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            // Add elements to the start of the LinkedList
            list.AddFirst(0);

            // Add elements after a specified node
            LinkedListNode<int> node = list.Find(2);
            list.AddAfter(node, 3);

            // Add elements before a specified node
            list.AddBefore(node, 1);

            // Display the elements in the LinkedList
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            // Check if the LinkedList contains a specific value
            bool containsTwo = list.Contains(2); // true
            Console.WriteLine("Contains 2: " + containsTwo);

            // Find a node with a specific value
            LinkedListNode<int> foundNode = list.Find(3);
            if (foundNode != null)
            {
                Console.WriteLine("Found node with value: " + foundNode.Value);
            }

            // Remove a node with a specific value
            list.Remove(1);

            // Remove the first node
            list.RemoveFirst();

            // Remove the last node
            list.RemoveLast();

            // Clear the LinkedList
            list.Clear();

            // Display the count of elements in the LinkedList
            Console.WriteLine("Count: " + list.Count);
        }
    }
}
```

### Real-World Scenarios

1. **Browser History**
   - A LinkedList can be used to maintain the browsing history, where each page visited is a node.

2. **Undo Functionality in Text Editors**
   - Operations can be stored as nodes in a LinkedList. Each undo or redo operation traverses through the list.

3. **Playlist in Media Players**
   - A playlist can be maintained using a LinkedList, allowing easy insertion and deletion of tracks.

4. **Chain of Responsibility Pattern**
   - In design patterns, a LinkedList can be used to implement the Chain of Responsibility pattern, where each node represents a handler.

### Summary

- **LinkedList**:
  - Allows for efficient insertions and deletions.
  - Useful when frequent additions and removals of elements are required.
  - Operations like AddAfter, AddBefore, AddFirst, and AddLast make it flexible for various use cases.

Understanding and utilizing LinkedList can greatly enhance the performance and flexibility of your applications where dynamic data structures are required. Each method and property provides specific functionalities to manipulate the list effectively.