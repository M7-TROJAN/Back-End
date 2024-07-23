# Fluent API with LINQ

This code demonstrates the use of Fluent API with LINQ in C#. Fluent API allows chaining of methods in a way that makes the code read like a sentence.
(بتقرأ الكود من الشمال لليمين كأنك بتقرأ جملة انجليزية مفهومة)

## Code

```csharp
private static void DemoFluentAPI()
{
    // ### Fluent API ###
    // 1. Method Chaining and Extension method to make statement look like a sentence.
    // 2. is code that reads as a sentence.
         
    var deck = new Deck();
    var cards = deck.Shuffle();

    var query = cards
        .OrderBy(x => x.Value).Skip(10).Take(10)
        .OrderBy(x => x.Suite).ToList();

    foreach (var item in query)
        Console.WriteLine(item.Name);
}
```

## Explanation

### Method Chaining

Method chaining is a technique where multiple methods are called on the same object, one after the other, in a single statement. This is made possible by extension methods in C#. In this example, we are using LINQ methods to chain multiple operations on a collection of cards.

### Code Breakdown

1. **Deck Initialization**:
    ```csharp
    var deck = new Deck();
    var cards = deck.Shuffle();
    ```
    - A `Deck` object is instantiated.
    - The `Shuffle` method is called on the deck, presumably returning a shuffled list of cards.

2. **LINQ Query**:
    ```csharp
    var query = cards
        .OrderBy(x => x.Value).Skip(10).Take(10)
        .OrderBy(x => x.Suite).ToList();
    ```
    - `OrderBy(x => x.Value)`: Sorts the cards by their `Value` property.
    - `Skip(10)`: Skips the first 10 cards in the sorted list.
    - `Take(10)`: Takes the next 10 cards from the remaining list.
    - `OrderBy(x => x.Suite)`: Sorts the resulting 10 cards by their `Suite` property.
    - `ToList()`: Converts the final sorted sequence to a list.

3. **Output**:
    ```csharp
    foreach (var item in query)
        Console.WriteLine(item.Name);
    ```
    - Iterates over the final list of cards.
    - Prints the `Name` property of each card to the console.

### LINQ Methods Used

- **OrderBy**: Sorts the elements of a sequence in ascending order according to a key.
- **Skip**: Bypasses a specified number of elements in a sequence and returns the remaining elements.
- **Take**: Returns a specified number of contiguous elements from the start of a sequence.
- **ToList**: Converts an `IEnumerable` to a `List`.

### Fluent API

The Fluent API style makes the code more readable and expressive. By chaining these methods together, we achieve a clear and concise sequence of operations that resemble a natural language sentence.

### Summary

This example demonstrates how to use Fluent API with LINQ to perform complex queries on a collection of objects in a readable and maintainable manner. The method chaining approach allows for easy-to-understand code that efficiently processes data through multiple transformations.
```
