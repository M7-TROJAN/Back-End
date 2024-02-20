
using System;
using System.Collections.Generic; // Import the `System.Collections.Generic` namespace to use the List class
using System.Collections; // Import the `System.Collections` namespace to use the ArrayList class

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Using List<T> to store Person objects
            var list = new List<Person>(); 

            // Adding Person objects to the list
            list.Add(new Person(name: "Mahmoud", id: 1));
            list.Add(new Person(name: "Ahmed", id: 2));
            list.Add(new Person(name: "Ali", id: 3));

            // Displaying items in the List
            Console.WriteLine("List items: ");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"\nLength: {list.Count} Items");
            Console.WriteLine($"Empty?: {list.Count == 0}");


            // Using ArrayList to store different types of objects
            var arrayList = new ArrayList();

            // Adding various types of objects to the ArrayList
            arrayList.Add(new Person(name: "Mahmoud", id: 1)); // Person
            arrayList.Add(2); // Integer
            arrayList.Add("Hello"); // String
            arrayList.Add(true); // Boolean
            arrayList.Add(new { FName = "Ahmed", LName = "Ali" }); // Anonymous type

            // Displaying items in the ArrayList
            Console.WriteLine("\n\nArrayList items: ");
            foreach (var item in arrayList)
            {
                Console.WriteLine($"{item} - {item.GetType()}");
            }
            Console.WriteLine($"\nLength: {arrayList.Count} Items");
            Console.WriteLine($"Empty?: {arrayList.Count == 0}");
        }
    }

    // A simple class representing a person with ID and Name
    public class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }

        // Default constructor
        public Person()
        {
            this.Name = string.Empty;
        }

        // Parameterized constructor
        public Person(int id, string name)
        {
            this.Name = name;
            this.Id = id;
        }

        // Override ToString method to display ID and Name
        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}";
        }
    }
}
