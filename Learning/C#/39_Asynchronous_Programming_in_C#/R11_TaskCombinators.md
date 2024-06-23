Task combinators like `Task.WhenAll` and `Task.WhenAny` are powerful features in .NET that allow you to manage and coordinate multiple asynchronous tasks efficiently. Let's go through these concepts step by step.

## Task Combinators in .NET

### Overview

Task combinators are methods that help you manage multiple asynchronous operations. The two main task combinators are:

1. **`Task.WhenAll`**: Waits for all tasks to complete.
2. **`Task.WhenAny`**: Waits for any one task to complete.

### 1. Task.WhenAll

`Task.WhenAll` is used when you have multiple tasks that can run concurrently, and you want to wait for all of them to finish before proceeding. This method returns a single task that completes when all the provided tasks have completed.

#### Key Points:
- It waits for all tasks to complete.
- It aggregates the results of the tasks into an array.
- If any of the tasks fail, it will propagate the exception.

#### Example: Basic Usage

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Task<int> task1 = Task.Run(() => { Task.Delay(1000).Wait(); return 1; });
        Task<int> task2 = Task.Run(() => { Task.Delay(2000).Wait(); return 2; });
        Task<int> task3 = Task.Run(() => { Task.Delay(3000).Wait(); return 3; });

        int[] results = await Task.WhenAll(task1, task2, task3);

        Console.WriteLine($"Results: {string.Join(", ", results)}");
    }
}
```

In this example:
- Three tasks (`task1`, `task2`, `task3`) are created.
- Each task simulates work by delaying for a specified time.
- `Task.WhenAll` is used to wait for all tasks to complete.
- The results of the tasks are printed to the console.

#### Handling Exceptions

If any task throws an exception, `Task.WhenAll` will throw an `AggregateException` containing all the exceptions thrown by the tasks.

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Task task1 = Task.Run(() => { throw new InvalidOperationException("Task 1 failed"); });
        Task task2 = Task.Run(() => { Task.Delay(2000).Wait(); });
        Task task3 = Task.Run(() => { throw new ArgumentNullException("Task 3 failed"); });

        try
        {
            await Task.WhenAll(task1, task2, task3);
        }
        catch (AggregateException ex)
        {
            foreach (var innerEx in ex.InnerExceptions)
            {
                Console.WriteLine(innerEx.Message);
            }
        }
    }
}
```

### 2. Task.WhenAny

`Task.WhenAny` is used when you have multiple tasks running concurrently, and you want to proceed as soon as any one of them completes. This method returns a single task that completes when any of the provided tasks have completed.

#### Key Points:
- It waits for any one task to complete.
- It returns the task that completed first.
- If the first task that completes fails, `Task.WhenAny` will complete with that exception.

#### Example: Basic Usage

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Task<int> task1 = Task.Run(() => { Task.Delay(1000).Wait(); return 1; });
        Task<int> task2 = Task.Run(() => { Task.Delay(2000).Wait(); return 2; });
        Task<int> task3 = Task.Run(() => { Task.Delay(3000).Wait(); return 3; });

        Task<int> completedTask = await Task.WhenAny(task1, task2, task3);

        Console.WriteLine($"First completed task result: {completedTask.Result}");
    }
}
```

In this example:
- Three tasks (`task1`, `task2`, `task3`) are created.
- Each task simulates work by delaying for a specified time.
- `Task.WhenAny` is used to wait for the first task to complete.
- The result of the first completed task is printed to the console.

#### Handling Exceptions

If the first task that completes throws an exception, you need to handle it accordingly.

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Task task1 = Task.Run(() => { throw new InvalidOperationException("Task 1 failed"); });
        Task task2 = Task.Run(() => { Task.Delay(2000).Wait(); });
        Task task3 = Task.Run(() => { Task.Delay(3000).Wait(); });

        Task completedTask = await Task.WhenAny(task1, task2, task3);

        try
        {
            await completedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"First completed task threw an exception: {ex.Message}");
        }
    }
}
```

### Combining Task.WhenAll and Task.WhenAny

You can use `Task.WhenAll` and `Task.WhenAny` together to handle more complex scenarios. For example, you might want to wait for the first task to complete and then wait for all tasks to complete.

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Task<int> task1 = Task.Run(() => { Task.Delay(1000).Wait(); return 1; });
        Task<int> task2 = Task.Run(() => { Task.Delay(2000).Wait(); return 2; });
        Task<int> task3 = Task.Run(() => { Task.Delay(3000).Wait(); return 3; });

        Task<int> firstCompletedTask = await Task.WhenAny(task1, task2, task3);
        Console.WriteLine($"First completed task result: {firstCompletedTask.Result}");

        int[] results = await Task.WhenAll(task1, task2, task3);
        Console.WriteLine($"All tasks completed. Results: {string.Join(", ", results)}");
    }
}
```

In this example:
- We first wait for any task to complete using `Task.WhenAny`.
- We then wait for all tasks to complete using `Task.WhenAll`.
- The results are printed accordingly.

### Summary

- **`Task.WhenAll`**: Waits for all tasks to complete and aggregates their results.
  - Use this when you need to wait for multiple tasks to complete before proceeding.
  - Be mindful of exception handling, as it will throw an `AggregateException` if any task fails.

- **`Task.WhenAny`**: Waits for any one task to complete and returns that task.
  - Use this when you want to proceed as soon as the first task completes.
  - Handle exceptions appropriately, as the first completed task might fail.

By understanding and using these task combinators, you can effectively manage and coordinate multiple asynchronous operations in your .NET applications.
