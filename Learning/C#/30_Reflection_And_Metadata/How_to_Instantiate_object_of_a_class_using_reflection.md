### How to Instantiate an object of a class using reflection, Instantiate means => (make an instance)

# How to Instantiate an Object of a Class Using Reflection

```csharp
using System;

namespace ReflectionDemo
{
    class Program
    {
        static void Main()
        {
            // Instantiate an integer using the standard way
            var i = new Int32(); // Equivalent to int i = 0;
            i = 10; // Assign a value to the integer

            // Obtain the type information for Int32 (integer)
            var type = typeof(Int32);

            // Instantiate an integer using reflection
            var obj = Activator.CreateInstance(type); // This creates an instance of type Int32, which is initialized to 0

            obj = 20; // Assign a value to the object
            Console.WriteLine(obj); // Output: 20

            // Instantiate a DateTime object using reflection with specific constructor parameters
            DateTime dateTime = (DateTime)Activator.CreateInstance(typeof(DateTime), 2020, 2, 2);
            Console.WriteLine(dateTime); // Output: 02/02/2020 00:00:00

            Console.ReadKey();
        }
    }
}
