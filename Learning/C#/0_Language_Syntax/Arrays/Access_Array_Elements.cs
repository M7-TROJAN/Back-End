/*
Access Array Elements
We can access the elements in the array using the index of the array. For example,

access element at index 2
    array[2];

access element at index 4
    array[4];

Here,
array[2] - access the 3rd element
array[4] - access the 5th element
*/

using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            // create an array
            int[] numbers = { 1, 2, 3 };

            //access first element
            Console.WriteLine("Element in first index : " + numbers[0]);

            //access second element
            Console.WriteLine("Element in second index : " + numbers[1]);

            //access third element
            Console.WriteLine("Element in third index : " + numbers[2]);


            //through loop
            Console.WriteLine("\nAccess array using loop:\n");
            for (int i = 0;i < numbers.Length; i++)
            {
                Console.WriteLine("Element in index {0} : {1} " ,i, numbers[0]);
            }

            Console.ReadKey();

            }
        }
    }

