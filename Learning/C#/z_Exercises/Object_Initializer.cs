using System;

class Program
{
    static void Main(string[] args)
    {
        // Methods to create an object

        // Method 1: Default Constructor with Individual Property Assignments

        Employee emp1 = new Employee();
        emp1.Id = 1;
        emp1.Name = "Mahmoud Mattar";
        emp1.Address = "Cairo, Egypt";

        // Method 2: Object Initializer Syntax

        Employee emp2 = new Employee
        {
            Id = 2,
            Name = "Ali Mohamed",
            Address = "Alexandria, Egypt"
        };

        // Method 3: Constructor with Parameter and Object Initializer

        Employee emp3 = new Employee(3)
        {
            Name = "Rahma Ysser",
            Address = "Zagazig, Egypt"
        };

        // Display employee information
        Console.WriteLine(emp1.DisplayEmployee());
        Console.WriteLine(emp2.DisplayEmployee());
        Console.WriteLine(emp3.DisplayEmployee());
    }
}

public class Employee
{
    // Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    // Constructors
    public Employee() { }

    public Employee(int id)
    {
        Id = id;
    }

    // Method to display employee information
    public string DisplayEmployee()
    {
        return $"ID: {Id}, Name: {Name}, Address: {Address}";
    }
}
