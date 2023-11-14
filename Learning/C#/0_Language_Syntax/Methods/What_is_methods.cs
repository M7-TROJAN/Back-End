/*
A method is a block of code which only runs when it is called.

You can pass data, known as parameters, into a method.

Methods are used to perform certain actions, and they are also known as functions.

C# is a fully OOP language , you cannot create a method outside class.

Create a Method
A method is defined with the name of the method, followed by parentheses (). 
C# provides some pre-defined methods, which you already are familiar with, such as Main(),
but you can also create your own methods to perform certain actions:

Example:
    static void MyMethod() 
    {
        code to be executed
    }
Remember That You should use static if you want to call the method without having object
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void PrintMyName()
        {
            Console.WriteLine("Mahmoud Mattar");
        }

        private static void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }

        static void PrintMyInfo(string Name, byte Age)
        {
            Console.WriteLine("Name= {0} , Age= {1}", Name, Age);
        }

        private static void Main(string[] args)
        {
            PrintMyName();
            SayHello("Mahmoud Mattar");
            PrintMyInfo("Mahmoud Mattar", 25);
            Console.ReadKey();
        }
    }
}