/*
C# for loop
Same as C++

The for keyword is used to create for loop in C#. The syntax for for loop is:

for (initialization; condition; iterator)
{
	body of for loop
}


C# nested for loops
The syntax for nested for loops is:

for (initialization; condition; iterator)
{
	body of outre for loop

    for (initialization; condition; iterator)
    {
        body of inner for loop
    }
}

*/

using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("\nForward Loop:");
            //forward loop
            for (int i = 1; i <= 10; i++)

            {

                Console.WriteLine(i);

            }

            Console.WriteLine("\nBackward Loop:");
            //backword loop
            for (int i = 10; i >= 1; i--)

            {

                Console.WriteLine(i);

            }


            Console.WriteLine("\nNested Loops:");
            //forward loop
            for (int i = 1; i <= 10; i++)

            {

                for (int j = 0; j < 10; j++)

                {

                    Console.WriteLine("i={0} and j={1}", i, j);

                }

            }

            Console.ReadKey();

        }
    }
}

