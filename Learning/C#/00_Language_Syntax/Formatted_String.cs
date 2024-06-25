using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string name = "Mahmoud";
            int age = 25;

            string formattedString = string.Format("My name is {0} and I am {1} years old.", name, age);
            Console.WriteLine(formattedString);

            name = "Ali";
            age = 20;

            string interpolatedString = $"My name is {name} and I am {age} years old.";
            Console.WriteLine(interpolatedString);

            /*
                Both examples will produce the same output: "My name is Mahmoud and I am 25 years old."
                (for the string.Format example)
                and "My name is Ali and I am 20 years old." (for the interpolation example).
            */

            //---------------------------------------------------------------------------------

            double price = 19.99;

            string formattedPrice = string.Format("The price is: {0:C}", price);
            Console.WriteLine(formattedPrice);  // Output: "The price is: $19.99"

            //---------------------------------------------------------------------------------

            DateTime currentDate = DateTime.Now;

            string formattedDate = string.Format("Today's date is: {0:dd-MM-yyyy}", currentDate);
            Console.WriteLine(formattedDate);  // Output: "Today's date is: 2023-08-28"

            //---------------------------------------------------------------------------------

            string product = "Widget";
            double unitPrice = 9.99;

            string formattedProduct = string.Format("{0,-15} | {1,10:C}", product, unitPrice);
            Console.WriteLine(formattedProduct);  // Output: "Widget          |     $9.99"

            /*
             In this example, {0,-15} specifies that the product name should be left-aligned within a
             15-character space,
             while {1,10:C} specifies that the unit price should be right-aligned within a 10-character space
             and formatted as currency.
             */

            //---------------------------------------------------------------------------------

            double pi = Math.PI;

            string formattedPi = string.Format("The value of pi is approximately: {0:N2}", pi);
            Console.WriteLine(formattedPi);  // Output: "The value of pi is approximately: 3.14"

            //---------------------------------------------------------------------------------

            Console.WriteLine("{0} {1}", "Welcome To", "M7TROJAN");
            Console.WriteLine("My Name Is: {0}, I Live In {1}", "Mahmoud", "Egypt");

            Console.ReadKey();
        }
    }
}