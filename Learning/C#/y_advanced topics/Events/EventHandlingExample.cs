using System;

namespace Revision
{
    class Program
    {
        public static int Count { get; private set; }

        static void Main(string[] args)
        {
            // Subscribe to the event
            Person.OnPersonCreate += Per_OnPersonCreate;

            // Create multiple instances of Person
            var per1 = new Person(1, "Mahmoud");
            var per2 = new Person(2, "Ahmed");
            var per3 = new Person(3, "Ali");
            var per4 = new Person(4, "Fatima");
            var per5 = new Person(5, "Layla");
        }

        // Event handler for the Person.OnPersonCreate event
        public static int Per_OnPersonCreate()
        {
            // Increment the Count property and display its value
            Console.WriteLine($"Total persons created: {++Count}");
            return Count;
        }
    }

    public delegate int OnPersonCreate();
    class Person
    {
        // Use a static event
        public static event OnPersonCreate? OnPersonCreate;

        public int Id { get; private set; }
        public string Name { get; private set; }

        public Person(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            // Invoke the event
            OnPersonCreate?.Invoke();
        }
    }
}
