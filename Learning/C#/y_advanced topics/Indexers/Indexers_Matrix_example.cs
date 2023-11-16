using System;

public class Matrix
{
    private int[,] data = new int[3, 3];

    // Indexer definition for a 3x3 matrix
    public int this[int row, int col]
    {
        get
        {
            return data[row, col];
        }
        set
        {
            data[row, col] = value;
        }
    }
}

class Program
{
    static void Main()
    {
        Matrix matrix = new Matrix();

        // Set values using the indexer
        matrix[0, 0] = 1;
        matrix[0, 1] = 2;
        matrix[1, 1] = 5;
        matrix[2, 2] = 9;

        // Access values using the indexer
        Console.WriteLine(matrix[0, 0]); // Output: 1
        Console.WriteLine(matrix[0, 1]); // Output: 2
        Console.WriteLine(matrix[1, 1]); // Output: 5
        Console.WriteLine(matrix[2, 2]); // Output: 9
    }
}
