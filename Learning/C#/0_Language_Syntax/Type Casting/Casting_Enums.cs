/*
Enums Conversion
Explicit casting is required to convert from an enum type to its underlying integral type.
*/


using System;

namespace Main
{
    internal class Program
    {

        enum WeekDays
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        static void Main(string[] args)
        {

            Console.WriteLine(WeekDays.Friday); //output: Friday 
            int day = (int)WeekDays.Friday; // enum to int conversion
            Console.WriteLine(day); //output: 4 

            var wd = (WeekDays)5; // int to enum conversion
            Console.WriteLine(wd);//output: Saturday 

            Console.ReadKey();

        }
    }
}

