using System;

namespace Overriding
{
    // In C#, every class implicitly inherits from a base class, even if it's not explicitly stated.
    // If no other base class is specified, the class automatically inherits from the `Object` class.
    // The `Object` class is the root of the .NET type hierarchy and serves as the ultimate base class for all objects in C#.
    // This means that every class in a C# program inherently inherits some fundamental functionalities and characteristics from the `Object` class,
    // such as the ability to call methods like `ToString()`, `Equals()`, and `GetHashCode()`.
    // Thus, the `Object` class is the parent of all .NET types and forms the foundation of object-oriented programming in C#.

    // Base class representing a Person
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }

        // Overrides the ToString method to return a formatted string representation of the Person object
        public override string ToString()
        {
            return $"{ID} - {Name}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // Creating an instance of Person
            Person person = new Person { ID = 1, Name = "Mahmoud" };

            // Calling the overridden ToString method
            Console.WriteLine(person); // Output: 1 - Mahmoud

            // Implicitly calling ToString on an object
            object obj = person;
            Console.WriteLine(obj); // Output: 1 - Mahmoud

            // Demonstrating how ToString works with string interpolation
            Console.WriteLine($"Person details: {person}"); // Output: Person details: 1 - Mahmoud

            // Demonstrating how ToString works with string concatenation
            string details = "Details: " + person;
            Console.WriteLine(details); // Output: Details: 1 - Mahmoud
        }
    }
}
