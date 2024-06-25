/*
Ternary Operator
The ternary operator ? : operates on three operands. It is a shorthand for if-then-else statement. Ternary operator can be used as follows:

variable = Condition? Expression1 : Expression2;


The ternary operator works as follows:

If the expression stated by Condition is true, the result of Expression1 is assigned to variable.
If it is false, the result of Expression2 is assigned to variable.
*/

using System;

namespace TernaryOperatorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 10;
            string result = (number > 5) ? "Number is greater than 5" : "Number is not greater than 5";

            Console.WriteLine(result);
        }
    }
}
