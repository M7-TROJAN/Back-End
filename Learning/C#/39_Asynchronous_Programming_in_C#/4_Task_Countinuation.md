# Task Continuation

Task continuation is an essential concept in asynchronous programming that allows you to specify additional actions to be executed after a task completes. This is particularly useful to avoid blocking the main thread and to keep your application responsive. In C#, you can use methods like `ContinueWith` and `GetAwaiter` to handle task continuations efficiently.

## Example 1: Blocking the Main Thread (Bad Practice)

In the first example, we use `Task.Result` to get the result of the task. This approach blocks the main thread until the task completes, which is generally a bad practice.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));

            // Bad practice: Blocking the main thread
            Console.WriteLine(task.Result);

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

In this example, the `Console.WriteLine("M7Trojan")` line will not execute until the task completes, which could take a significant amount of time, blocking the main thread.

## Example 2: Using `ContinueWith` to Avoid Blocking the Main Thread

Using the `ContinueWith` method allows you to continue execution on the main thread while the task completes asynchronously.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));

            // Use ContinueWith to avoid blocking the main thread
            task.ContinueWith(res =>
            {
                Console.WriteLine(res.Result);
            });

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

In this example, the main thread continues to execute the next line of code while the task is running. So, the `Console.WriteLine("M7Trojan")` line will execute before the task result. When the task completes, the continuation action prints the result.

## Example 3: Using `GetAwaiter` to Avoid Blocking the Main Thread

Another way to handle task continuations is by using the `GetAwaiter` method, which provides more control over the continuation.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));

            // Use GetAwaiter to avoid blocking the main thread
            var awaiter = task.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Console.WriteLine(awaiter.GetResult()); // GetResult method blocks the thread, but the task is already completed
            });

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

In this example, `GetAwaiter` is used to register a continuation that will execute when the task completes. This approach avoids blocking the main thread and allows for better control over the continuation.

## Exception Handling in Task Continuations

When working with task continuations, it is important to handle exceptions properly. If a task throws an exception, that exception can be observed in the continuation.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));

            // Use ContinueWith to handle exceptions
            task.ContinueWith(res =>
            {
                if (res.IsFaulted)
                {
                    Console.WriteLine($"Exception: {res.Exception?.GetBaseException().Message}");
                }
                else
                {
                    Console.WriteLine(res.Result);
                }
            });

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

## Using `await` for Continuations

The `await` keyword is a cleaner and more concise way to handle continuations in modern C#. It automatically handles exceptions and avoids blocking the main thread.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                int result = await Task.Run(() => CountPrimeNumbersInRange(2, 3000000));
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

## Task Continuation Options

You can control the behavior of task continuations using `TaskContinuationOptions`. These options allow you to specify conditions under which the continuation should run.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));

            task.ContinueWith(res =>
            {
                Console.WriteLine(res.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.ContinueWith(res =>
            {
                Console.WriteLine($"Exception: {res.Exception?.GetBaseException().Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

## Chaining Multiple Continuations

You can chain multiple continuations to handle different stages of task execution.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => CountPrimeNumbersInRange(2, 3000000))
                .ContinueWith(res => 
                {
                    Console

.WriteLine(res.Result);
                    return res.Result * 2;
                })
                .ContinueWith(res =>
                {
                    Console.WriteLine($"Doubled result: {res.Result}");
                });

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
```

## Task Continuations and Synchronization Context

By default, continuations might run on the original synchronization context (such as the UI thread). You can use `ConfigureAwait(false)` to avoid capturing the synchronization context and improve performance.

```csharp
namespace CA05TaskContinuation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                int result = await Task.Run(() => CountPrimeNumbersInRange(2, 3000000)).ConfigureAwait(false);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("M7Trojan");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false.
                    }
                }
                return true.
            }
        }
    }
}
```

## Best Practices for Task Continuations

- **Avoid Blocking Calls**: Prefer asynchronous methods like `await` over blocking calls like `Task.Wait` or `Task.Result`.
- **Handle Exceptions**: Always handle exceptions in continuations to avoid unobserved task exceptions.
- **Use `ConfigureAwait(false)`**: Use `ConfigureAwait(false)` in library code to avoid capturing the synchronization context unless necessary.
- **Use TaskContinuationOptions**: Use `TaskContinuationOptions` to specify conditions for continuations, such as running only on task success or failure.
- **Chaining Continuations**: Chain continuations to create a sequence of operations, each dependent on the previous one.

Understanding and utilizing task continuations efficiently can significantly improve the responsiveness and maintainability of your asynchronous applications.

## Conclusion

Using `ContinueWith` and `GetAwaiter` methods allows you to handle task continuations efficiently without blocking the main thread. This makes your applications more responsive and improves the user experience. Understanding and utilizing task continuations is crucial for writing efficient and maintainable asynchronous code in C#.