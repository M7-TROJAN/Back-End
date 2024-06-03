using System;

// In C#, an interface is similar to abstract class. However, unlike abstract classes, all methods of an interface are fully abstract (method without body).
public interface IPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }

    void Introduce();

    void Print();

    string To_String();

}

public abstract class Person : IPerson

{

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public abstract void Introduce();

    public void SayGoodbye()
    {
        Console.WriteLine("Goodbye!");
    }

    public void Print()
    {
        Console.WriteLine("Hi I'm the print method");
    }

    public string To_String()
    {
        return "Hi this is the complete string....";

    }

    public void SedEmail()
    {
        Console.WriteLine("Email Sent :-)");

    }

}

public class Employee : Person
{
    public int EmployeeId { get; set; }

    public override void Introduce()
    {
        Console.WriteLine($"Hi, my name is {FirstName} {LastName}, and my employee ID is {EmployeeId}.");
    }
}

public class Program
{
    public static void Main()
    {
        //You cannot create an object of an Interface, you can only Implement it.
       // IPerson Person1 = new IPerson();

        Employee employee = new Employee();
        employee.FirstName = "Mahmoud";
        employee.LastName = "Mattar";
        employee.EmployeeId = 123;
        employee.Introduce(); // Output: "Hi, my name is Mahmoud Mattar, and my employee ID is 123."
        employee.SayGoodbye(); // Output: "Goodbye!"
        employee.Print();
        employee.SedEmail();

        Console.ReadKey();

    }
}
