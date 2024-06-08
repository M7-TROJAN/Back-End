using System;

namespace ReflectionDemo
{
    class Program
    {
        static void Main()
        {
            var type = typeof(Goon);
            Console.WriteLine($"Type: {type}");
            // Demonstrates how to instantiate an object of a class using reflection.

            do
            {
                Console.Write("\nEnter Enemy Name (Goon, Agar, Pixa): ");
                string enemyName = Console.ReadLine();
                object obj = null;

                try
                {
                    // Get the namespace dynamically and construct the fully qualified type name.
                    var namespaceName = typeof(Program).Namespace;
                    var fullyQualifiedTypeName = $"{namespaceName}.{enemyName}";

                    // Get the assembly name dynamically and ensure it's fully qualified.
                    var assemblyName = typeof(Program).Assembly.GetName().Name;

                    // Use Activator to create an instance of the enemy class.
                    var enemy = Activator.CreateInstance(assemblyName, fullyQualifiedTypeName);
                    obj = enemy.Unwrap();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                // Determine the type of the created object and handle it accordingly.
                switch (obj)
                {
                    case Goon goon:
                        Console.WriteLine(goon);
                        break;
                    case Agar agar:
                        Console.WriteLine(agar);
                        break;
                    case Pixa pixa:
                        Console.WriteLine(pixa);
                        break;
                    default:
                        Console.WriteLine("Invalid enemy name.");
                        break;
                }

            } while (true);
        }
    }

    /// <summary>
    /// Represents a monster in the game named Goon.
    /// </summary>
    public class Goon
    {
        public override string ToString()
        {
            return "{\n I am Goon\n" +
                   " Speed: 20,  HitPower: 13,  Strength: 7\n}";
        }
    }

    /// <summary>
    /// Represents a monster in the game named Agar.
    /// </summary>
    public class Agar
    {
        public override string ToString()
        {
            return "{\n I am Agar\n" +
                   " Speed: 21,  HitPower: 18,  Strength: 10\n}";
        }
    }

    /// <summary>
    /// Represents a monster in the game named Pixa.
    /// </summary>
    public class Pixa
    {
        public override string ToString()
        {
            return "{\n I am Pixa\n" +
                   " Speed: 25,  HitPower: 20,  Strength: 15\n}";
        }
    }
}
