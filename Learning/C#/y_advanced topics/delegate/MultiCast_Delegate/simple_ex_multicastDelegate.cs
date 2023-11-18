using System;

public delegate void MyDelegate(string message);

public class Example
{
    public static void Method1(string message)
    {
        Console.WriteLine($"Method1: {message}");
    }

    public static void Method2(string message)
    {
        Console.WriteLine($"Method2: {message}");
    }

    public static void Method3(string message)
    {
        Console.WriteLine($"Method3: {message}");
    }

    public static void Main()
    {
        // Create an instance of the delegate and add multiple methods to it
        MyDelegate myDelegate = Method1;
        myDelegate += Method2;
        myDelegate += Method3;

        // Invoke the delegate, and all methods are called in the order they were added
        myDelegate("Hello, Multicast Delegate!");

        // Remove a method from the delegate
        myDelegate -= Method2;

        // Invoke the delegate again, excluding the removed method
        myDelegate("Hello, Updated Multicast Delegate!");
    }
}
