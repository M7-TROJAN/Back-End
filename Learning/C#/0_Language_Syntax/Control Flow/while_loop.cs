/*
The while keyword is used to create while loop in C#. The syntax for while loop is:

Same as C++

while (test-expression)
{
	body of while
}
*/

using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            int i = 1;
            while (i <= 5)
            {
                Console.WriteLine("C# while Loop: Iteration {0}", i);
                i++;
            }

            Console.ReadKey();

            }
        }
    }

