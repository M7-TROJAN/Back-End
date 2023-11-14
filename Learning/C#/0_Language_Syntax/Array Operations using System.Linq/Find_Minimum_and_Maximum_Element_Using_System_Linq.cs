/*
Array Operations using System.Linq
In C#, we have the System.Linq namespace that provides different methods to perform various operations in an array. For example,
Find Minimum and Maximum Element
*/


using System;

// provides us various methods to use in an array
using System.Linq;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            // Note that we used System.Linq above.

            int[] numbers = { 51, -1, 2, 14, 18, 40, 178 };

            // get the minimum element
            Console.WriteLine("Smallest  Element: " + numbers.Min());

            // Max() returns the largest number in array
            Console.WriteLine("Largest Element: " + numbers.Max());

            Console.ReadKey();

        }
    }
}


