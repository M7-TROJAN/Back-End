## Understanding CancellationToken in .NET

- CancellationToken is the ability to cancel a task or thread.
- It is a struct that is passed to the task or thread that needs to be cancelled.
- The task or thread can check the token to see if it has been cancelled and then stop its work.

### What is a CancellationToken?

A `CancellationToken` in .NET provides a standardized way to cancel asynchronous operations or tasks. It allows you to safely and cooperatively cancel a running task or thread.

### Key Concepts

1. **CancellationToken**:
   - A struct passed to the task or thread that needs to be canceled.
   - The task or thread periodically checks this token to see if it has been canceled and then stops its work if necessary.

2. **CancellationTokenSource**:
   - An object that creates and controls a `CancellationToken`.
   - It has methods to signal cancellation and to retrieve the associated `CancellationToken`.

### How It Works

1. **Creating a CancellationToken**:
   - You create a `CancellationTokenSource` instance which generates a `CancellationToken`.
   - This token is passed to tasks or threads that need to support cancellation.

2. **Checking for Cancellation**:
   - The running task or thread periodically checks the `IsCancellationRequested` property of the token.
   - If `IsCancellationRequested` is `true`, the task or thread should gracefully stop its work.

3. **Signaling Cancellation**:
   - Calling the `Cancel` method on the `CancellationTokenSource` signals cancellation.
   - Any task or thread checking the token will see that cancellation has been requested and can respond accordingly.

### Examples

### Example: Using IsCancellationRequested

Here's an example that demonstrates using `IsCancellationRequested` to check for cancellation:

```csharp
namespace CA09CancellationToken
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task was cancelled");
                        return;
                    }

                    Console.WriteLine(i);
                    Thread.Sleep(1000);
                }
            }, token);

            Console.WriteLine("Press enter to cancel the task");
            Console.ReadLine();
            tokenSource.Cancel();

            task.Wait(); // Wait for the task to complete

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```

### Example: Using ThrowIfCancellationRequested

Another way to handle cancellation is to use `ThrowIfCancellationRequested`, which throws an `OperationCanceledException` when cancellation is requested. This exception can be caught to handle the cancellation more explicitly.

```csharp
namespace CA09CancellationToken
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Run(() => DoWork(token), token);

            Console.WriteLine("Press any key to cancel...");
            Console.ReadKey();
            tokenSource.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DoWork(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine($"Working... {i}");
                Thread.Sleep(1000); // Simulate work
            }
        }
    }
}
```

### Explanation

1. **CancellationTokenSource**:
   - We create an instance of `CancellationTokenSource` which gives us a `CancellationToken`.

2. **Task.Run**:
   - We start a task using `Task.Run` and pass the `CancellationToken` to it.

3. **Cancellation Check**:
   - Inside the `DoWork` method, we periodically check if cancellation has been requested using `token.ThrowIfCancellationRequested()`.
   - This throws an `OperationCanceledException` if cancellation is requested.

4. **Cancellation Request**:
   - We wait for a key press and then call `cts.Cancel()` to signal cancellation.

5. **Exception Handling**:
   - We catch `OperationCanceledException` to handle the cancellation gracefully.

### Advanced Example with Downloading

Here's a more advanced example that demonstrates canceling an asynchronous download operation:

```csharp
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var url = "https://www.microsoft.com";
            var task = DownloadContentAsync(url, token);

            Console.WriteLine("Press any key to cancel...");
            Console.ReadKey();
            cts.Cancel();

            try
            {
                var content = await task;
                Console.WriteLine($"Downloaded content: {content.Substring(0, 100)}...");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Download was cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task<string> DownloadContentAsync(string url, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url, token);
                token.ThrowIfCancellationRequested();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
```

### Explanation

1. **HttpClient and Cancellation**:
   - The `HttpClient.GetAsync` method accepts a `CancellationToken`.

2. **ThrowIfCancellationRequested**:
   - We call `token.ThrowIfCancellationRequested()` after the `GetAsync` call to handle cancellation after the request is sent.

