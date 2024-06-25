/*
Explicit Casting
Explicit casting must be done manually by placing the type in parentheses in front of the value.
*/

using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {


            double myDouble = 17.58;
            int myInt = (int) myDouble;    // Manual casting: double to int

            Console.WriteLine(myDouble);   // Outputs 17.58
            Console.WriteLine(myInt);      // Outputs 17


            Console.ReadKey();

            }
        }
    }

