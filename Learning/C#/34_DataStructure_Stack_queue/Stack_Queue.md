**Stack** and **Queue** in detail, with comprehensive definitions, constructors, methods, properties, and examples of real-world scenarios.

## Stack

### Definition
A **stack** is a collection of elements that operates on the Last-In-First-Out (LIFO) principle. The last element added to the stack is the first one to be removed. This data structure is useful in scenarios where you need to access the most recently added element quickly.

### Stack Constructor
In C#, the `Stack<T>` class is part of the `System.Collections.Generic` namespace. Here is how you can create a stack:
```csharp
Stack<int> stack = new Stack<int>();
```

### Stack Methods

- **Push(T item)**: Adds an item to the top of the stack.
  ```csharp
  stack.Push(1);
  stack.Push(2);
  ```

- **Pop()**: Removes and returns the item at the top of the stack.
  ```csharp
  int top = stack.Pop(); // top is 2
  ```

- **Peek()**: Returns the item at the top of the stack without removing it.
  ```csharp
  int top = stack.Peek(); // top is 1
  ```

- **Contains(T item)**: Checks if the stack contains a specific item.
  ```csharp
  bool containsOne = stack.Contains(1); // true
  ```

- **Clear()**: Removes all items from the stack.
  ```csharp
  stack.Clear();
  ```

- **ToArray()**: Copies the stack to a new array.
  ```csharp
  int[] stackArray = stack.ToArray();
  ```

### Stack Properties

- **Count**: Gets the number of elements contained in the stack.
  ```csharp
  int count = stack.Count; // count is 0 after Clear()
  ```

### Real-World Scenarios

1. **Undo Mechanism in Text Editors**:
   - Each action (e.g., typing, deleting) is pushed onto the stack.
   - When "Undo" is pressed, the last action is popped from the stack and undone.

2. **Expression Evaluation**:
   - Used in evaluating postfix expressions and in function call management (call stack).

3. **Backtracking Algorithms**:
   - In solving puzzles like maze or Sudoku, you can use a stack to store your path and backtrack when you hit a dead end.

### Example: Balanced Parentheses
```csharp
public bool IsValid(string s)
{
    Stack<char> stack = new Stack<char>();
    foreach (char c in s)
    {
        if (c == '(')
        {
            stack.Push(c);
        }
        else if (c == ')')
        {
            if (stack.Count == 0 || stack.Pop() != '(')
            {
                return false;
            }
        }
    }
    return stack.Count == 0;
}
```
This algorithm ensures that every opening parenthesis has a corresponding closing parenthesis and that they are correctly nested.

This code checks if a given string `s` containing only the characters `(` and `)` has valid parentheses. The algorithm ensures that each opening parenthesis `(` is properly closed by a corresponding closing parenthesis `)` in the correct order.

## Queue

### Definition
A **queue** is a collection of elements that operates on the First-In-First-Out (FIFO) principle. The first element added to the queue is the first one to be removed. This data structure is useful in scenarios where you need to process elements in the order they were added.

### Queue Constructor
In C#, the `Queue<T>` class is part of the `System.Collections.Generic` namespace. Here is how you can create a queue:
```csharp
Queue<int> queue = new Queue<int>();
```

### Queue Methods

- **Enqueue(T item)**: Adds an item to the end of the queue.
  ```csharp
  queue.Enqueue(1);
  queue.Enqueue(2);
  ```

- **Dequeue()**: Removes and returns the item at the beginning of the queue.
  ```csharp
  int front = queue.Dequeue(); // front is 1
  ```

- **Peek()**: Returns the item at the beginning of the queue without removing it.
  ```csharp
  int front = queue.Peek(); // front is 2
  ```

- **Contains(T item)**: Checks if the queue contains a specific item.
  ```csharp
  bool containsTwo = queue.Contains(2); // true
  ```

- **Clear()**: Removes all items from the queue.
  ```csharp
  queue.Clear();
  ```

- **ToArray()**: Copies the queue to a new array.
  ```csharp
  int[] queueArray = queue.ToArray();
  ```

### Queue Properties

- **Count**: Gets the number of elements contained in the queue.
  ```csharp
  int count = queue.Count; // count is 0 after Clear()
  ```

### Real-World Scenarios

1. **Task Scheduling**:
   - Processes in an operating system or print jobs in a print queue.

2. **Breadth-First Search (BFS)**:
   - Used in graph algorithms to explore nodes level by level.

3. **Order Processing Systems**:
   - Orders are processed in the order they are received.

### Example: Level Order Traversal of a Binary Tree
```csharp
public void LevelOrderTraversal(TreeNode root)
{
    if (root == null) return;

    Queue<TreeNode> queue = new Queue<TreeNode>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        TreeNode current = queue.Dequeue();
        Console.WriteLine(current.Value);

        if (current.Left != null)
        {
            queue.Enqueue(current.Left);
        }

        if (current.Right != null)
        {
            queue.Enqueue(current.Right);
        }
    }
}
```
The provided code performs a Level Order Traversal (also known as Breadth-First Search, BFS) on a binary tree. This traversal visits nodes level by level from left to right.

### Full Example:
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace BinaryTreeTraversal
{
    public class Program
    {
        public static void Main()
        {
            // Create the binary tree
            TreeNode root = new TreeNode(1)
            {
                Left = new TreeNode(2)
                {
                    Left = new TreeNode(4),
                    Right = new TreeNode(5)
                },
                Right = new TreeNode(3)
                {
                    Right = new TreeNode(6)
                }
            };

            // Perform level order traversal
            LevelOrderTraversal(root);

            Console.ReadKey();
        }

        public static void LevelOrderTraversal(TreeNode root)
        {
            if (root == null) return;

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                TreeNode current = queue.Dequeue();
                Console.WriteLine(current.Value);

                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
                }
            }
        }
    }

    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
}
```

## Summary

### Stack vs. Queue

- **Stack**: 
  - **LIFO (Last-In-First-Out)** principle.
  - Used in scenarios like undo mechanisms, expression evaluation, and backtracking algorithms.

- **Queue**: 
  - **FIFO (First-In-First-Out)** principle.
  - Used in scenarios like task scheduling, breadth-first search, and order processing systems.

Understanding and utilizing these data structures efficiently can greatly enhance the performance and maintainability of your applications. Each has its specific use cases, and knowing when to use which can help you solve problems more effectively.
