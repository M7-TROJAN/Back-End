// loose coupling example 
// TIGHT COUPLING MEANS ONE CLASS IS DEPENDENT ON ANOTHER GLASS
// LOOSE COUPLING MEANS ONE CLASS IS DEPENDENT ON INTERFACE RATHER THAN CLASS
using System;

namespace InterfaceExample
{
    // Interface defining the contract for payment methods
    public interface IPayment
    {
        void Pay(decimal amount);
    }

    // Class representing a cash payment method
    public class Cash : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Cash Payment: {Math.Round(amount, 2):C0}");
        }
    }

    // Class representing a debit card payment method
    public class Debit : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Debit Payment: {Math.Round(amount, 2):C0}");
        }
    }

    // Class representing a MasterCard payment method
    public class MasterCard : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"MasterCard Payment: {Math.Round(amount, 2)::C0}");
        }
    }

    // Class representing a Visa payment method
    public class Visa : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Visa Payment: {Math.Round(amount, 2)::C0}");
        }
    }


    // Class representing a cashier that processes payments
    public class Cashier
    {
        private IPayment _payment; // Private field to hold the payment method

        // Constructor to initialize the Cashier with a specific payment method
        public Cashier(IPayment payment)
        {
            _payment = payment; // Assign the injected payment method to the private field
        }

        // Method to process the payment
        public void CheckOut(decimal amount)
        {
            _payment.Pay(amount); // Call the Pay method on the assigned payment method
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a Cashier object with a Cash payment method and process a payment
            Cashier cashier = new Cashier(new Cash());
            cashier.CheckOut(100);

            // Create a Cashier object with a Visa payment method and process a payment
            cashier = new Cashier(new Visa());
            cashier.CheckOut(9954.67m);

            // Create a Cashier object with a MasterCard payment method and process a payment
            cashier = new Cashier(new MasterCard());
            cashier.CheckOut(96849.1m);

            // Create a Cashier object with a Debit payment method and process a payment
            cashier = new Cashier(new Debit());
            cashier.CheckOut(77854.20m);
        }
    }
}

/*
This code demonstrates the concept of loose coupling using interfaces in C#. 
Loose coupling is a design principle that promotes independence and flexibility between components in a system.

In this example:

1. **Cashier Class**: This class represents a cashier who can process payments. 
It has a constructor that takes an object implementing the `IPayment` interface. 
The `CheckOut` method in the `Cashier` class calls the `Pay` method on the injected `IPayment` object to process the payment.

2. **IPayment Interface**: This interface defines the contract for payment methods. 
It has a single method `Pay` that takes a `decimal` amount as input.

3. **Cash, Debit, MasterCard, Visa Classes**: These classes implement the `IPayment` interface, 
providing concrete implementations of the `Pay` method for different payment methods. 
Each class handles payment processing specific to its payment type (e.g., Cash, Debit, MasterCard, Visa).

4. **Main Method**: In the `Main` method, instances of the `Cashier` class are created with different payment types injected into them.
Each `Cashier` instance then processes payments using the respective payment method.

**Explanation:**

- Loose Coupling: The `Cashier` class depends only on the `IPayment` interface, not on specific payment implementations.
This allows different payment methods to be easily swapped without modifying the `Cashier` class. 
It promotes flexibility and scalability in the system.

- Dependency Injection: The `Cashier` class receives an instance of an `IPayment` implementation via its constructor.
This is an example of dependency injection, where dependencies are provided to a class from external sources rather than created internally.
It makes the `Cashier` class more reusable and testable.

- Interface Polymorphism: The `Cashier` class interacts with payment objects through the `IPayment` interface.
This allows different payment implementations to be treated uniformly, enabling polymorphic behavior.
The `Pay` method is called on different payment objects,
but the specific implementation is determined at runtime based on the injected object type.

- Example Execution: In the `Main` method, various payment scenarios are demonstrated 
by creating `Cashier` instances with different payment methods injected. 
Each `Cashier` object processes payments using the appropriate payment method, resulting in different output 
messages indicating the payment type and amount.

Overall, this example illustrates how interfaces can be used to achieve loose coupling and facilitate modular, 
extensible designs in C# applications. 
*/
