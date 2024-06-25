// Local method is a method defined within the scope of another method. In C#, local methods are declared inside a containing method, 
// allowing for encapsulation of logic specific to that method. They enhance readability and maintainability by keeping related functionality 
// together, limiting its visibility to the containing method only.

// In this example, the PrintEvens method contains a local method IsEven, which checks if a given number is even. 
// This encapsulation helps organize the code and ensures that the IsEven logic is only accessible within the PrintEvens method. 
// Local methods can access variables from their containing method, providing a convenient way to share state.


using System;

class Program
{
    static void Main(string[] args)
    {
        // Example of using the PrintEvens method in the Demo class

        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Call the PrintEvens method to print even numbers from the array
        Demo.PrintEvens(numbers);
    }
}

class Demo
{
    // Method to print even numbers from an array
    public static void PrintEvens(int[] numbers)
    {
        // Check if the array is empty
        if (numbers.Length == 0)
        {
            Console.WriteLine("The array is empty.");
            return;
        }

        Console.Write("Even numbers: { ");

        // Iterate through each number in the array
        foreach (var num in numbers)
        {
            // Check if the number is even using the IsEven local method
            if (IsEven(num))
            {
                // Print the even number
                Console.Write(num + " ");
            }
        }

        Console.WriteLine("}");

        // Local method to check if a number is even
        bool IsEven(int num) => (num & 1) == 0;
    }

}
