# Concurrency and Parallelism
 - ![Concurrency and Parallelism](./Concurrency_VS_Parallelism.png)
## Introduction

Concurrency and Parallelism are fundamental concepts in computer science, particularly when dealing with multitasking and performance optimization. They allow multiple tasks to be executed in overlapping periods or simultaneously, improving the efficiency and responsiveness of applications.

## Concurrency

### Definition

Concurrency is when two or more tasks can start, run, and complete in overlapping time periods. It doesn't mean the tasks are running at the same instant but rather that they can be in progress simultaneously.

### Characteristics

- **Time-Slicing:** Tasks may share the same processor (CPU), with the processor switching between tasks to give the appearance of simultaneous execution.
- **Single-Core Execution:** Concurrency can occur on a single-core CPU through context switching, where the CPU rapidly switches between tasks.
- **Threads:** Multiple threads are often used to achieve concurrency. Each thread represents a separate task.

### Example

Imagine you are typing a document while listening to music. You are doing both activities concurrently. Your attention (like the CPU) switches between typing and listening, but you are not doing both at the exact same moment.

### Code Example

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class ConcurrencyExample
{
    static async Task Main(string[] args)
    {
        Task task1 = Task.Run(() => DoWork("Task 1"));
        Task task2 = Task.Run(() => DoWork("Task 2"));

        await Task.WhenAll(task1, task2);
    }

    static void DoWork(string taskName)
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{taskName} is running.");
            Thread.Sleep(1000); // Simulate work
        }
    }
}
```

## Parallelism

### Definition

Parallelism is when tasks literally run at the same time, for example, on a multi-core processor. This means that multiple tasks are being executed simultaneously.

### Characteristics

- **Simultaneous Execution:** Tasks are executed simultaneously on multiple CPU cores.
- **Multi-Core Processing:** Parallelism requires a multi-core processor where each core can handle a separate task.
- **Efficiency:** Parallelism can significantly speed up processing time by dividing a task into sub-tasks that are executed concurrently.

### Example

Imagine you have a team of people assembling a car. Each person (like a CPU core) is working on a different part of the car at the same time. This is parallelism, where multiple parts of the task are being executed simultaneously.

### Code Example

```csharp
using System;
using System.Threading.Tasks;

class ParallelismExample
{
    static void Main(string[] args)
    {
        Parallel.Invoke(
            () => DoWork("Task 1"),
            () => DoWork("Task 2")
        );
    }

    static void DoWork(string taskName)
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{taskName} is running.");
            Task.Delay(1000).Wait(); // Simulate work
        }
    }
}
```

## Key Differences

- **Concurrency:** Tasks may not run at the same instant but can be in progress simultaneously. It relies on the system's ability to manage multiple tasks through context switching.
- **Parallelism:** Tasks run simultaneously on different processors or cores. It is a subset of concurrency where tasks truly execute at the same time.

## Summary

- **Concurrency** allows multiple tasks to progress at overlapping times through context switching. It is useful for improving responsiveness.
- **Parallelism** executes multiple tasks at the same time on different cores, improving throughput and performance.

Understanding these concepts helps in designing efficient and responsive applications, especially when dealing with long-running or compute-intensive tasks.
