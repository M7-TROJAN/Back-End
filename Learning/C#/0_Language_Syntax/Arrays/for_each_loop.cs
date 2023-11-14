/*
C# foreach loop
We will learn about foreach loops (an alternative to for loop) and how to use them with arrays and collections.

C# provides an easy to use and more readable alternative to for loop, 
the foreach loop when working with arrays and collections to iterate through the items of arrays/collections. 
The foreach loop iterates through each item, hence called foreach loop.

Syntax of foreach loop:
    foreach (element in iterable-item)
    {
        body of foreach loop
    }
Here iterable-item can be an array or a class of collection
*/

using System;


namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            // Example1:
            char[] myArray = { 'H', 'e', 'l', 'l', 'o' };

            foreach (char ch in myArray)
            {
                Console.WriteLine(ch);
            }

            Console.ReadKey();

            // Example2:
            char[] gender = { 'm', 'f', 'm', 'm', 'm', 'f', 'f', 'm', 'm', 'f' };
            int male = 0, female = 0;
            foreach (char g in gender)
            {
                if (g == 'm')
                    male++;
                else if (g == 'f')
                    female++;
            }
            Console.WriteLine("Number of male = {0}", male);
            Console.WriteLine("Number of female = {0}", female);

            Console.ReadKey();

        }
    }
}


