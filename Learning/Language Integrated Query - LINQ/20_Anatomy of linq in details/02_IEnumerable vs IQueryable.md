# IEnumerable vs IQueryable with LINQ

This code demonstrates the differences between `IEnumerable` and `IQueryable` in LINQ. It highlights how each approach handles query execution.

## Code

```csharp
private static void DemoIEnumerableIQueryable()
{
    var deck = new Deck();

    // LINQ to Objects, 
    // which mostly just does literally what it is told;
    // if you sort then pages, then it sorts then pages;
    // if you page then sort, then it pages then sorts

    var queryIEnumerable = deck.Shuffle()
        .Where(x => x.Value > 5).Skip(5).OrderBy(x => x.Value)
        .ThenByDescending(x => x.Suite).Take(5).AsEnumerable();

    // i.e LINQ to SQL
    // Query is being composed (Expression Tree)
    // When Execute Provider inspect your query tree
    // build the most suitable implementation possible

    var queryIQueryable = deck.Shuffle()
        .Where(x=> x.Value > 5).Skip(5).OrderBy(x => x.Value)
        .ThenByDescending(x=>x.Suite).Take(5).AsQueryable();
}
```

## Explanation

### IEnumerable

`IEnumerable` is used for querying in-memory collections like lists, arrays, etc. When you use LINQ methods on an `IEnumerable`, the methods are executed immediately on the collection.

1. **Query on IEnumerable**:
    ```csharp
    var queryIEnumerable = deck.Shuffle()
        .Where(x => x.Value > 5).Skip(5).OrderBy(x => x.Value)
        .ThenByDescending(x => x.Suite).Take(5).AsEnumerable();
    ```
    - **Shuffle**: Shuffles the deck.
    - **Where**: Filters cards where the value is greater than 5.
    - **Skip(5)**: Skips the first 5 cards.
    - **OrderBy(x => x.Value)**: Sorts the remaining cards by value in ascending order.
    - **ThenByDescending(x => x.Suite)**: Further sorts by suite in descending order.
    - **Take(5)**: Takes the next 5 cards.
    - **AsEnumerable**: Casts the result to `IEnumerable`.

   In `IEnumerable`, the query is executed in-memory, and the operations are performed in the order they are written.

### IQueryable

`IQueryable` is used for querying data from out-of-memory sources such as databases. When you use LINQ methods on an `IQueryable`, the query is composed but not executed until the data is enumerated (e.g., using a `foreach` loop).

1. **Query on IQueryable**:
    ```csharp
    var queryIQueryable = deck.Shuffle()
        .Where(x => x.Value > 5).Skip(5).OrderBy(x => x.Value)
        .ThenByDescending(x=>x.Suite).Take(5).AsQueryable();
    ```
    - **Shuffle**: Shuffles the deck.
    - **Where**: Filters cards where the value is greater than 5.
    - **Skip(5)**: Skips the first 5 cards.
    - **OrderBy(x => x.Value)**: Sorts the remaining cards by value in ascending order.
    - **ThenByDescending(x => x.Suite)**: Further sorts by suite in descending order.
    - **Take(5)**: Takes the next 5 cards.
    - **AsQueryable**: Casts the result to `IQueryable`.

   In `IQueryable`, the query is not executed immediately. Instead, an expression tree is built, and the query is only executed when the data is actually accessed. This allows the query provider (like Entity Framework) to optimize and translate the query into the most efficient form for the data source.

### Key Differences

- **Execution**: `IEnumerable` executes the query in-memory, while `IQueryable` builds an expression tree and executes the query on the data source when needed.
- **Optimization**: `IQueryable` allows for query optimization by the data provider, potentially leading to more efficient execution.

### Summary

This example demonstrates the differences between `IEnumerable` and `IQueryable` when working with LINQ. Understanding these differences is crucial for optimizing query performance, especially when dealing with large datasets or external data sources.