/*
short-circuiting in programming refers to a behavior where the evaluation of an expression stops as soon as the final result can be determined. 
This is particularly relevant when using logical operators like AND (&&) and OR (||).

For example:
in an expression involving && (logical AND), 
if the left operand is false, the right operand won't be evaluated because the overall result is already known 
(it will be false regardless of the right operand).

Similarly, in an expression involving || (logical OR), 
if the left operand is true, the right operand won't be evaluated because the overall result is already known 
(it will be true regardless of the right operand).

// && || => يعني بيختصرو الطريق من الاخر short circuit بيعملو  
// & | => short circuit مبيعملوش 
*/

using System;
namespace Revision
{
    class Program
    {
        // A simple method that prints a message and returns true
        static bool Check()
        {
            Console.WriteLine("Checking.....");
            return true;
        }

        static void Main(string[] args)
        {
            // Logical OR operator (||) with short-circuiting
            Console.WriteLine($"Using || (Logical OR with short-circuiting):");
            bool valueOr = true || Check();
            Console.WriteLine($"Result: {valueOr}");
            Console.WriteLine();

            // Logical OR operator (|) without short-circuiting
            Console.WriteLine($"Using | (Logical OR without short-circuiting):");
            bool valuePipe = true | Check();
            Console.WriteLine($"Result: {valuePipe}");
            Console.WriteLine();

            // Explanation:
            // With || (logical OR with short-circuiting), the Check() method is not executed
            // because the first condition is already true, so the overall result is true.

            // With | (logical OR without short-circuiting), both sides of the expression are evaluated,
            // and the Check() method is executed even though the first condition is already true.

            // The output will show the difference in behavior between || and |.
        }
    }
}
