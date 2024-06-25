/*
"expression-bodied methods" or "expression-bodied members.":
    It's a concise way to define methods or properties when they consist of a single expression.

if you have a single expression in other words just one line of code represents the return statement,
you can use body expression method instead of the full method definition.
*/

using System;

class Program
{
    static void Main()
    {
        // Example of an expression-bodied method
        int number = 20;

        // Call the IsEven method and print the result
        var result = IsEven(number);
        Console.WriteLine(result);
    }

    // Define an expression-bodied method to check if a number is even
    // Syntax: <returntype> <methodName>() => <Your Expression>
    bool IsEven(int number) => number % 2 == 0;
}

/*

bool IsEven(int number) => number % 2 == 0;

This is equivalent to a full method definition:

bool IsEven(int number) 
{ 
  return number % 2 == 0; 
}

*/
