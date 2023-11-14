/*
Type casting is when you assign a value of one data type to another type.

In C#, there are two types of casting:

- 'Implicit Casting' (automatically) - converting a smaller type to a larger type size
    char -> int -> long -> float -> double

- 'Explicit Casting' (manually) - converting a larger type to a smaller size type
    double -> float -> long -> int -> char
*/

using System;

namespace ImplicitCastingExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Implicit casting from int to double
            int intValue = 42;
            double doubleValue = intValue;
            Console.WriteLine("Implicit casting from int to double: " + doubleValue);

            // Implicit casting from char to int
            char charValue = 'A';
            int intValueFromChar = charValue;
            Console.WriteLine("Implicit casting from char to int: " + intValueFromChar);

            // Implicit casting from float to double
            float floatValue = 3.14f;
            double doubleValueFromFloat = floatValue;
            Console.WriteLine("Implicit casting from float to double: " + doubleValueFromFloat);

            //......................................................................................

            // Explicit casting from double to int
            double doubleValue2 = 3.99;
            int intValue2 = (int)doubleValue2;
            Console.WriteLine("Explicit casting from double to int: " + intValue2);

            // Explicit casting from int to char
            int intValueForChar = 65;
            char charValue2 = (char)intValueForChar;
            Console.WriteLine("Explicit casting from int to char: " + charValue2);

            // Explicit casting from long to int (possible loss of data)
            long longValue = 1234567890L;
            int intValueFromLong = (int)longValue;
            Console.WriteLine("Explicit casting from long to int: " + intValueFromLong);

            Console.ReadLine();
        }
    }
}