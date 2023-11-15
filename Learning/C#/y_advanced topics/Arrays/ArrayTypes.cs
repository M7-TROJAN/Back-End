using System;
using System.Linq;

internal class ArrayTypes
{
    static void Main(string[] args)
    {
        // Example of a 1D Array
        int[] oneDimensionalArray = new int[3] { 1, 2, 3 };
        oneDimensionalArray.PrintArray();


        // Example of a 2D Array (Sudoku)
        int[,] sudoku = new[,]
        {
            { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
            { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
            { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
            { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
            { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
            { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
            { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
            { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
        };

        sudoku.Print2DArray();


        // Example of a Jagged Array (Array of Arrays)
        var jaggedArray = new int[][]
        {
            new int[] { 1, 2 },
            new int[] { 3 },
            new int[] { 4, 5, 6, 7, 8 },
            new int[] { 10 }
        };

        jaggedArray.PrintJaggedArray();
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

    // Extension Method to Print 2D Array
    public static void Print2DArray<T>(this T[,] source)
    {
        for (int i = 0; i < source.GetLength(0); i++)
        {
            T[] row = new T[source.GetLength(1)];
            for (int j = 0; j < source.GetLength(1); j++)
            {
                row[j] = source[i, j];
            }
            row.PrintArray();
        }
    }

    // Extension Method to Print Jagged Array
    public static void PrintJaggedArray<T>(this T[][] source)
    {
        for (int i = 0; i < source.Length; i++)
        {
            source[i].PrintArray();
        }
    }
}
