// Switch On Types Example

using System;

namespace Revision
{
    class Program
    {
        static void Main(string[] args)
        {
            object obj = 3;  // Object can be of any type  (ممكن يكون اي حاجة)

            switch (obj)
            {
                case int i:
                    Console.WriteLine("It's an integer.");
                    break;
                case string str:
                    Console.WriteLine("It's a string.");
                    break;
                default:
                    Console.WriteLine("It's another type.");
                    break;
            }


            var isVip = true;

            switch (isVip)
            {
                case bool i when i == true:
                    Console.WriteLine("Yes.");
                    break;
                case bool i when i == false:
                    Console.WriteLine("No.");
                    break;
            }


            // Checking Range Example:
            int number = 15;
            switch (number)
            {
                case int n when n > 10 && n <= 20:
                    Console.WriteLine("Number is between 11 and 20.");
                    break;
                case int n when n > 20:
                    Console.WriteLine("Number is greater than 20.");
                    break;
                default:
                    Console.WriteLine("Number is 10 or less.");
                    break;
            }

            // Checking Type and Property Example:
            object obj = "Hello";
            switch (obj)
            {
                case string s when s.Length > 5:
                    Console.WriteLine("String has more than 5 characters.");
                    break;
                case string s when s.Length <= 5:
                    Console.WriteLine("String has 5 or fewer characters.");
                    break;
                case int i:
                    Console.WriteLine("It's an integer.");
                    break;
                default:
                    Console.WriteLine("Unknown type.");
                    break;
            }


            // Checking for Null Example:
            string name = null;
            switch (name)
            {
                case string s when !string.IsNullOrEmpty(s):
                    Console.WriteLine("Name is not null or empty.");
                    break;
                case null:
                    Console.WriteLine("Name is null.");
                    break;
                default:
                    Console.WriteLine("Name is empty.");
                    break;
            }


        }
    }
}