3. **Cancellation Handling**:
   - The main method is similar to the previous example, where we wait for a key press to signal cancellation and handle the `OperationCanceledException`.


### Another Example

```csharp
namespace CA09CancellationToken
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // CancellationToken is the ability to cancel a task or thread.
            // It is a struct that is passed to the task or thread that needs to be cancelled.
            // The task or thread can check the token to see if it has been cancelled and then stop its work.

            CancellationTokenSource cts = new CancellationTokenSource();

            // Uncomment one of the following to test different approaches:
            
            // -- 1 --  
            // await DoCheck01(cts);

            // -- 2 --
            // await DoCheck02(cts);

            // -- 3 --
            await DoCheck03(cts);

            Console.ReadKey();
        }

        // Approach 1: Using IsCancellationRequested
        static async Task DoCheck01(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run(() => 
            { 
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                    Console.WriteLine("\nTask has been Cancelled !!");
                }
            });

            while (!token.IsCancellationRequested)
            {
                Console.Write("Checking ...");
                await Task.Delay(4000);
                Console.Write($"Completed on {DateTime.Now}");
                Console.WriteLine();
            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }

        // Approach 2: Using Task.Delay with CancellationToken
        static async Task DoCheck02(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run(() => 
            { 
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                    Console.WriteLine("\nTask has been Cancelled !!");
                }
            });

            while (true)
            {
                Console.Write("Checking ...");
                await Task.Delay(4000, token); // Task.Delay with CancellationToken
                Console.Write($"Completed on {DateTime.Now}");
                Console.WriteLine();
            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }

        // Approach 3: Using ThrowIfCancellationRequested
        static async Task DoCheck03(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run(() =>
            {
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                }
            });

            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested(); // Check for cancellation
                    Console.Write("Checking ...");
                    await Task.Delay(4000);
                    Console.Write($"Completed on {DateTime.Now}");
                    Console.WriteLine();
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }
    }
}
```

### Explanation

1. **Approach 1: Using IsCancellationRequested**
   - A task is run to listen for a key press. If 'Q' is pressed, the cancellation token is canceled.
   - The main loop periodically checks `token.IsCancellationRequested` to determine if it should stop.
   - This is a simple and straightforward way to check for cancellation.

2. **Approach 2: Using Task.Delay with CancellationToken**
   - Similar to the first approach, but `Task.Delay` is called with the `CancellationToken`.
   - If cancellation is requested, `Task.Delay` throws an `OperationCanceledException`.
   - This approach integrates cancellation into the delay itself, making it more responsive.

3. **Approach 3: Using ThrowIfCancellationRequested**
   - A task is run to listen for a key press. If 'Q' is pressed, the cancellation token is canceled.
   - Inside the main loop, `token.ThrowIfCancellationRequested` is called, which throws an `OperationCanceledException` if cancellation is requested.
   - The exception is caught, and the cancellation is handled gracefully.

### Additional Topics on CancellationToken

1. **Linked Tokens**:
   You can link multiple `CancellationTokenSource` instances to create a composite cancellation token that cancels if any of the source tokens are canceled.

   ```csharp
   var cts1 = new CancellationTokenSource();
   var cts2 = new CancellationTokenSource();
   var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);

   var token = linkedCts.Token;
   ```

2. **Timeout**:
   You can create a `CancellationTokenSource` that cancels after a specified time.

   ```csharp
   var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
   ```

3. **Registering Callbacks**:
   You can register callbacks to be invoked when the token is canceled.

   ```csharp
   var cts = new CancellationTokenSource();
   var token = cts.Token;
   token.Register(() => Console.WriteLine("Token was canceled"));

   // Trigger cancellation
   cts.Cancel();
   ```

### Summary

- **CancellationToken** allows for cooperative cancellation between tasks or threads and the code that started them.
- **CancellationTokenSource** generates and controls the cancellation token.
- Tasks or threads periodically check the token to see if cancellation has been requested and respond accordingly.
- Using `async` and `await` with cancellation tokens makes it easy to manage long-running or potentially blocking operations.

Understanding and using `CancellationToken` properly can help you create more responsive and robust applications that handle cancellation scenarios gracefully.
