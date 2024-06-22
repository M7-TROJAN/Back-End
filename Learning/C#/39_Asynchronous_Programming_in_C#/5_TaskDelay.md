## Task Delay

Using delays in asynchronous programming is a common requirement, whether it is for simulating work, throttling processes, or waiting for a resource to become available. In C#, you can implement delays using `Task.Delay` for asynchronous non-blocking delays or `Thread.Sleep` for blocking delays. Here’s how you can use these methods effectively.

### Example 1: `Task.Delay` vs `Thread.Sleep`

In the first example, you might expect the statement `Task.Delay(5000) completed` to be printed after five seconds. However, this is not true because `Task.Delay` is asynchronous and does not block the calling thread.

```csharp
namespace CA06TaskDelay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DelayUsingTask(5000);

            Console.ReadKey();
        }

        static void DelayUsingTask(int ms)
        {
            Task.Delay(ms);
            Console.WriteLine("Task.Delay(5000) completed");
        }

        static void DelayUsingThreadSleep(int ms)
        {
            Thread.Sleep(ms);
            Console.WriteLine("Thread.Sleep(5000) completed");
        }
    }
}
```

In this code, `Console.WriteLine("Task.Delay(5000) completed");` is executed immediately because `Task.Delay(ms)` returns a `Task` that completes after the specified delay without blocking the calling thread.
Task.Delay() it's just returns an object and you should use it like:

```csharp
var task = Task.Delay(ms);

task.ContinueWith(t =>
{
    Console.WriteLine($"Task.Delay({ms}) completed");
});
```

```csharp
Task.Delay(ms).ContinueWith( x => 
    Console.WriteLine($"Task.Delay({ms}) completed")
);
```

On the other hand, when using `Thread.Sleep(ms)`:

```csharp
namespace CA06TaskDelay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // DelayUsingTask(5000);

            DelayUsingThreadSleep(5000);

            Console.ReadKey();
        }

        static void DelayUsingTask(int ms)
        {
            Task.Delay(ms);
            Console.WriteLine($"Task.Delay({ms}) completed");
        }

        static void DelayUsingThreadSleep(int ms)
        {
            Thread.Sleep(ms);
            Console.WriteLine($"Thread.Sleep({ms}) completed");
        }
    }
}
```

Here, `Thread.Sleep(ms)` blocks the calling thread for the specified duration, so `Console.WriteLine("Thread.Sleep(5000) completed");` is printed after five seconds.

### Example 2: Proper Asynchronous Delay Handling

To properly handle asynchronous delays and execute code after the delay completes, you need to await the `Task.Delay` or use a continuation.

```csharp
namespace CA06TaskDelay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DelayUsingTask(5000);

            Console.ReadKey();
        }

        static void DelayUsingTask(int ms)
        {
            Task.Delay(ms).GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine($"Task.Delay({ms}) completed");
            });
        }

        static void DelayUsingThreadSleep(int ms)
        {
            Thread.Sleep(ms);
            Console.WriteLine($"Thread.Sleep({ms}) completed");
        }
    }
}
```

In this code, `Task.Delay(ms).GetAwaiter().OnCompleted(...)` schedules the continuation (the code inside the lambda) to be executed after the delay completes, without blocking the main thread.

### Example 3: Using `await` for Simplicity

The `await` keyword provides a more straightforward way to handle asynchronous delays. 

```csharp
namespace CA06TaskDelay
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await DelayUsingTask(5000);

            Console.ReadKey();
        }

        static async Task DelayUsingTask(int ms)
        {
            await Task.Delay(ms);
            Console.WriteLine($"Task.Delay({ms}) completed");
        }

        static void DelayUsingThreadSleep(int ms)
        {
            Thread.Sleep(ms);
            Console.WriteLine($"Thread.Sleep({ms}) completed");
        }
    }
}
```

Here, the `await Task.Delay(ms)` statement pauses the `DelayUsingTask` method without blocking the main thread. After the delay, it prints the completion message. This approach makes the code easier to read and maintain.

### Summary

- **`Task.Delay(ms)`**: Asynchronous, non-blocking delay. Use `await` or continuations to perform actions after the delay.
- **`Thread.Sleep(ms)`**: Synchronous, blocking delay. Halts the calling thread for the specified duration.

By understanding the differences between these methods, you can choose the appropriate delay mechanism for your scenario, ensuring responsive and efficient code execution.