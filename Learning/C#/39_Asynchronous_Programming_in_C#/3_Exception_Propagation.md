# Exception Propagation

Exception propagation is a fundamental concept in programming where an exception thrown in one method is passed up the call stack until it is caught by an appropriate exception handler. In the context of multithreading, handling exceptions can be more complex, especially when using threads directly. Understanding how exceptions propagate and how to properly handle them is crucial for robust application development.

## Exception Propagation in Threads

When an exception is thrown in a thread, it is caught within that thread's context. If not handled, it will terminate the thread, but it will not affect other threads, including the main thread. This can make it difficult to catch and handle exceptions that occur in background threads from the main thread.

### Example 1: Exception in a Thread (Not Propagated to Main Thread)

```csharp
namespace CA04ExceptionPropagation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var th = new Thread(ThrowException);
                th.Start();
                th.Join();
            }
            catch
            {
                Console.WriteLine("Exception is Thrown !!");
            }
        }

        static void ThrowException()
        {
            throw new NotImplementedException();
        }
    }
}
```

In this example, the exception thrown in the `ThrowException` method is not caught by the main thread's try-catch block. The exception is caught by the thread itself, terminating the thread without affecting the main thread.

### Example 2: Exception Handling Within a Thread

```csharp
namespace CA04ExceptionPropagation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var th = new Thread(ThrowExceptionWithTryCatch);
            th.Start();
            th.Join();
        }

        static void ThrowException()
        {
            throw new NotImplementedException();
        }

        static void ThrowExceptionWithTryCatch()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                Console.WriteLine("Exception is Thrown !!");
            }
        }
    }
}
```

In this example, the exception is handled within the `ThrowExceptionWithTryCatch` method. The main method is not in the same call stack as the method that throws the exception, so the exception is not propagated to the main thread.

## Exception Propagation in Tasks

Using `Task` instead of `Thread` simplifies exception handling. Tasks can propagate exceptions back to the calling thread, making it easier to catch and handle exceptions in the main thread or other parent threads.

### Example 3: Exception Propagation Using Tasks

```csharp
namespace CA04ExceptionPropagation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Task.Run(() => ThrowException()).Wait();
            }
            catch
            {
                Console.WriteLine("Exception is Thrown !!");
            }
        }

        static void ThrowException()
        {
            throw new NotImplementedException();
        }
    }
}
```

In this example, the exception thrown in the `ThrowException` method is propagated back to the main thread. The `Wait` method blocks the calling thread until the task completes and rethrows any exceptions that occurred in the task.

## Conclusion

Exception propagation is a key feature that distinguishes `Task` from `Thread`. While exceptions in threads are isolated to the thread itself, making them difficult to handle in the main thread, tasks provide a more seamless way to propagate exceptions back to the calling thread. This makes tasks more efficient and easier to work with in terms of exception handling.

### Summary

- **Threads**: Exceptions are caught within the thread and do not propagate to the main thread.
- **Tasks**: Exceptions can propagate to the calling thread, making it easier to handle them in the main method.

Understanding exception propagation is essential for writing robust and maintainable multithreaded applications. By using tasks, you can take advantage of better exception handling, improving the reliability and readability of your code.