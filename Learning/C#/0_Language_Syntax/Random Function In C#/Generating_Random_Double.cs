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

            // Generate a random double between 0 and 1
            double randomDouble = random.NextDouble();
            Console.WriteLine("Random double between 0 and 1: " + randomDouble);

            // Generate random doubles within a loop
            for (int i = 0; i < 5; i++)
            {
                double nextRandomDouble = random.NextDouble() * 10; // Generates between 0 and 10
                Console.WriteLine("Random double #" + (i + 1) + ": " + nextRandomDouble);
            }
        }
    }
}
