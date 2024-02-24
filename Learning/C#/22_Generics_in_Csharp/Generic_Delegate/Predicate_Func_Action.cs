using System;
using System.Collections.Generic;
using System.Threading.Channels;

namespace Predicate_Func_Action
{
    class Program
    {
        static void Main(string[] args)
        {

            Action<string> action = Print;
            action("Mahmoud");

            Func<int, int, int> func = Add;
            Console.WriteLine(func(1, 2));

            Predicate<int> predicate = IsEven;
            Console.WriteLine(predicate(1));
        }

        public static void Print(string value) => Console.WriteLine(value);
        public static int Add(int n1, int n2) => n1 + n2;
        public static bool IsEven(int value) => (value & 1) == 0;
        
    }
}
