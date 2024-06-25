using System;

namespace ReadIntExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Please enter an integer: ");
            string userInput = Console.ReadLine();
            int parsedValue = 0;

            while (true)
            {
                if (int.TryParse(userInput, out parsedValue))
                {
                    Console.WriteLine("You entered: " + parsedValue);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }

                Console.Write("Please enter an integer: ");
                userInput = Console.ReadLine();
            }
            Console.ReadKey();

        }
    }
}