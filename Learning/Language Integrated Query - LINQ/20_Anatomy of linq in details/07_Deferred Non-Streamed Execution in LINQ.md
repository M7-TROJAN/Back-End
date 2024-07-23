# Deferred Non-Streamed Execution in LINQ

This code demonstrates the concept of deferred non-streamed execution in LINQ. Deferred non-streamed execution means that the query processes elements after reading all source data, which is common in operations like sorting and grouping.

## Code

```csharp
private static void DemoDeferredNonStreamedExecution()
{
    // Deferred (Non-Streaming): i.e. Sorting, grouping must read all source data before yield element
    // Does not mean "this is a sequence that is ordered"
    // This is a sequence that has had an ordering operation applied to it
    // ThenBy to impose additional ordering

    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 12, 9 };
    var query = numbers
        .Where(x =>
        {
            Console.WriteLine($"Where({x} > 5) => {x > 5}");
            return x > 5;
        })
        .OrderBy(x => x) // Buffering: 6, 8, 9, 12
        .Select(x =>
        {
            var result = x * x;
            Console.WriteLine($"\tSelect({x} X {x}) => {result}");
            Console.WriteLine($"\t\t\t\tTake: {result}");
            return result;
        })
        .Take(2) // 36, 64 
        .ToList();

    var list = query.ToList();
}
```

## Explanation

### Deferred Non-Streamed Execution

Deferred non-streamed execution means that the query processes all source data before yielding any elements. This is typical for operations that require a complete dataset, such as sorting and grouping. These operations read the entire source data into a buffer before applying the subsequent query operations.

### Code Breakdown

1. **Initial Array**:
    ```csharp
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 12, 9 };
    ```
    - An array of integers is defined.

2. **LINQ Query with Deferred Non-Streamed Execution**:
    ```csharp
    var query = numbers
        .Where(x =>
        {
            Console.WriteLine($"Where({x} > 5) => {x > 5}");
            return x > 5;
        })
        .OrderBy(x => x) // Buffering: 6, 8, 9, 12
        .Select(x =>
        {
            var result = x * x;
            Console.WriteLine($"\tSelect({x} X {x}) => {result}");
            Console.WriteLine($"\t\t\t\tTake: {result}");
            return result;
        })
        .Take(2) // 36, 64 
        .ToList();
    ```
    - **Where**: Filters numbers greater than 5. The lambda expression prints the value and the result of the condition. The resulting sequence is `{ 8, 6, 12, 9 }`.
    - **OrderBy**: Sorts the filtered numbers in ascending order. This operation requires buffering the entire sequence before proceeding. The resulting sequence is `{ 6, 8, 9, 12 }`.
    - **Select**: Projects each number to its square. The lambda expression prints the value and the result of the multiplication. The resulting sequence is `{ 36, 64, 81, 144 }`.
    - **Take(2)**: Takes the first 2 elements from the projected sequence. The resulting sequence is `{ 36, 64 }`.
    - **ToList**: Converts the query result to a list, causing the query to be executed.

3. **Execution**:
    ```csharp
    var list = query.ToList();
    ```

    - Converts the query result to a list, triggering the execution of the query.

### Key Points

- **Deferred Non-Streamed Execution**: The query reads all source data before yielding any elements, which is necessary for operations like sorting and grouping.
- **Buffering**: Operations such as `OrderBy` require buffering the entire sequence to apply the ordering operation.
- **Performance Considerations**: Non-streamed operations can be more expensive in terms of memory and processing time, especially for large datasets.

### Summary

This example demonstrates the concept of deferred non-streamed execution in LINQ. By using non-streamed execution, queries read all source data before yielding any elements, which is necessary for operations like sorting and grouping. Understanding when to use deferred non-streamed execution is crucial for optimizing performance and ensuring the correct behavior of your queries.