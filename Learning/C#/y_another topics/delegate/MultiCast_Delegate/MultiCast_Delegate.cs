using System;

namespace MultiCast_Delegate
{
    // Multicast Delegate Example:
    // A multicast delegate is a delegate that can hold references to multiple methods.
    // When invoked, all referenced methods are called in the order they were added.

    // Note: The delegate's signature must match the signature of the functions it points to.
    // This includes having the same parameter types and return type to ensure proper invocation.
    public delegate void RectDelegate(decimal width, decimal height);

    class Program
    {
        static void Main(string[] args)
        {
            var helper = new RectangleHelper();

            // Create an instance of the delegate
            RectDelegate rectDelegate;

            // Add 'helper.GetArea' and 'helper.GetPerimeter' methods to the delegate
            rectDelegate = helper.GetArea;
            rectDelegate += helper.GetPerimeter;

            // Invoke the delegate, and all methods are called in the order they were added
            Console.WriteLine("Invoking RectDelegate with both GetArea and GetPerimeter:");
            rectDelegate(10, 10);

            // Remove a method from the delegate
            rectDelegate -= helper.GetArea;

            Console.WriteLine("\nAfter Unsubscribing rectDelegate.GetArea:");

            // Invoke the delegate again, excluding the removed method
            rectDelegate(10, 10);
        }
    }

    public class RectangleHelper
    {
        public void GetArea(decimal width, decimal height)
        {
            var result = width * height;
            Console.WriteLine($"Area: {width} * {height} = {result}");
        }

        public void GetPerimeter(decimal width, decimal height)
        {
            var result = 2 * (width + height);
            Console.WriteLine($"Perimeter: 2 x ({width} + {height}) = {result}");
        }
    }
}
