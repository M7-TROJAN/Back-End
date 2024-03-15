using System;
using System.Reflection;
using DemoLib;

namespace Assemblies
{
    class Program
    {
        static void Main()
        {
            // Method 1: Retrieving assembly information from a specified type
            var type = typeof(Employee);
            var assembly = type.Assembly;
            Console.WriteLine($"Assembly Information from Type:\n{assembly}\n");

            // Method 2: Retrieving assembly information from the currently executing assembly
            var assembly2 = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Assembly Information from Executing Assembly:\n{assembly2}\n");

            // Additional example: Accessing assembly information of DateTime
            Console.WriteLine($"Assembly Information of DateTime:\n{typeof(DateTime).Assembly}\n");

            // Calling a method from the DemoLib project to trace assembly information
            Demo.Trace();

            Console.ReadKey();
        }
    }

    class Employee
    {

    }
}