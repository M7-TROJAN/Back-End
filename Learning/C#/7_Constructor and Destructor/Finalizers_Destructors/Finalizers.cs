
using System;

namespace Finalizers
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create some garbage objects
            MakeSomeGarbage();

            // Display the memory usage before garbage collection
            Console.WriteLine($"Memory used Before Collecting: {GC.GetTotalMemory(false):N0}");

            // Explicitly trigger garbage collection
            GC.Collect();

            // Display the memory usage after garbage collection
            Console.WriteLine($"Memory used After Collecting: {GC.GetTotalMemory(true):N0}");

            Console.ReadKey();
        }

        // Method to create some garbage objects
        static void MakeSomeGarbage()
        {
            const int iterations = 1000;

            // a reference to a Version object (Version is a larger class in .Net)
            var obj = new Version();

            for (int i = 0; i < iterations; i++)
            {
                // Reassign the reference to a new Version object in each iteration
                obj = new Version();
            }
        }
    }
}



/*
The `GC.Collect()` method in C# is used to explicitly trigger garbage collection.
Garbage collection is the process by which the .NET runtime reclaims memory occupied by objects that are no longer
in use. When you call GC.Collect(), you're requesting the runtime to reclaim memory
immediately rather than waiting for it to occur automatically.
*/
