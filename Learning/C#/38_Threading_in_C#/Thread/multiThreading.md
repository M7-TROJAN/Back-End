# Multithreading in C#

Multithreading is a powerful feature in C# that allows for concurrent execution of code. This can improve the responsiveness and performance of your application by allowing multiple threads to run in parallel. This guide provides an overview of the `Thread` class in C# and includes examples to illustrate how to create and manage threads in your application.

## The `Thread` Class

The `Thread` class in C# is part of the `System.Threading` namespace. It provides the functionality to create, control, and manage individual threads. Below are key members of the `Thread` class:

### Key Members

- **Constructors**:
  - `Thread(ThreadStart)`: Initializes a new instance of the `Thread` class, specifying the method to be executed by the thread.
  - `Thread(ParameterizedThreadStart)`: Initializes a new instance of the `Thread` class, specifying a method that accepts a single parameter to be executed by the thread.

- **Properties**:
  - `IsAlive`: Gets a value indicating the execution status of the thread.
  - `IsBackground`: Gets or sets a value indicating whether a thread is a background thread.
  - `Name`: Gets or sets the name of the thread.
  - `Priority`: Gets or sets a value indicating the scheduling priority of a thread.
  - `ThreadState`: Gets a value containing the states of the current thread.

- **Methods**:
  - `Start()`: Causes the operating system to change the state of the current instance to `ThreadState.Running`.
  - `Join()`: Blocks the calling thread until the thread represented by this instance terminates.
  - `Abort()`: Raises a `ThreadAbortException` in the thread on which it is invoked, to begin the process of terminating the thread.
  - `Sleep()`: Suspends the current thread for a specified time.
  - `Interrupt()`: Interrupts a thread that is in the `WaitSleepJoin` state.

## Example Usages

### Example 1: Starting a Simple Thread

```csharp
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Thread.CurrentThread.Name = "Main Thread";
        Console.WriteLine($"{Thread.CurrentThread.Name}");

        var wallet1 = new Wallet("Mahmoud", 80);

        Thread thread1 = new Thread(wallet1.RunRandomTransactions);
        thread1.Name = "t1";
        Console.WriteLine($"t1 Background Thread: {thread1.IsBackground}"); // false
        Console.WriteLine($"after declaration {thread1.Name} state is: {thread1.ThreadState}"); // Unstarted

        thread1.Start();
        Console.WriteLine($"after start {thread1.Name} state is: {thread1.ThreadState}"); // Running

        Thread thread2 = new Thread(new ThreadStart(wallet1.RunRandomTransactions));
        thread2.Name = "t2";
        thread2.Start();

        Console.ReadKey();
    }
}
public class Wallet
{
    public string Name { get; private set; }
    public int BitCoins { get; private set; }

    public Wallet(string name, int bitCoins)
    {
        Name = name;
        BitCoins = bitCoins;
    }

    public void Debit(int amount)
    {
        Thread.Sleep(1000);
        BitCoins -= amount;
        Console.WriteLine(
            $"[Thread: {Thread.CurrentThread.ManagedThreadId} - {Thread.CurrentThread.Name}" +
            $", Processor Id: {Thread.GetCurrentProcessorId()}] -{amount}");
    }

    public void Credit(int amount)
    {
        Thread.Sleep(1000);
        BitCoins += amount;
        Console.WriteLine(
            $"[Thread: {Thread.CurrentThread.ManagedThreadId} - {Thread.CurrentThread.Name}" +
            $", Processor Id: {Thread.GetCurrentProcessorId()}] -{amount}");
    }

    public void Transfer(Wallet to, int amount)
    {
        Debit(amount);
        to.Credit(amount);
    }

    public void RunRandomTransactions()
    {
        var amounts = new int[] { 10, 20, 30, -20, 10, -10, 30, -10, 40, -20 };

        foreach (var amount in amounts)
        {
            var absValue = Math.Abs(amount);
            if (amount > 0)
                Credit(absValue);
            else
                Debit(absValue);

        }
    }

    public override string ToString()
    {
        return $"[{Name} has {BitCoins} bitcoins]";
    }
}
```

