
using System;

class Program
{
    static void Main()
    {

        // Convert integer to binary representation using BitConverter

        int number = 10;

        // Get the bytes representing the integer
        var bytes = BitConverter.GetBytes(number);

        Console.WriteLine("Binary representation of the integer:");

        // Iterate through each byte and print its binary representation
        foreach (var b in bytes)
        {
            // Convert byte to binary string and ensure it's 8 digits long
            var binary = Convert.ToString(b, 2).PadLeft(8, '0');
            Console.WriteLine(binary);
        }

        Console.WriteLine("________________________________________________________");

        // Convert characters in a string to ASCII, binary, and hexadecimal representations

        string name = "Mahmoud";

        // Convert the string to an array of characters
        char[] letters = name.ToCharArray();

        Console.WriteLine("Character details:");

        // Iterate through each character and print its ASCII, binary, and hexadecimal representations
        foreach (var letter in letters)
        {
            // Get ASCII value of the character
            int ascii = (int)letter;

            // Convert character to binary string and ensure it's 8 digits long
            var binary = Convert.ToString(letter, 2).PadLeft(8, '0');

            // Create an output string with character details
            var output = $"Letter: {letter}, ASCII: {ascii}, Binary: {binary}, Hexadecimal: {ascii:x}";

            Console.WriteLine(output);
        }

        Console.WriteLine("________________________________________________________");

        // Convert hexadecimal strings to characters

        string[] hexValues = { "4d", "61", "68", "6d", "6f", "75", "64" };

        Console.WriteLine("Characters from hexadecimal values:");

        // Iterate through each hexadecimal string and convert it to a character
        foreach (var hex in hexValues)
        {
            // Convert hexadecimal string to integer
            int value = Convert.ToInt32(hex, 16);

            // Convert integer to character
            char character = (char)value;

            // Print the character
            Console.WriteLine(character);
        }
    }
}
