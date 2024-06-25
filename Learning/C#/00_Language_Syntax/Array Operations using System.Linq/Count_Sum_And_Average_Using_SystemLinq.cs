/*
Array Operations using System.Linq
In C#, we have the System.Linq namespace that provides different methods to perform various operations in an array. For example,
Count, Sum, Average
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

            int[] numbers = { 20, 22, 19, 18, 1 };

            // compute Count
            Console.WriteLine("Count : " + numbers.Count());

            // compute Sum
            Console.WriteLine("Sum : " + numbers.Sum());

            // compute the average
            Console.WriteLine("Average: " + numbers.Average());

            Console.ReadKey();

        }
    }
}

