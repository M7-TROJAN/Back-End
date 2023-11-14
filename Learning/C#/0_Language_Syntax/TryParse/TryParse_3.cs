using System;

namespace ReadIntExample
{
    internal class Program
    {
        // Function to read an integer from the user
        private static int GetIntFromUser(string message = "")
        {
            string userInput;
            while (true)
            {
                Console.Write(message);
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int result))
                {
                    return result; // Return the parsed integer
                }

                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        private static void Main(string[] args)
        {
            string name;
            int age;

            Console.Write("Please enter your name: ");
            name = Console.ReadLine();

            age = GetIntFromUser("Please enter your age: ");

            Console.WriteLine($"Your name is: {name}, and your age is: {age}");

            Console.ReadKey();
        }
    }
}