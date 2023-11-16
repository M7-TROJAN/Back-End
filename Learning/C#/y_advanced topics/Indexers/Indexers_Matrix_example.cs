using System;

public class Matrix
{
    private readonly uint row;
    private readonly uint column;
    private int[,] data;


    public Matrix(uint row, uint col)
    {
        this.row = row;
        this.column = col;
        data = new int[row, column];
    }

    // Indexer definition for a matrix
    public int this[uint row, uint col]
    {
        get
        {
            if (row < this.row && col < this.column)
                return data[row, col];
            else
                throw new ArgumentOutOfRangeException("Invalid matrix indices.");
        }
        set
        {
            if (row < this.row && col < this.column)
                data[row, col] = value;
            else
                throw new ArgumentOutOfRangeException("Invalid matrix indices.");
        }
    }

}

class Program
{
    static void Main()
    {
        Matrix matrix = new Matrix(3,3);

        // Set values using the indexer
        matrix[0, 0] = 1;
        matrix[0, 1] = 2;
        matrix[0, 2] = 3;
        matrix[1, 0] = 4;
        matrix[1, 1] = 5;
        matrix[1, 2] = 6;
        matrix[2, 0] = 7;
        matrix[2, 1] = 8;
        matrix[2, 2] = 9;

        // Access values using the indexer
        Console.WriteLine(matrix[0, 0]); // Output: 1
        Console.WriteLine(matrix[0, 1]); // Output: 2
        Console.WriteLine(matrix[0, 2]); // Output: 3
        Console.WriteLine(matrix[1, 0]); // Output: 4
        Console.WriteLine(matrix[1, 1]); // Output: 5
        Console.WriteLine(matrix[1, 2]); // Output: 6
        Console.WriteLine(matrix[2, 0]); // Output: 7
        Console.WriteLine(matrix[2, 1]); // Output: 8
        Console.WriteLine(matrix[2, 2]); // Output: 9
    }
}
