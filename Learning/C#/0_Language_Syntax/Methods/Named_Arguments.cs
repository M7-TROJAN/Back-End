/*
C# Named Arguments
Named Arguments
It is also possible to send arguments with the key: value syntax.

That way, the order of the arguments does not matter.
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void MyMethod(string child1, string child2, string child3)
        {
            Console.WriteLine("The youngest child is: " + child3);
        }

        private static void Main(string[] args)
        {
            //see the order of sending parameters is not important.
            MyMethod(child3: "Omar", child1: "Saqer", child2: "Hamza");

            MyMethod(child1: "mahmoud", child2: "Ali", child3: "Moustafa");

            Console.ReadKey();
        }
    }
}