using System;
using System.Reflection;

namespace ReflectionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the assembly file (DLL file)
            var path = @"C:\Users\matte\OneDrive\Desktop\pro_vs\Learning\ConsoleApp1\MOE.dll";

            // Load the assembly from the specified path
            var assembly = Assembly.LoadFile(path);
            var assemblyName = assembly.GetName().Name;
            Console.WriteLine($"Assembly Name: {assemblyName}");

            // List all types and their methods from the assembly
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                Console.WriteLine($"\nType: {type.FullName}");
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    Console.WriteLine($"  Method: {method.Name}");
                }
            }

            // Get a specific type from the assembly
            var typeToInstantiate = assembly.GetType("MOE.University");
            if (typeToInstantiate != null)
            {
                // Create an instance of the type
                object obj = Activator.CreateInstance(typeToInstantiate);

                // Print the instance if it's created successfully
                if (obj != null)
                {
                    Console.WriteLine($"\nCreated instance of type: {typeToInstantiate.FullName}");
                    Console.WriteLine(obj);
                }
                else
                {
                    Console.WriteLine($"\nFailed to create an instance of type: {typeToInstantiate.FullName}");
                }
            }
            else
            {
                Console.WriteLine("\nType 'MOE.University' not found in the assembly.");
            }

            Console.ReadKey();
        }
    }
}
