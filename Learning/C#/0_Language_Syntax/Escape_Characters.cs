using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Useful Escape Characters:\n");

            //Newline
            Console.WriteLine("Newline:");
            Console.WriteLine("Welcome to \n M7TROJAN\n");

            //Tab
            Console.WriteLine("Tab:");
            Console.WriteLine("Welcome to\tM7TROJAN\n");

            //Backspace
            Console.WriteLine("Backspace:");
            Console.WriteLine("Welcome to \bM7TROJAN\n");

            //Single quote
            Console.WriteLine("Single Quote:");
            Console.WriteLine("Welcome to \' M7TROJAN\n");

            //Double quote
            Console.WriteLine("Double Quote:");
            Console.WriteLine("Welcome to \" M7TROJAN\n");

            //Backslash
            Console.WriteLine("Backslash:");
            Console.WriteLine("Welcome to \\ M7TROJAN\n");

            //Alert
            Console.WriteLine("Alert:");
            Console.WriteLine("Press any Key To Make alert......");
            Console.ReadKey();
            Console.WriteLine("\a");

            Console.WriteLine("Press any Key To Close This Window......");
            Console.ReadKey();
        }
    }
}