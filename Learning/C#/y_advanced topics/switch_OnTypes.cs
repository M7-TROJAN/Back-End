// Switch On Types Example

using System;

namespace Revision
{
    class Program
    {
        static void Main(string[] args)
        {
            object obj = 3;  // Object can be of any type  (ممكن يكون اي حاجة)

            switch (obj)
            {
                case int i:
                    Console.WriteLine("It's an integer.");
                    break;
                case string str:
                    Console.WriteLine("It's a string.");
                    break;
                default:
                    Console.WriteLine("It's another type.");
                    break;
            }
        }
    }
}
