# Deferred Streamed Execution in LINQ

This code demonstrates the concept of deferred streamed execution in LINQ. Deferred streamed execution means that the query is not executed when constructed but only when it is enumerated, processing elements on demand without reading all source data upfront.

## Code

```csharp
private static void DemoDeferredStreamedExecution()
{
    //  Deferred Execution (Streaming):
    //  at the time of execution they do not read all source data
    //  before the yield element
    // Where is not required to find all matching items before fetching the first matching item. 
    // Where fetches matching items "on demand"
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };

    var query = numbers
        .Where(x =>
        {
            Console.WriteLine($"Where({x} > 5) => {x > 5}");
            return x > 5;
        })
        .Select(x =>
        {
            Console.WriteLine($"\tSelect({x} X {x}) => {x * x}");
            return x * x;
        })
        .Where(x =>
        {
            var result = x % 6 == 0;
            Console.WriteLine($"\t\tWhere({x} % 6 == 0) => {result}");
            if (result)
                Console.WriteLine($"\t\t\t\tTake: {x}");
            return x % 6 == 0;
        })
        .Take(2);

    var list = query.ToList();
}
```

## Explanation

### Deferred Streamed Execution

Deferred streamed execution means that the LINQ query processes elements on demand, fetching matching items as they are needed rather than reading all source data upfront. This is especially useful for large datasets or when the query involves operations that can short-circuit, such as `Take`.

### Code Breakdown

1. **Initial Array**:
    ```csharp
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };
    ```
    - An array of integers is defined.

2. **LINQ Query with Deferred Streamed Execution**:
    ```csharp
    var query = numbers
        .Where(x =>
        {
            Console.WriteLine($"Where({x} > 5) => {x > 5}");
            return x > 5;
        })
        .Select(x =>
        {
            Console.WriteLine($"\tSelect({x} X {x}) => {x * x}");
            return x * x;
        })
        .Where(x =>
        {
            var result = x % 6 == 0;
            Console.WriteLine($"\t\tWhere({x} % 6 == 0) => {result}");
            if (result)
                Console.WriteLine($"\t\t\t\tTake: {x}");
            return x % 6 == 0;
        })
        .Take(2);
    ```
    - **Where**: Filters numbers greater than 5. The lambda expression prints the value and the result of the condition.
    - **Select**: Projects each number to its square. The lambda expression prints the value and the result of the multiplication.
    - **Where**: Filters numbers that are divisible by 6. The lambda expression prints the value and the result of the condition. If the condition is true, it prints the value to be taken.
    - **Take(2)**: Takes the first 2 elements that match the previous conditions.

3. **Execution**:
    ```csharp
    var list = query.ToList();
    ```

    - Converts the query result to a list, triggering the execution of the query.

### Key Points

- **Deferred Streamed Execution**: The query processes elements on demand, fetching matching items as needed.
- **Performance Considerations**: Streamed execution can be more efficient than reading all source data upfront, especially for large datasets or when using operations like `Take` that can short-circuit.
- **Real-time Processing**: Since the query fetches data on demand, it operates on the most current data at the time of enumeration.

### Summary

This example demonstrates the concept of deferred streamed execution in LINQ. By using streamed execution, queries process elements on demand, fetching matching items as needed rather than reading all source data upfront. This approach can improve performance and ensure that the query operates on the most current data.