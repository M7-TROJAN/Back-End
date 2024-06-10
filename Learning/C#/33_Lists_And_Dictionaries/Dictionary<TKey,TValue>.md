In C#, `Dictionary<TKey, TValue>` is a collection that stores key-value pairs. It provides fast lookups, adds, and removes, using a hash table. The `System.Collections.Generic` namespace contains this collection.

### Dictionary<TKey, TValue> Overview

- **Key-Value Storage**: Each item in a dictionary is a pair consisting of a key and a value.
- **Unique Keys**: Keys must be unique within a dictionary.
- **Fast Operations**: Dictionary provides efficient O(1) average time complexity for lookups, adds, and removes.

### Common Operations

#### Add
The `Add` method inserts a new key-value pair into the dictionary.

```csharp
Dictionary<int, string> dict = new Dictionary<int, string>();
dict.Add(1, "One");
dict.Add(2, "Two");
```

#### Remove
The `Remove` method removes the key-value pair with the specified key from the dictionary.

```csharp
dict.Remove(1); // Removes the key-value pair where the key is 1
```

#### TryGetValue
The `TryGetValue` method attempts to get the value associated with the specified key.

```csharp
if (dict.TryGetValue(2, out string value))
{
    Console.WriteLine(value); // Output: Two
}
```

#### ContainsKey
The `ContainsKey` method determines whether the dictionary contains the specified key.

```csharp
bool containsKey = dict.ContainsKey(2); // true
```

#### Indexer
You can use the indexer to get or set the value associated with a specific key.

```csharp
string value = dict[2]; // "Two"
dict[2] = "Two Updated"; // Updates the value associated with key 2
```

#### Keys and Values
You can retrieve the keys and values as collections.

```csharp
ICollection<int> keys = dict.Keys; // Gets a collection of all keys
ICollection<string> values = dict.Values; // Gets a collection of all values
```

### ToDictionary
The `ToDictionary` method is an extension method provided by LINQ that converts a collection to a `Dictionary<TKey, TValue>`. You must specify how to extract the keys and values from the elements in the original collection.

#### Example

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        List<string> fruits = new List<string> { "apple", "watermelon", "cherry" };

        // Convert List<string> to Dictionary<int, string>
        Dictionary<int, string> fruitDict = fruits.ToDictionary(fruit => fruit.Length, fruit => fruit);

        foreach (var kvp in fruitDict)
        {
            Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            // Output:
            // Key: 5, Value: apple
            // Key: 10, Value: WaterMelon
            // Key: 6, Value: cherry
        }
    }
}
```

### Example Usage

Here is a more comprehensive example demonstrating various operations with `Dictionary<TKey, TValue>`:

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        // Create and populate dictionary
        Dictionary<int, string> dict = new Dictionary<int, string>();
        dict.Add(1, "One");
        dict.Add(2, "Two");
        dict.Add(3, "Three");

        // Access value by key
        string value = dict[2]; // "Two"
        Console.WriteLine("Value for key 2: " + value);

        // Update value for key
        dict[2] = "Two Updated";

        // Check if a key exists
        bool containsKey = dict.ContainsKey(3); // true
        Console.WriteLine("Dictionary contains key 3: " + containsKey);

        // Remove a key-value pair
        dict.Remove(1);

        // TryGetValue
        if (dict.TryGetValue(2, out string updatedValue))
        {
            Console.WriteLine("Updated value for key 2: " + updatedValue); // "Two Updated"
        }

        // Iterate over keys and values
        foreach (var kvp in dict)
        {
            Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            // Output:
            // Key: 2, Value: Two Updated
            // Key: 3, Value: Three
        }

        // Convert list to dictionary using ToDictionary
        List<string> fruits = new List<string> { "apple", "banana", "cherry" };
        Dictionary<char, string> fruitDict = fruits.ToDictionary(fruit => fruit[0], fruit => fruit);
        foreach (var kvp in fruitDict)
        {
            Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            // Output:
            // Key: a, Value: apple
            // Key: b, Value: banana
            // Key: c, Value: cherry
        }
    }
}
```

This example demonstrates creating a dictionary, adding, accessing, updating, checking existence, removing key-value pairs, and converting a list to a dictionary.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
