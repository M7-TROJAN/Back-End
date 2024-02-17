using System;
using System.Dynamic;

namespace InterfaceExample
{
    // Define IMove interface with a Move method
    interface IMove
    {
        void Move(); // Method signature for moving

        // Default implementation function (C# 8 feature)
        // Note: Before C# 8, interfaces did not support default implementations
        void Turn()
        {
            Console.WriteLine("Turning...");
        }
    }

    // Define IDisplace interface with a Move method (same name as in IMove)
    interface IDisplace
    {
        void Move(); // Method signature for moving (same name as in IMove)
    }

    // Vehicle class implementing both IMove and IDisplace interfaces
    public class Vehicle : IMove, IDisplace
    {
        // Explicit implementation of Move method from IMove interface
        void IMove.Move()
        {
            Console.WriteLine("IMove move");
        }

        // Explicit implementation of Move method from IDisplace interface
        void IDisplace.Move()
        {
            Console.WriteLine("IDisplace move");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Vehicle
            Vehicle vehicle = new Vehicle();

            // Call Move method from IMove interface
            // Since Vehicle class implements IMove, we can assign it to IMove type
            IMove moveObj = vehicle;
            moveObj.Move(); // Output: IMove move

            // Call Turn method from IMove interface (default implementation)
            moveObj.Turn(); // Output: Turning... (Default implementation)

            // Call Move method from IDisplace interface
            // Since Vehicle class implements IDisplace, we can assign it to IDisplace type
            IDisplace displaceObj = vehicle;
            displaceObj.Move(); // Output: IDisplace move

            // Since the Move method is explicitly implemented in Vehicle class,
            // it's not accessible through the class instance directly

            // Uncommenting the following line will result in a compilation error
            // vehicle.Move(); // Error: 'Vehicle' does not contain a definition for 'Move'

            /*
            **Explanation:**
            1. **Explicit Interface Implementation**: In the `Vehicle` class, 
            the `Move()` method is explicitly implemented for both the `IMove` and `IDisplace` interfaces. 
            This means that these methods are accessible only through an interface reference, 
            not directly through the class instance.

            2. **Calling Interface Methods**: To call the `Move()` method on a `Vehicle` object,
            you need to cast the object to the appropriate interface type and then call the method 
            through that interface reference.
            */

            // Create an instance of Vehicle
            Vehicle v = new Vehicle();

            // Call Move method from IMove interface
            // Cast the Vehicle object to IMove interface and then call the method
            ((IMove)v).Move(); // Output: IMove move

            // Call Move method from IDisplace interface
            // Cast the Vehicle object to IDisplace interface and then call the method
            ((IDisplace)v).Move(); // Output: IDisplace move

            // By explicitly casting the `vehicle` object to the respective interfaces (`IMove` and `IDisplace`),
            // you can access and call the `Move()` method correctly.
        }
    }
}
