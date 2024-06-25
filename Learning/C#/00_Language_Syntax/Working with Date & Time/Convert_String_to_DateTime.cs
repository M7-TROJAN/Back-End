/*
Convert String to DateTime
A valid date and time string can be converted to a DateTime object using Parse(), ParseExact(), TryParse() and TryParseExact() methods.

The Parse() and ParseExact() methods will throw an exception if the specified string is not a valid representation of a date and time. 
So, it's recommended to use TryParse() or TryParseExact() method because they return false if a string is not valid.
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var str = "6/12/2023";
            DateTime dt;

            var isValidDate = DateTime.TryParse(str, out dt);

            if (isValidDate)
                Console.WriteLine(dt);
            else
                Console.WriteLine($"{str} is not a valid date string");

            //invalid string date
            var str2 = "6/65/2023";
            DateTime dt2;

            var isValidDate2 = DateTime.TryParse(str2, out dt2);

            if (isValidDate2)
                Console.WriteLine(dt2);
            else
                Console.WriteLine($"{str2} is not a valid date string");

            Console.ReadKey();
        }
    }
}