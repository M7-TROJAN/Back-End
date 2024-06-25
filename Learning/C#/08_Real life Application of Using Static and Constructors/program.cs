using System;

class Person
{
    // Properties of the Person class
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    // Private constructor to enforce the use of static factory methods
    private Person(int id, string name, int age, string userName, string password)
    {
        Id = id;
        Name = name;
        Age = age;
        UserName = userName;
        Password = password;
    }

    // Static factory method to find a person by ID
    public static Person Find(int id)
    {
        // Simulating database query
        if (id == 10)
        {
            return new Person(id, "Mahmoud Mattar", 25, "matt74", "123456");
        }

        return null;
    }

    // Static factory method to find a person by UserName and Password
    public static Person Find(string userName, string password)
    {
        // Simulating database query
        if (userName == "matt74" && password == "123456")
        {
            return new Person(10, "Mahmoud Mattar", 25, "matt74", "123456");
        }

        return null;
    }
}

class Program
{
    // Method to get the current formatted date and time
    static string GetFormattedDate()
    {
        DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("dddd MM/dd/yyyy h:mm:ss tt");
        return formattedDate;
    }

    // Main entry point of the program
    static void Main()
    {
        Console.WriteLine(GetFormattedDate());

        // Find a person by ID
        Person personById = Person.Find(10);
        DisplayPersonDetails(personById);

        // Find a person by UserName and Password
        Person personByCredentials = Person.Find("matt74", "123456");
        DisplayPersonDetails(personByCredentials);
    }

    // Helper method to display person details or a message if person is not found
    static void DisplayPersonDetails(Person person)
    {
        if (person != null)
        {
            Console.WriteLine($"ID: {person.Id}");
            Console.WriteLine($"Name: {person.Name}");
            Console.WriteLine($"Age: {person.Age}");
            Console.WriteLine($"UserName: {person.UserName}");
            Console.WriteLine($"Password: {person.Password}");
        }
        else
        {
            Console.WriteLine("Could not find the person.");
        }
    }
}




// Instead of using a private constructor, we can initialize properties directly in the class and make the setters private if needed. 
// like this:
    // public int Id { get; private set; }
    // public string Name { get; private set; }
    // public int Age { get; private set; }
    // public string UserName { get; private set; }
    // public string Password { get; private set; }

