using System;

namespace Learning
{
    internal class Person
    {
        // Fields should be private for encapsulation
        private string _firstName;
        private string _lastName;

        public Person() : this(string.Empty, string.Empty) { }

        public Person(string fName, string lName)
        {
            _firstName = fName;
            _lastName = lName;
        }

        // Properties to access private fields
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string GetFullName()
        {
            return $"{_firstName} {_lastName}";
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            // Creating instances of the Person class
            Person person1 = new Person();
            person1.FirstName = "Mahmoud";
            person1.LastName = "Mattar";

            Person person2 = new Person("Ali", "Mohamed");

            // Output for Person1
            Console.WriteLine("Person1");
            Console.WriteLine($"First Name: {person1.FirstName}");
            Console.WriteLine($"Last Name: {person1.LastName}");
            Console.WriteLine($"Full Name: {person1.GetFullName()}");

            // Output for Person2
            Console.WriteLine("\nPerson2");
            Console.WriteLine($"First Name: {person2.FirstName}");
            Console.WriteLine($"Last Name: {person2.LastName}");
            Console.WriteLine($"Full Name: {person2.GetFullName()}");

            Console.ReadKey();
        }
    }
}
