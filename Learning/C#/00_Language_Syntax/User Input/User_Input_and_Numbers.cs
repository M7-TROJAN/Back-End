/*
User Input and Numbers
The Console.ReadLine() method returns a string. Therefore, you cannot get information from another data type, such as int.

therefore you should use casting when you read.
*/

using System;

namespace Main
{
    internal class Program
    {
        // Function to read an integer from the user
        private static int GetIntFromUser(string message = "")
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result; // Return the parsed integer
                }

                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter your age?");
            //if you dont convert you will get error, and if you enter string you will get error
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Your age is: " + age);

            int age2 = GetIntFromUser("Enter Your Age: ");
            Console.WriteLine("Your age is: " + age2);

            Console.ReadKey();
        }
    }
}