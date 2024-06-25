/*
C# if (if-then) Statement
C# if-then statement will execute a block of code if the given condition is true.

The syntax of if-then statement in C# is:

if (boolean-expression)
{
	statements executed if boolean-expression is true
}


C# if...else (if-then-else) Statement
The if statement in C# may have an optional else statement. 
The block of code inside the else statement will be executed if the expression is evaluated to false.

The syntax of if...else statement in C# is:

if (boolean-expression)
{
	statements executed if boolean-expression is true
}
else
{
	statements executed if boolean-expression is false
}


C# if...else if (if-then-else if) Statement
When we have only one condition to test, if-then and if-then-else statement works fine.
But what if we have a multiple condition to test and execute one of the many block of code.

For such case, we can use if..else if statement in C#. The syntax for if...else if statement is:

if (boolean-expression-1)
{
	statements executed if boolean-expression-1 is true
}
else if (boolean-expression-2)
{
	statements executed if boolean-expression-2 is true
}
else if (boolean-expression-3)
{
	statements executed if boolean-expression-3 is true
}
.
.
.
else
{
	statements executed if all above expressions are false
}

*/


using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            int x = 10; int y = 20;

            //if then statement
            if (x == 10 && y <= 20)
            {
                Console.WriteLine("yes x = 10 and y<=20");
            }



            //if then else statement
            if (x == 11)
            {
                Console.WriteLine("yes x = 11 ");
            }
            else
            {
                Console.WriteLine("yes x != 11 ");
            }


            //if else if statement
            int number = 12;

            if (number < 5)
            {
                Console.WriteLine("{0} is less than 5", number);
            }
            else if (number > 5)
            {
                Console.WriteLine("{0} is greater than 5", number);
            }
            else
            {
                Console.WriteLine("{0} is equal to 5");
            }


            Console.ReadKey();

        }
    }
}
