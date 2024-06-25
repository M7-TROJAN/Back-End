using System;

namespace Revision
{
    // Base class representing an animal
    public class Animal
    {
        // Method to simulate movement of the animal
        public void Move()
        {
            Console.WriteLine("Moving...");
        }
    }

    // Derived class representing an Eagle, which inherits from Animal
    public class Eagle : Animal
    {
        // Method to simulate flying of the Eagle
        public void Fly()
        {
            Console.WriteLine("Flying...");
        }
    }

    // Derived class representing a Falcon, which also inherits from Animal
    public class Falcon : Animal
    {
        // Method to simulate flying of the Falcon
        public void Fly()
        {
            Console.WriteLine("Flying...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Eagle
            var eagle = new Eagle();

            // Implicit downcasting: Assigning the Eagle instance to a base class reference
            Animal animal = eagle;

            // Attempting to cast Animal reference back to Falcon (InvalidCastException)
            //Falcon falcon = (Falcon)animal;

            // Avoiding exception by using 'as' keyword for casting
            // If the cast succeeds, falcon will refer to the same object as animal, otherwise it will be null
            Falcon falcon = animal as Falcon;

            // Check if the cast was successful
            if (falcon != null)
            {
                Console.WriteLine("Casting succeeded: animal is a Falcon.");
            }
            else
            {
                Console.WriteLine("Casting failed: animal is not a Falcon or is null.");
            }
        }
    }
}
