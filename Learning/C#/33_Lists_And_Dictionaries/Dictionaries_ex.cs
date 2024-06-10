using System;
using System.Collections.Generic;

namespace Dictionaries
{
    public class Program
    {
        public static void Main()
        {
            // Input text to analyze
            string article = "Dot Net is a free, cross-platform, " +
                             "open-source developer platform for building many different types of applications." +
                             "With Dot Net you can use multiple languages and libraries to build for web and IOT";

            // Split the text into words
            string[] words = article.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Create a dictionary to store word frequencies
            Dictionary<string, int> wordFrequencies = new Dictionary<string, int>();

            // Count the frequency of each word
            foreach (string word in words)
            {
                // Update word frequency
                if (wordFrequencies.ContainsKey(word))
                {
                    wordFrequencies[word]++;
                }
                else
                {
                    wordFrequencies[word] = 1;
                }
            }

            // Print the dictionary of word frequencies
            PrintWordFrequencies(wordFrequencies);

            Console.WriteLine();

            // Search for a word
            Console.WriteLine("Enter a word to search: ");
            string searchWord = Console.ReadLine();

            if (wordFrequencies.ContainsKey(searchWord))
            {
                Console.WriteLine($"The word '{searchWord}' was found {wordFrequencies[searchWord]} times");
            }
            else
            {
                Console.WriteLine($"The word '{searchWord}' was not found");
            }

            Console.WriteLine();

            // Remove a word
            Console.WriteLine("Enter a word to remove: ");
            string removeWord = Console.ReadLine();

            if (wordFrequencies.ContainsKey(removeWord))
            {
                wordFrequencies.Remove(removeWord);
                Console.WriteLine($"The word '{removeWord}' was removed");
            }
            else
            {
                Console.WriteLine($"The word '{removeWord}' was not found");
            }

            Console.WriteLine();

            // Print the updated dictionary of word frequencies
            PrintWordFrequencies(wordFrequencies);

            Console.ReadKey();
        }

        // Print the dictionary of word frequencies
        public static void PrintWordFrequencies(Dictionary<string, int> wordFrequencies)
        {
            foreach (KeyValuePair<string, int> entry in wordFrequencies)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }
    }
}
