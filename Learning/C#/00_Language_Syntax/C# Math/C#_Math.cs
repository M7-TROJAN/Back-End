/*
The C# Math class has many methods that allows you to perform mathematical tasks on numbers.

Math.Max(x,y)
    The 'Math.Max(x,y)' method can be used to find the highest value of x and y.

example:
    Math.Max(5, 10);
------------------------------------------------------------------------------------

Math.Min(x,y)
The 'Math.Min(x,y)' method can be used to find the lowest value of of x and y.

example:
    Math.Min(5, 10);
------------------------------------------------------------------------------------

Math.Sqrt(x)
The Math.Sqrt(x) method returns the square root of x

example:
    Math.Sqrt(64);
------------------------------------------------------------------------------------

Math.Abs(x)
The Math.Abs(x) method returns the absolute (positive) value of x.

example:
    Math.Abs(-4.7);
------------------------------------------------------------------------------------

Math.Round()
Math.Round() rounds a number to the nearest whole number.

example:
    Math.Round(9.99);
------------------------------------------------------------------------------------
*/


using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Max of 5, 10 is: {0}", Math.Max(5, 10));
            Console.WriteLine("Min of 5, 10 is: {0}", Math.Min(5, 10));
            Console.WriteLine("Squire Root of 64 is: {0}", Math.Sqrt(64));
            Console.WriteLine("Absolute (positive) value of  -4.7 is: {0}", Math.Abs(-4.7));
            Console.WriteLine("Round of 9.99 is: {0}", Math.Round(9.99));

            Console.ReadKey();

        }
    }
}

