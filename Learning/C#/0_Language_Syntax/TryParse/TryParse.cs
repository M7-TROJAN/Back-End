using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
             In C#, the TryParse method is a useful utility provided by many primitive data types
             (like integers, doubles, etc.) and some complex types (like DateTime).
             It allows you to attempt to parse a string representation of a value into
             its corresponding data type, while also checking if the parsing
             was successful without throwing an exception.
             This is particularly helpful when you're dealing with user input or data from external sources
             where the input might not be valid.

            The TryParse method follows a specific pattern and returns a Boolean value to indicate
            whether the parsing was successful or not. If successful,
            the parsed value is stored in an output parameter.

            Here's the basic structure of the TryParse method
            bool success = SomeType.TryParse(stringValue, out ValueType result);
            */

            string userInput = "42";

            int parsedValue;
            bool success = int.TryParse(userInput, out parsedValue);

            if (success)
            {
                Console.WriteLine("Parsing successful! Parsed value: " + parsedValue);
            }
            else
            {
                Console.WriteLine("Parsing failed. Invalid input.");
            }

            Console.ReadKey();

            /*
            In this example, int.TryParse tries to parse the string "42" into an integer. 
            If successful, the parsed integer value is stored in the parsedValue variable, and success will be true. 
            If the parsing fails (for instance, if the user input is not a valid integer), 
            success will be false, and the value of parsedValue will be 0.

            This method is much safer to use than directly parsing with int.Parse, 
            which throws an exception if parsing fails. 
            It's a good practice to use TryParse when dealing with potentially unreliable input data.
            */
        }
    }
}



/*
The TryParse method is used for parsing strings into other data types, such as integers, doubles, dates, etc. 
It attempts to convert a string representation of a value into the specified data type and returns a Boolean value 
indicating whether the parsing was successful or not.

The TryParse method has the following syntax:

bool success = SomeType.TryParse(stringValue, out ValueType result);


Here's what each part of the syntax means:

- 'SomeType': This represents the data type you are trying to parse the string into. For example, 
    int.TryParse is used to parse a string into an integer (int).

- 'stringValue': This is the input string that you want to parse into the specified data type.

- 'out ValueType result': The out keyword is used to pass the result variable by reference. 
ValueType should be replaced with the actual data type you are trying to parse into 
(e.g., int, double, DateTime, etc.). The result variable will store the parsed value if the parsing is successful.
*/

