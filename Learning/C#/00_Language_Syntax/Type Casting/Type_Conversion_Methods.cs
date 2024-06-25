/*
Type Conversion Methods
It is also possible to convert data types explicitly by using built-in methods, such as Convert.ToBoolean, Convert.ToDouble, 
Convert.ToString, Convert.ToInt32 (int) and Convert.ToInt64 (long)
*/


using System;

namespace ExplicitConversionExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Converting to boolean
            int intValue = 1;
            bool boolValue = Convert.ToBoolean(intValue);
            Console.WriteLine("Converted to boolean: " + boolValue);

            // Converting to double
            float floatValue = 3.14f;
            double doubleValue = Convert.ToDouble(floatValue);
            Console.WriteLine("Converted to double: " + doubleValue);

            // Converting to string
            char charValue = 'A';
            string stringValue = Convert.ToString(charValue);
            Console.WriteLine("Converted to string: " + stringValue);

            // Converting to int
            long longValue = 12345L;
            int intValue2 = Convert.ToInt32(longValue);
            Console.WriteLine("Converted to int: " + intValue2);

            // Converting to long
            double largeDoubleValue = 1234567890.12345;
            long longValueFromDouble = Convert.ToInt64(largeDoubleValue);
            Console.WriteLine("Converted to long: " + longValueFromDouble);

            // Converting integer to string
            int intValue3 = 42;
            string stringValue2 = Convert.ToString(intValue3);
            Console.WriteLine("Converted to string: " + stringValue2);

            // Converting double to string
            double doubleValue2 = 3.14;
            stringValue2 = Convert.ToString(doubleValue2);
            Console.WriteLine("Converted to string: " + stringValue2);

            // Converting boolean to string
            bool boolValue2 = true;
            stringValue2 = Convert.ToString(boolValue2);
            Console.WriteLine("Converted to string: " + stringValue2);

            // Converting DateTime to string
            DateTime dateTimeValue = DateTime.Now;
            stringValue2 = Convert.ToString(dateTimeValue);
            Console.WriteLine("Converted to string: " + stringValue2);

            // Converting string to DateTime
            string dateString = "2023-08-29";
            DateTime dateTimeValue2 = Convert.ToDateTime(dateString);
            Console.WriteLine("Converted to DateTime: " + dateTimeValue2);

            // Converting string with specific format to DateTime
            string dateStringWithFormat = "08/29/2023";
            dateTimeValue2 = DateTime.ParseExact(dateStringWithFormat, "MM/dd/yyyy", null);
            Console.WriteLine("Converted to DateTime: " + dateTimeValue2);

            Console.ReadLine();
        }
    }
}
