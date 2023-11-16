using System;

public class Sudoku
{
    private readonly int[,] board;

    public Sudoku()
    {
        board = new int[9, 9];
        Initialize();
    }

    private void Initialize()
    {
        // Initial Sudoku configuration (a solved Sudoku puzzle)
        int[,] initialConfiguration =
        {
            {5, 3, 4, 6, 7, 8, 9, 1, 2},
            {6, 7, 2, 1, 9, 5, 3, 4, 8},
            {1, 9, 8, 3, 4, 2, 5, 6, 7},
            {8, 5, 9, 7, 6, 1, 4, 2, 3},
            {4, 2, 6, 8, 5, 3, 7, 9, 1},
            {7, 1, 3, 9, 2, 4, 8, 5, 6},
            {9, 6, 1, 5, 3, 7, 2, 8, 4},
            {2, 8, 7, 4, 1, 9, 6, 3, 5},
            {3, 4, 5, 2, 8, 6, 1, 7, 9}
        };

        // Copy the initial configuration to the board
        Array.Copy(initialConfiguration, board, initialConfiguration.Length);
    }

    public void PrintBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write($"{board[i, j]} ");
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main()
    {
        Sudoku sudoku = new Sudoku();
        sudoku.PrintBoard();
    }
}
