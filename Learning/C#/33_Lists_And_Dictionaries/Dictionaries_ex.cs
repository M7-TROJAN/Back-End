using System;
using System.Collections.Generic;

namespace Dictionaries
{
    public class Program
    {
        public static void Main()
        {
            var article = "Dot Net is a free, cross-platform, " +
                "open-source developer platform for building many different types of applications." +
                "With Dot Net you can use multiple languages and libraries to build for web and IOT";

            Dictionary<char, List<string>> lettersDictionary = new Dictionary<char, List<string>>();

            PrintDictionary(lettersDictionary);

            foreach (var word in article.Split(' '))
            {
                char firstLetter = word[0];

                // Ensure the dictionary contains an entry for the first letter
                if (!lettersDictionary.ContainsKey(firstLetter))
                {
                    lettersDictionary[firstLetter] = new List<string>();
                }

                // Add the word to the list corresponding to its first letter
                lettersDictionary[firstLetter].Add(word);
            }

            PrintDictionary(lettersDictionary);
        }

        public static void PrintDictionary<TKey, TValue>(Dictionary<TKey, List<TValue>> dictionary)
        {
            
            if (dictionary == null || dictionary.Count == 0)
            {
                Console.WriteLine("Dictionary is null.");
                return;
            }

            foreach (var pair in dictionary)
            {
                Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}");
            }
        }
    }
}
