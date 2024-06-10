using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace GenericListExample
{
    public class Program
    {
        public static void Main()
        {
            // Initialize countries
            var egypt = new Country("EGY", "Egypt");
            var india = new Country("IN", "India");
            var usa = new Country("US", "United States of America");
            var iraq = new Country("IQ", "Iraq");
            var jordan = new Country("JOR", "Jordan");

            // Create and populate the list of countries
            List<Country> countries = new List<Country> { egypt, india, usa, iraq, jordan };

            // Initial list of countries
            PrintCountries(countries);

            // Add a country (Time Complexity: O(1))
            countries.Add(new Country("SA", "Saudi Arabia"));
            PrintCountries(countries);

            // Insert a country at index 2 (Time Complexity: O(n))
            countries.Insert(2, new Country("UK", "United Kingdom"));
            PrintCountries(countries);

            // Remove a country (Time Complexity: O(n))
            countries.Remove(usa);
            PrintCountries(countries);

            // Remove a country at index 3 (Time Complexity: O(n))
            countries.RemoveAt(3);
            PrintCountries(countries);

            // Find a country (Time Complexity: O(n))
            var country = countries.Find(c => c.Name == "gg");
            if (country == null)
                Console.WriteLine("\nCountry not found");
            else
                Console.WriteLine($"\nFound country: {country}");

            // Sort the list (Time Complexity: O(n log n))
            countries.Sort();
            PrintCountries(countries);

            // Reverse the list (Time Complexity: O
            // Reverse the list (Time Complexity: O(n))
            countries.Reverse();
            PrintCountries(countries);

            // Clear the list (Time Complexity: O(n))
            countries.Clear();
            PrintCountries(countries);

            // Trim excess capacity (Time Complexity: O(n))
            countries.TrimExcess();
            PrintCountries(countries);

            // Convert array to list (Time Complexity: O(n))
            Country[] countriesArray = { egypt, india, usa, iraq, jordan };
            List<Country> countriesList = countriesArray.ToList();
            PrintCountries(countriesList);


            // IndexOf (Time Complexity: O(n))
            int index = countries.IndexOf(india);
            Console.WriteLine($"\nIndex of India: {index}");

            // AddRange (Time Complexity: O(n))
            var moreCountries = new List<Country>
            {
                new Country("FR", "France"),
                new Country("DE", "Germany")
            };
            countries.AddRange(moreCountries);
            PrintCountries(countries);

            // InsertRange (Time Complexity: O(n + m), where n is the number of elements before the insert, and m is the number of elements being inserted)
            countries.InsertRange(2, moreCountries);
            PrintCountries(countries);

            // RemoveRange (Time Complexity: O(n))
            countries.RemoveRange(2, 2);
            PrintCountries(countries);

            // RemoveAll (Time Complexity: O(n))
            countries.RemoveAll(c => c.Name.StartsWith("G"));
            PrintCountries(countries);
        }

        public static void PrintCountries(List<Country> countries)
        {
            Console.WriteLine("\nCountries:");
            Console.WriteLine($"Count: {countries.Count}"); // Number of elements in the list
            Console.WriteLine($"Capacity: {countries.Capacity}"); // Capacity of the list before resizing is required
            Console.WriteLine("----------");
            foreach (var country in countries)
            {
                Console.WriteLine(country);
            }
        }
    }

    public class Country : IComparable<Country>
    {
        public string ISOCode { get; set; }
        public string Name { get; set; }

        public Country(string isoCode, string name)
        {
            ISOCode = isoCode;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} - ({ISOCode})"; // e.g., India - (IN)
        }

        public int CompareTo(Country? other)
        {
            if (other == null) return 1; // Consider the current instance greater if other is null
            return String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
