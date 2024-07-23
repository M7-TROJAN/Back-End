# Deferred Execution in LINQ

This code demonstrates the concept of deferred execution in LINQ. Deferred execution means that the query is not executed when it is constructed but only when it is enumerated.

## Code

```csharp
private static void DemoDeferredExecution()
{
    // Not executed when constructed, only when it's enumerated
    // Setting up a data structure that describes the query

    // queries are always up-to-date.
    // queries is more expensive than list to retrieve result
    // queries are tiny
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };
    var query = numbers
        .Where(x => x > 5)                       // 8, 6, 7, 9  
        .Select(x => x * x)
        .Take(2);                                // 64, 36 
     
    foreach (var n in query)
        Console.WriteLine(n); 
}
```

## Explanation

### Deferred Execution

Deferred execution means that the LINQ query is not executed at the point of its declaration. Instead, it is executed only when the query is enumerated, such as during a `foreach` loop, or when calling methods like `ToList`, `ToArray`, etc.

### Code Breakdown

1. **Initial Array**:
    ```csharp
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };
    ```
    - An array of integers is defined.

2. **LINQ Query with Deferred Execution**:
    ```csharp
    var query = numbers
        .Where(x => x > 5)                       // 8, 6, 7, 9  
        .Select(x => x * x)
        .Take(2);                                // 64, 36 
    ```
    - **Where**: Filters numbers greater than 5. The resulting sequence is `{ 8, 6, 7, 9 }`.
    - **Select**: Projects each number to its square. The resulting sequence is `{ 64, 36, 49, 81 }`.
    - **Take(2)**: Takes the first 2 elements from the projected sequence. The resulting sequence is `{ 64, 36 }`.

3. **Execution**:
    ```csharp
    foreach (var n in query)
        Console.WriteLine(n); 
    ```
    - The `foreach` loop triggers the execution of the query. The elements are processed and printed one by one.

### Key Points

- **Deferred Execution**: The query is only executed when it is enumerated. This allows the query to always operate on the most current data.
- **Performance Considerations**: While deferred execution keeps queries up-to-date, it can be more expensive to retrieve results compared to using a list that was executed immediately. This is because the query has to be re-evaluated every time it is enumerated.
- **Query Composition**: Deferred execution allows for query composition, meaning you can build complex queries by chaining multiple LINQ methods together.

### Summary

This example demonstrates the concept of deferred execution in LINQ. By using deferred execution, queries are always up-to-date with the current state of the data source. However, it can be more expensive in terms of performance compared to immediate execution, especially for large datasets. Understanding when to use deferred execution is crucial for optimizing performance and ensuring the correct behavior of your queries.