/*
Implicit casting is done automatically when passing a smaller size type to a larger size type
*/

using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {


            int myInt = 17;
            double myDouble = myInt;       // Automatic casting: int to double

            Console.WriteLine(myInt);      // Outputs 17
            Console.WriteLine(myDouble);   // Outputs 17


            Console.ReadKey();

        }
    }
}

