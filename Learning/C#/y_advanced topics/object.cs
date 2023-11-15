using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Example 1: Object of type int
        object obj1 = 3;
        IdentifyAndSquare(obj1);

        // Example 2: Object of type float
        object obj2 = 3f;
        IdentifyAndSquare(obj2);

        // Example 3: Object of type double
        object obj3 = 3d;
        IdentifyAndSquare(obj3);

        // Example 4: Object of type string
        object obj4 = "Mahmoud";
        IdentifyAndSquare(obj4);

        // Example 5: Object of type int array
        object obj5 = new int[] { 1, 2, 3 };
        IdentifyAndSquare(obj5);
    }

    static void IdentifyAndSquare(object obj)
    {
        switch (obj)
        {
            case int i:
                Console.WriteLine($"It's an int, square of {i} is {i * i}");
                break;
            case long l:
                Console.WriteLine($"It's a long, square of {l} is {l * l}");
                break;
            case float f:
                Console.WriteLine($"It's a float, square of {f} is {f * f}");
                break;
            case double d:
                Console.WriteLine($"It's a double, square of {d} is {d * d}");
                break;
            case char c:
                Console.WriteLine($"It's a char, {c}");
                break;
            case string s:
                Console.WriteLine($"It's a string, {s}");
                break;
            case string[] sArray:
                Console.WriteLine($"It's an array of strings");
                sArray.PrintArray();
                break;
            case int[] iArray:
                Console.WriteLine($"It's an array of integers");
                iArray.PrintArray();
                break;
            default:
                Console.WriteLine("Unknown type.");
                break;
        }
    }
}

public static class ArrayExtensions
{
    // Extension Method to Print 1D Array
    public static void PrintArray<T>(this T[] source)
    {
        if (!source.Any())
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
