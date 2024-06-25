/*
C# Exceptions - Try..Catch

When executing C# code, different errors can occur: 
coding errors made by the programmer, errors due to wrong input, or other unforeseeable things.

When an error occurs, C# will normally stop and generate an error message. 
The technical term for this is: C# will throw an exception (throw an error).

C# try and catch
The 'try' statement allows you to define a block of code to be tested for errors while it is being executed.

The 'catch' statement allows you to define a block of code to be executed, if an error occurs in the try block.

The 'try' and 'catch' keywords come in pairs:

Syntax:
    try 
    {
        Block of code to try
    }
    catch (Exception e)
    {
        Block of code to handle errors
    }
*/


using System;

namespace Main
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                int x = 10, y = 0;
                int result = x / y; // This will cause a DivideByZeroException
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Finally block executed.");
            }

            Console.WriteLine("Program continues after exception handling.");

            Console.ReadKey();
        }
    }
}
