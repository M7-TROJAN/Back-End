### Finalizers in C#

Finalizers in C# are special methods that are automatically called by the garbage collector before reclaiming the memory occupied by an object that is no longer referenced. 
Finalizers are also known as destructors in C#. They are used to perform cleanup operations such as releasing unmanaged resources 
(e.g., file handles, network connections, database connections) held by an object before it is garbage collected.

In C#, finalizers are defined using the tilde (~) followed by the class name, and they cannot be explicitly called like regular methods. 
The .NET runtime invokes finalizers automatically as part of the garbage collection process.

However, finalizers have some limitations and drawbacks:

1. Finalizers introduce overhead to the garbage collection process because objects with finalizers require two garbage collection cycles to be reclaimed: one to finalize the object and another to actually reclaim its memory.
2. Finalizers are non-deterministic, meaning there's no guarantee when they will be executed. This can lead to resource leaks if unmanaged resources are not properly released.
3. Finalizers should be used sparingly and only for cleaning up unmanaged resources. In many cases, it's better to implement the IDisposable interface and use the Dispose pattern for deterministic resource cleanup.

Here's an example of a finalizer in C#:

```csharp
using System;

class MyClass
{
    // Finalizer (destructor)
    ~MyClass()
    {
        // Cleanup operations here
        Console.WriteLine("Finalizing MyClass");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of MyClass
        MyClass obj = new MyClass();

        // Let the garbage collector reclaim the memory
        obj = null;

        // Force garbage collection (for demonstration purposes only)
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Done");
    }
}
```

