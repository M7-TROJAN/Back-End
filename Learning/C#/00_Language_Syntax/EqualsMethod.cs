using System;

namespace EqualsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string str1 = "Hello";
            string str2 = "Hello";
            string str3 = "World";

            // Using Equals method for string comparison
            bool areEqual1 = str1.Equals(str2);
            bool areEqual2 = str1.Equals(str3);

            Console.WriteLine($"str1 equals str2: {areEqual1}"); // Output: True
            Console.WriteLine($"str1 equals str3: {areEqual2}"); // Output: False

            // Using Equals method for custom class comparison
            Person person1 = new Person("Mahmoud", 25);
            Person person2 = new Person("Mahmoud", 25);
            Person person3 = new Person("Mattar", 30);

            bool areEqual3 = person1.Equals(person2);
            bool areEqual4 = person1.Equals(person3);

            Console.WriteLine($"person1 equals person2: {areEqual3}"); // Output: True
            Console.WriteLine($"person1 equals person3: {areEqual4}"); // Output: False
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // Override Equals method for custom class comparison
        public override bool Equals(object obj)
        {
            if (obj is Person otherPerson)
            {
                return this.Name == otherPerson.Name && this.Age == otherPerson.Age;
            }
            return false;
        }
    }
}
