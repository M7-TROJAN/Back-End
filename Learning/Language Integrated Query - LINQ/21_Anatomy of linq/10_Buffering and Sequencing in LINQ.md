# Buffering and Sequencing in LINQ

This code demonstrates how LINQ handles buffering and sequencing operations, such as filtering, skipping, ordering, and taking elements from a sequence. It specifically shows the order in which these operations are applied and how they affect the final result.

## Code

```csharp
private static void RunQuery()
{
    var deck = new Deck();

    var cards = deck.GetSample();

    var query = cards      // { Jack Clubs, 9 Diamonds, 4 Hearts, 10 Spades, 3 Hearts, 6 Hearts }
        .Where(x => x.IsRed)   // {             9 Diamonds, 4 Hearts,            3 Hearts, 6 Hearts }
        .Skip(1)               // {                       , 4 Hearts,            3 Hearts, 6 Hearts }
        .OrderBy(x => x.Value) // {                       , 3 Hearts,            4 Hearts, 6 Hearts }
        .Take(2)
        .ToList(); // { 3 Hearts, 4 Hearts }

    query.PrintDeck("Order Buffer Sequence, when it's enumerated");
}
```

## Explanation

### LINQ Operations Breakdown

1. **Deck and Cards Initialization**:
    ```csharp
    var deck = new Deck();
    var cards = deck.GetSample();
    ```
    - A new deck of cards is created, and a sample of cards is obtained.

2. **LINQ Query**:
    ```csharp
    var query = cards      // { Jack Clubs, 9 Diamonds, 4 Hearts, 10 Spades, 3 Hearts, 6 Hearts }
        .Where(x => x.IsRed)   // {             9 Diamonds, 4 Hearts,            3 Hearts, 6 Hearts }
        .Skip(1)               // {                       , 4 Hearts,            3 Hearts, 6 Hearts }
        .OrderBy(x => x.Value) // {                       , 3 Hearts,            4 Hearts, 6 Hearts }
        .Take(2)
        .ToList(); // { 3 Hearts, 4 Hearts }
    ```
    - **Where**: Filters the cards to include only red cards. The resulting sequence is `{ 9 Diamonds, 4 Hearts, 3 Hearts, 6 Hearts }`.
    - **Skip(1)**: Skips the first card in the filtered sequence. The resulting sequence is `{ 4 Hearts, 3 Hearts, 6 Hearts }`.
    - **OrderBy**: Orders the remaining cards by their value. The resulting sequence is `{ 3 Hearts, 4 Hearts, 6 Hearts }`.
    - **Take(2)**: Takes the first 2 cards from the ordered sequence. The resulting sequence is `{ 3 Hearts, 4 Hearts }`.

3. **Execution**:
    ```csharp
    query.PrintDeck("Order Buffer Sequence, when it's enumerated");
    ```
    - Converts the query result to a list, triggering the execution of the query.
    - Prints the resulting list of cards with the title "Order Buffer Sequence, when it's enumerated".

### Key Points

- **Order of Operations**: The order in which LINQ methods are applied affects the final result. In this example, filtering is done first, followed by skipping, ordering, and then taking elements.
- **Deferred Execution**: LINQ queries are not executed until they are enumerated (in this case, by calling `ToList()`). This allows for efficient query composition.
- **Buffering**: The `OrderBy` method requires buffering all elements in memory to sort them before yielding any results. This can impact performance for large sequences.

### Execution and Output

The `PrintDeck` method prints the result of the query. The output will show the cards `{ 3 Hearts, 4 Hearts }`, as these are the first two red cards in the sequence after skipping the first red card and ordering the remaining red cards by their value.

### Summary

This example illustrates how the order of LINQ operations such as `Where`, `Skip`, `OrderBy`, and `Take` can change the result of a query. By understanding the impact of these operations and their execution order, you can compose more efficient and accurate LINQ queries.
