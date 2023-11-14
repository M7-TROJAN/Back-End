// string interpolationExample

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fName = "mahmoud";
            string LName = "mattar";

            // Using string interpolation to combine and display the names
            Console.WriteLine($"{fName}, {LName}");

            // Using string interpolation to combine the names into another variable
            string fullName = $"{fName} {LName}";
            Console.WriteLine(fullName);

            Console.ReadKey();
        }
    }
}