/*
Switch statement can be used to replace the if...else if statement in C#. 
The advantage of using switch over if...else if statement is the codes will look much cleaner and readable with switch.



Same as C++ :-) but it also support strings

The syntax of switch statement is:

switch (variable/expression)
{
    case value1:
        Statements executed if expression(or variable) = value1
        break;
    case value2:
        Statements executed if expression(or variable) = value1
        break;
    ... ... ... 
    ... ... ... 
    default:
        Statements executed if no case matches
}


A problem with the switch statement is, when the matching value is found, it executes all statements after it until the end of switch block.

To avoid this, we use break statement at the end of each case. 
The break statement stops the program from executing non-matching statements by terminating the execution of switch statement.
*/

using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            // Example1:

            char ch;
            Console.WriteLine("Enter a letter?");
            ch = Convert.ToChar(Console.ReadLine());

            switch (Char.ToLower(ch))
            {
                case 'a':
                    Console.WriteLine("Vowel");
                    break;
                case 'e':
                    Console.WriteLine("Vowel");
                    break;
                case 'i':
                    Console.WriteLine("Vowel");
                    break;
                case 'o':
                    Console.WriteLine("Vowel");
                    break;
                case 'u':
                    Console.WriteLine("Vowel");
                    break;
                default:
                    Console.WriteLine("Not a vowel");
                    break;
            }

            // .........................................................................

            // Example 2:
            char ch;
            Console.WriteLine("Enter a letter");
            ch = Convert.ToChar(Console.ReadLine());

            switch (Char.ToLower(ch))
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    Console.WriteLine("Vowel");
                    break;
                default:
                    Console.WriteLine("Not a vowel");
                    break;
            }

            // .........................................................................

            // Example3:
            //Simple Calculator
            char op;
            double first, second, result;

            Console.Write("Enter first number: ");
            first = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter second number: ");
            second = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter operator (+, -, *, /): ");
            op = (char)Console.Read();

            switch (op)
            {
                case '+':
                    result = first + second;
                    Console.WriteLine("{0} + {1} = {2}", first, second, result);
                    break;

                case '-':
                    result = first - second;
                    Console.WriteLine("{0} - {1} = {2}", first, second, result);
                    break;

                case '*':
                    result = first * second;
                    Console.WriteLine("{0} * {1} = {2}", first, second, result);
                    break;

                case '/':
                    result = first / second;
                    Console.WriteLine("{0} / {1} = {2}", first, second, result);
                    break;

                default:
                    Console.WriteLine("Invalid Operator");
                    break;

            }
            // .........................................................................

            // Example4:

            //You can compare string as well using switch
            string Name = "Mohammed";
            switch (Name.ToLower())
            {
                case "mohammed":
                    Console.WriteLine("Yes Mohammed");
                    break;
                case "ali":
                    Console.WriteLine("Yes ALi");
                    break;
                default:
                    Console.WriteLine("No Name Matched!");
                    break;
            }
            Console.ReadKey();

        }
    }
}
