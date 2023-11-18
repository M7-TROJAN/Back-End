using System;

class Program
{
    // Define a delegate with Func<int, int> signature
    delegate int SquareDelegate(int x);

    static void Main()
    {
        // Example 1: Using a traditional delegate
        SquareDelegate squareMethod = Square;
        int result1 = squareMethod(5);
        Console.WriteLine($"Square using delegate: {result1}");

        // Example 2: Using a lambda expression
        Func<int, int> squareLambda = x => x * x;
        int result2 = squareLambda(5);
        Console.WriteLine($"Square using lambda: {result2}");
    }

    // Traditional method to calculate the square
    static int Square(int x)
    {
        return x * x;
    }
}
