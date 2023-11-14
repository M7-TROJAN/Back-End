/*
Two-Dimensional Array Declaration
Here's how we declare a 2D array in C#.

int[ , ] x = new int [2, 3];
Here, x is a two-dimensional array with 2 elements. And, each element is also an array with 3 elements.

So, all together the array can store 6 elements (2 * 3).

Note: The single comma [ , ] represents the array is 2 dimensional.

Two-Dimensional Array initialization
In C#, we can initialize an array during the declaration. For example,
    int[ , ] x = { { 1, 2 ,3}, { 3, 4, 5 } };

Here, x is a 2D array with two elements {1, 2, 3} and {3, 4, 5}. We can see that each element of the array is also an array.

We can also specify the number of rows and columns during the initialization. For example,
    int [ , ] x = new int[2, 3] { {1, 2, 3}, {3, 4, 5} };
*/

using System;


namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            //initializing 2D array
            int[,] numbers = { { 12, 13 }, { 55, 77 } };

            // access first element from the first row
            Console.WriteLine("Element at index [0, 0] : " + numbers[0, 0]);

            // access first element from second row
            Console.WriteLine("Element at index [1, 0] : " + numbers[1, 0]);


            Console.ReadKey();

            }
        }
    }

