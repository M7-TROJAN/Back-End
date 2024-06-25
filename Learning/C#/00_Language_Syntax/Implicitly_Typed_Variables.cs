
using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Implicitly typed variables
             Alternatively in C#, we can declare a variable without knowing
             its type using var keyword.
             Such variables are called implicitly typed local variables.

             Variables declared using var keyword must be initialized at the time of declaration.
            */

            // Implicitly typed variables
            var x = 10;          // x is implicitly of type int
            var y = 10.5;       // y is implicitly of type double
            var z = "Mahmoud"; // z is implicitly of type string

            // Printing the values of the implicitly typed variables
            Console.WriteLine("x={0}, y={1}, z={2}", x, y, z);

            Console.ReadKey();
        }
    }
}

