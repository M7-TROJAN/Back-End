/*
Generate Random Number in Min to Max Range
Use the 'Next(int min, int max)' overload method to get a random integer that is within a specified range.

Example: Generate Random Integers in Range Copy
    Random rnd = new Random();

    for(int j = 0; j < 4; j++)
    {
        Console.WriteLine(rnd.Next(10, 20)); // returns random integers >= 10 and < 20
    }

Note:
    Remember that the Random class should be initialized only once and reused throughout the application 
    to ensure better randomness in the generated values.
*/

using System;

namespace RandomExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            // Generate a random integer between 1 and 100
            int randomNumber = random.Next(1, 101);
            Console.WriteLine("Random integer: " + randomNumber);

            // Generate random integers within a loop
            for (int i = 0; i < 5; i++)
            {
                int nextRandom = random.Next(10, 21); // Generates between 10 and 20
                Console.WriteLine("Random number #" + (i + 1) + ": " + nextRandom);
            }
        }
    }
}
