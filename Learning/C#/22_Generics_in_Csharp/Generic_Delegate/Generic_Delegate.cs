
using System;
using System.Text; // Import StringBuilder
using System.Collections.Generic;

namespace GenericDelegate
{
    // Define a generic delegate named Filter that takes a single parameter of type T and returns a bool
    public delegate bool Filter<T>(T obj);

    class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public Person(string fName, string lName)
        {
            this.FirstName = fName;
            this.LastName = lName;
        }

        // Override ToString method to provide custom string representation of Person object
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            // Create an array of integers
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            // Filter and print numbers greater than or equal to 7
            PrintFilteredItems(numbers, (num) => num >= 7);

            // Create an array of strings
            string[] names = { "mahmoud", "MAHMOUD" };

            // Filter and print names that are uppercase
            PrintFilteredItems(names, (name) => name.Equals(name.ToUpper()));

            // Filter and print names that are lowercase
            PrintFilteredItems(names, (name) => name.Equals(name.ToLower()));

            // Create an array of booleans
            bool[] booleans = { true, false, true, false, false };

            // Filter and print boolean values that are true
            PrintFilteredItems(booleans, (b) => b is true);

            // Create an array of Person objects
            var people = new List<Person>
            {
                new Person("Mahmoud", "Mattar"),
                new Person("Ali", "Emad"),
                new Person("Rahma", "Yasser")
            };

            // Filter and print Person objects that match the specified condition
            PrintFilteredItems(people, (p) => p.FirstName != "Mahmoud" && p.LastName != "Mattar");
        }

        // Method to print items of an array based on a filter
        public static void PrintFilteredItems<T>(IEnumerable<T> items, Filter<T> filter)
        {
            StringBuilder result = new StringBuilder(); // Use StringBuilder for efficient string concatenation
            foreach (var item in items)
            {
                // If the item passes the filter condition, add it to the result
                if (filter(item))
                {
                    result.Append($"{item}, ");
                }
            }
            string filteredItems = result.ToString().TrimEnd(' ', ',');

            // Print the result, trimming the trailing space and comma
            Console.WriteLine($"Filtered items: {{{filteredItems}}}");
        }
    }
}