**Explanation**:
- The `Main` thread is named "Main Thread".
- Two additional threads (`thread1` and `thread2`) are created to run the `RunRandomTransactions` method on `wallet1`.
- `thread1` is started first, followed by `thread2`.
- Both threads run in parallel with the main thread and each other.

### Example 2: Using `Join()` to Wait for a Thread to Finish

```csharp
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Thread.CurrentThread.Name = "Main Thread";
        Console.WriteLine($"{Thread.CurrentThread.Name}");

        var wallet1 = new Wallet("Mahmoud", 80);

        Thread thread1 = new Thread(wallet1.RunRandomTransactions);
        thread1.Name = "t1";
        Console.WriteLine($"t1 Background Thread: {thread1.IsBackground}"); // false
        Console.WriteLine($"after declaration {thread1.Name} state is: {thread1.ThreadState}"); // Unstarted

        thread1.Start();
        Console.WriteLine($"after start {thread1.Name} state is: {thread1.ThreadState}"); // Running

        thread1.Join(); // Wait for thread1 to finish before starting thread2

        Thread thread2 = new Thread(new ThreadStart(wallet1.RunRandomTransactions));
        thread2.Name = "t2";
        thread2.Start();

        Console.ReadKey();
    }
}
```
In this example, `thread2` will only start after `thread1` has completed its execution, ensuring the tasks are performed sequentially.

**Explanation**:
- The `Main` thread is named "Main Thread".
- `thread1` is started and the `Main` thread waits for `thread1` to complete using the `Join()` method.
- Once `thread1` has finished, `thread2` is started.
- This ensures that `thread2` only runs after `thread1` has completed.

## When to Use `Join()`

The `Join()` method is useful when you need one thread to wait for another to complete before continuing. Here are some scenarios where `Join()` is commonly used:

1. **Sequential Task Execution**:
   When tasks must be performed in a specific order. For example, if `thread1` must complete its work before `thread2` starts, you use `Join()` to ensure this sequence.

2. **Dependent Operations**:
   When a thread needs to wait for another thread to finish because it relies on the result of the first thread's operations. For instance, if `thread2` depends on data processed by `thread1`, `Join()` ensures `thread1` completes before `thread2` begins.

3. **Resource Management**:
   To prevent resource conflicts and ensure proper resource management. For example, if two threads are accessing the same resource, you might use `Join()` to make sure one thread finishes using the resource before the other starts.

## Running the Code

When you run these examples, you should see the threads running and their states changing as they execute. The `Console.ReadKey()` is used to keep the console application open until the user presses a key, allowing you to observe the output of the threads.

### Example Output

The output will show interleaved messages from the threads, demonstrating concurrent execution:

```
Main Thread
t1 Background Thread: False
after declaration t1 state is: Unstarted
after start t1 state is: Running
[Thread: 3, Processor Id: 1] 10
[Thread: 3, Processor Id: 1] 20
[Thread: 4, Processor Id: 2] 10
[Thread: 4, Processor Id: 2] 20
...
```

In the second example, `thread2` will only start after `thread1` has finished, ensuring sequential execution:

```
Main Thread
t1 Background Thread: False
after declaration t1 state is: Unstarted
after start t1 state is: Running
[Thread: 3, Processor Id: 1] 10
[Thread: 3, Processor Id: 1] 20
...
after start t2 state is: Running
[Thread: 4, Processor Id: 2] 10
[Thread: 4, Processor Id: 2] 20
...
```

## Conclusion

The `Thread` class in C# provides a robust mechanism for creating and managing threads, enabling concurrent execution of tasks. By understanding and utilizing its properties and methods, you can achieve parallelism, improve application performance, and handle complex operations more efficiently. The provided examples illustrate how to start threads, check their states, and manage their execution flow using `Join()`. Additionally, understanding when to use `Join()` helps ensure that your threads execute in the desired order, avoiding potential conflicts and ensuring that dependent tasks are performed correctly.

---