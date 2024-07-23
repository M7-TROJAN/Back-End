
# Filter and Order in LINQ

This code demonstrates the impact of the order in which filtering and ordering operations are applied in LINQ queries. It highlights the differences between filtering first and then ordering, versus ordering first and then filtering.

## Code

```csharp
private static void DemoFilterOrder()
{
    // Filter / Order (Top 10 in the Red Cards)

    var deck = new Deck();

    var cards = deck.Shuffle();

    var query1 = cards      
    .Where(x => x.IsRed)       
    .OrderBy(x => x.Value)    
    .Take(10);
    
    query1.PrintDeck("Top 10 Red Cards");

    // Order / Filter (Red Cards in the Top 10)

    var query2 = cards
    .OrderBy(x => x.Value)
    .Take(10)
    .Where(x => x.IsRed);
    
    query2.PrintDeck("Red Cards in the Top 10");
}
```

## Explanation

### Filter and Order (Top 10 Red Cards)

In this part of the code, the query filters the red cards first and then orders them by their value before taking the top 10 cards.

```csharp
var query1 = cards      
    .Where(x => x.IsRed)       
    .OrderBy(x => x.Value)    
    .Take(10);
    
query1.PrintDeck("Top 10 Red Cards");
```
- **Where**: Filters the cards to include only red cards.
- **OrderBy**: Orders the filtered red cards by their value.
- **Take(10)**: Takes the top 10 cards from the ordered sequence.

### Order and Filter (Red Cards in the Top 10)

In this part of the code, the query orders all the cards by their value first, then takes the top 10 cards, and finally filters these 10 cards to include only the red cards.

```csharp
var query2 = cards
    .OrderBy(x => x.Value)
    .Take(10)
    .Where(x => x.IsRed);
    
query2.PrintDeck("Red Cards in the Top 10");
```
- **OrderBy**: Orders all the cards by their value.
- **Take(10)**: Takes the top 10 cards from the ordered sequence.
- **Where**: Filters these 10 cards to include only the red cards.

### Key Points

- **Query Composition**: The order of operations in a LINQ query can significantly change the result. 
    - In `query1`, filtering happens first, then ordering, which ensures that only red cards are considered for the top 10.
    - In `query2`, ordering happens first, then the top 10 cards are selected, and finally, only the red cards from these 10 are considered.
- **Deferred Execution**: LINQ queries are not executed until they are enumerated. This allows for building complex queries by chaining methods.

### Execution and Output

The `PrintDeck` method prints the result of each query. The output will be different for each query due to the different order of operations.

- **Top 10 Red Cards**: This output will show the top 10 red cards based on their value.
- **Red Cards in the Top 10**: This output will show the red cards that are in the top 10 cards when all cards are ordered by their value.

### Summary

This example illustrates the importance of the order in which LINQ methods are applied in a query. By understanding the impact of different query compositions, you can control the behavior and results of your LINQ queries more effectively.
