/*
Get User Input
You have already learned that Console.WriteLine() is used to output (print) values, for input we use Console.ReadLine()

Equivalent to cin>> in C++

Important Note: Console.ReadLine() always reads string.
*/


using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            // Type your username and press enter
            Console.WriteLine("Enter username?");

            string userName = Console.ReadLine();
            Console.WriteLine("Username is: " + userName);

            Console.ReadKey();

        }
    }
}

