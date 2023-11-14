/*
In C#, a string is a series of characters that is used to represent text. 
It can be a character, a word or a long passage surrounded with the double quotes ".
*/


using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string S1 = "Mahmoud Mattar";

            // Get the length of the string
            Console.WriteLine(S1.Length);

            // Extract and print a substring of length 5 starting from index 2
            //this will take 5 characters staring from position 2
            Console.WriteLine(S1.Substring(2, 5));

            // Convert the string to lowercase
            Console.WriteLine(S1.ToLower());

            // Convert the string to uppercase
            Console.WriteLine(S1.ToUpper());

            // Access and print the character at index 2
            Console.WriteLine(S1[2]);

            // Insert "KKKK" at position 3
            Console.WriteLine(S1.Insert(3, "KKKK"));

            // Replace occurrences of 'm' with '*'
            // It won't replace just the first occurrence; it will replace every occurrence.
            Console.WriteLine(S1.Replace("m", "*"));

            // Find the index of the first occurrence of 'm'
            Console.WriteLine(S1.IndexOf("m"));

            // Check if the string contains the letter 'm'
            Console.WriteLine(S1.Contains("m"));

            // Check if the string contains the letter 'x'
            Console.WriteLine(S1.Contains("x"));

            // Find the index of the last occurrence of 'm'
            Console.WriteLine(S1.LastIndexOf("m"));

            string S2 = "Ali,Ahmed,Khalid";

            // Split the string into an array based on the comma delimiter
            string[] NamesList = S2.Split(',');

            // Access and print elements of the array
            Console.WriteLine(NamesList[0]);
            Console.WriteLine(NamesList[1]);
            Console.WriteLine(NamesList[2]);

            string S3 = "  Mahmoud  ";

            // Remove leading and trailing spaces
            Console.WriteLine(S3.Trim());

            // Remove leading spaces
            Console.WriteLine(S3.TrimStart());

            // Remove trailing spaces
            Console.WriteLine(S3.TrimEnd());

            Console.ReadKey();
        }
    }
}


