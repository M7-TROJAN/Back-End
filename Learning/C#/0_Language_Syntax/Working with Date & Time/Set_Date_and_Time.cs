/*
Working with Date and Time in C#
C# includes DateTime struct to work with dates and times.

To work with date and time in C#, create an object of the DateTime struct using the new keyword. 

The following creates a DateTime object with the default value.

DateTime dt = new DateTime(); // assigns default value 01/01/0001 00:00:00


The default and the lowest value of a DateTime object is January 1, 0001 00:00:00 (midnight). 
The maximum value can be December 31, 9999 11:59:59 P.M.
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //assigns default value 01/01/0001 00:00:00
            DateTime dt1 = new DateTime();

            //assigns year, month, day
            DateTime dt2 = new DateTime(2023, 12, 31);

            //assigns year, month, day, hour, min, seconds
            DateTime dt3 = new DateTime(2023, 12, 31, 5, 10, 20);

            //assigns year, month, day, hour, min, seconds, UTC timezone
            DateTime dt4 = new DateTime(2023, 12, 31, 5, 10, 20, DateTimeKind.Utc);

            Console.WriteLine(dt1);
            Console.WriteLine(dt2);
            Console.WriteLine(dt3);
            Console.WriteLine(dt4);

            // Get The Current DateTime Now
            DateTime dt5 = DateTime.Now;
            Console.WriteLine("{0:dd/MM/yyyy h:mm tt}", dt5);

            Console.ReadKey();
        }
    }
}