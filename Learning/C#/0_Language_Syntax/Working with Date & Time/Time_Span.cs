/*
TimeSpan -> الفترة الزمنية
TimeSpan is a struct that is used to represent time in days, hour, minutes, seconds, and milliseconds.
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime dt = new DateTime(2023, 2, 21);

            // Hours, Minutes, Seconds
            TimeSpan ts = new TimeSpan(49, 25, 34);
            Console.WriteLine(ts);
            Console.WriteLine(ts.Days);
            Console.WriteLine(ts.Hours);
            Console.WriteLine(ts.Minutes);
            Console.WriteLine(ts.Seconds);

            //this will add time span to the date.
            DateTime newDate = dt.Add(ts);

            Console.WriteLine(newDate);

            Console.ReadKey();
        }
    }
}