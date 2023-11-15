using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Define an array of friends
        var friends = new string[] { "Mahmoud", "Mohamed", "Abdalaziz", "Ali", "Maged", "Mattar" };

        // Print the original array
        Console.WriteLine("Original Array:");
        friends.PrintArray();

        // Example 1: Slice to get the first two elements
        Console.WriteLine("\nSlicing to get the first two elements:");
        var slice1 = friends[..2]; // return the first tow elements (from start to the index number 2 (2 is execlusiv))
        slice1.PrintArray();

        // Example 2: Slice to get the first three elements
        Console.WriteLine("\nSlicing to get the first three elements:");
        var slice2 = friends[..3]; // return the first three elements (from index 0 to the index number 3 (3 is execlusiv))
        slice2.PrintArray();

        // Example 3: Slice to skip the first two elements
        Console.WriteLine("\nSlicing to skip the first two elements:");
        var slice3 = friends[2..]; // // Skip the first two elements and return the (from index 2 to the end (index 2 is inclusive))
        slice3.PrintArray();

        // Example 4: Slice to get elements starting from index 2 until index 3 (exclusive)
        Console.WriteLine("\nSlicing to get elements starting from index 2 until index 3 (exclusive):");
        var slice4 = friends[2..3];
        slice4.PrintArray();

        // Example 5: Slice to get elements starting from index 2 to the third-to-last element
        Console.WriteLine("\nSlicing to get elements starting from index 2 to the third-to-last element:");
        var slice5 = friends[2..^3]; // من الاندكس رقم 2 لعند الاندكس التالت من الاخر
        slice5.PrintArray();
    }
}

public static class Extensions
{
    // Extension method to print an array
    public static void PrintArray<T>(this T[] source)
    {
        if (source == null || source.Length == 0)
        {
            Console.WriteLine("{}");
            return;
        }

        Console.Write("{");
        for (int i = 0; i < source.Length; i++)
        {
            Console.Write(source[i]);
            Console.Write(i < source.Length - 1 ? ", " : "");
        }
        Console.WriteLine("}");
    }
}
