# Deferred Execution in LINQ

## Introduction

Deferred execution is a feature in LINQ that delays the evaluation of a query until its results are actually needed. This means that the query is not executed when it is defined, but when it is iterated over or accessed.

## Example Code

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class DeferredExecutionExample
{
    public static void Main()
    {
        List<int> ints = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        // Define a query to filter even numbers
        var evens = ints.Where(i => i % 2 == 0);

        // Add more items to the list
        ints.Add(14);
        ints.Add(16);

        // Iterate over the query
        foreach (var item in evens)
        {
            Console.WriteLine(item);
        }
    }
}
```

## Explanation

### Query Definition

```csharp
var evens = ints.Where(i => i % 2 == 0);
```

- This line defines a LINQ query named `evens` that filters the `ints` list to include only even numbers.
- The query is not executed at this point. It is only a definition of the logic that should be applied when the query is iterated over.

### Modifying the List

```csharp
ints.Add(14);
ints.Add(16);
```

- These lines add the numbers `14` and `16` to the list `ints`.
- Since the LINQ query has not been executed yet, these additions will be considered when the query is finally executed.

### Query Execution

```csharp
foreach (var item in evens)
{
    Console.WriteLine(item);
}
```

- The `foreach` loop iterates over the `evens` query.
- At this point, the LINQ query is executed.
- The query takes into account the current state of the `ints` list, which now includes `14` and `16`.

## Output

The output will include all even numbers in the list `ints` at the time of the query execution, including the newly added elements `14` and `16`:

```
2
4
6
8
10
12
14
16
```

## Additional Example

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class DeferredExecutionAdditionalExample
{
    public static void Main()
    {
        List<int> ints = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        // Define a query to filter even numbers
        var evens = ints.Where(i => i % 2 == 0);

        // Add more items to the list
        ints.Add(14);
        ints.Add(16);

        // Iterate over the query and print the results
        foreach (var item in evens)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine();

        // Add more items to the list
        ints.Add(18);
        ints.Add(20);

        // Iterate over the query again and print the results
        foreach (var item in evens)
        {
            Console.Write(item + " ");
        }
    }
}
```

## Output

The output will include all even numbers in the list `ints` at the time of the query execution, including the newly added elements `14`, `16`, `18`, and `20`:

```
2 4 6 8 10 12 14 16
2 4 6 8 10 12 14 16 18 20
```

## Benefits of Deferred Execution

- **Performance**: The query is executed only when needed, which can improve performance, especially when dealing with large datasets.
- **Flexibility**: Allows modifications to the data source before the query is executed, making the query results always up-to-date with the latest data.

## Summary

Deferred execution in LINQ means that a query is not executed when it is defined but when it is iterated over or accessed. This allows for greater flexibility and can improve performance by delaying the computation until it is actually needed. Understanding this concept is crucial for writing efficient and effective LINQ queries.
