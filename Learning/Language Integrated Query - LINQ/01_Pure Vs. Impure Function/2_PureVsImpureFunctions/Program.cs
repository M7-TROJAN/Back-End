using System;
using System.Collections.Generic;

namespace PureVsImpureFunctions
{
    internal class Program
    {
        // Global list of numbers
        static List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        static void Main(string[] args)
        {
            // Demonstrating impure functions
            AddInteger1(3); // Impure: modifies global variable
            int x = 2;
            AddInteger2(ref x); // Impure: modifies parameter and global variable
            AddInteger3(); // Impure: interacts with the outside world

            // Demonstrating pure function
            var newList = AddInteger4(numbers, 3); // Pure: returns new list without modifying original

            // Print results
            Console.WriteLine("Old list:");
            Print(numbers); // Printing original list
            Console.WriteLine("New list:");
            Print(newList); // Printing new list

            Console.ReadKey();
        }

        // Print method (Pure)
        static void Print(List<int> source)
        {
            foreach (var item in source)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }

        // Impure: Modifies global variable
        static void AddInteger1(int num)
        {
            numbers.Add(num); // Impure: modifies global variable
        }

        // Impure: Modifies parameter and global variable
        static void AddInteger2(ref int num)
        {
            num++; // Impure: modifies parameter
            numbers.Add(num); // Impure: modifies global variable
        }

        // Impure: Interacts with the outside world
        static void AddInteger3()
        {
            numbers.Add(new Random().Next()); // Impure: interacts with the outside world (random number generation)
        }

        // Pure: Returns a new list without modifying the original
        static List<int> AddInteger4(List<int> originalNumbers, int num)
        {
            var result = new List<int>(originalNumbers); // Create a copy of the original list
            result.Add(num); // Add the new number to the copy
            return result; // Return the new list
        }
    }
}
