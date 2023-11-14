/*
Ticks
Ticks is a date and time expressed in the number of 100-nanosecond intervals 
that have elapsed since January 1, 0001, at 00:00:00.000 in the Gregorian calendar.

A single tick represents one hundred nanoseconds or one ten-millionth of a second.

There are 10,000 ticks in a millisecond and 10 million ticks in a second.
*/
using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            //number of 100-nanosecond intervals that have elapsed
            //since January 1, 0001, at 00:00:00.000 in the Gregorian calendar. 
           
            DateTime dt = new DateTime();
            Console.WriteLine( DateTime.MinValue.Ticks);  //min value of ticks
            Console.WriteLine(DateTime.MaxValue.Ticks); // max value of ticks


            Console.ReadKey();

            }
        }
    }

