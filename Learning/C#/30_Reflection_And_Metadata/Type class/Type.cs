using System;
using System.Reflection;

namespace ReflectionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example of obtaining type information at runtime and compile time
            Type t1 = DateTime.Now.GetType(); // at runtime
            Console.WriteLine(t1); // Output: System.DateTime

            Type t2 = typeof(DateTime); // at compile time
            Console.WriteLine(t2); // Output: System.DateTime

            // Display various properties of the DateTime type
            Console.WriteLine($"FullName : {t1.FullName}"); // Output: System.DateTime
            Console.WriteLine($"Namespace: {t1.Namespace}"); // Output: System
            Console.WriteLine($"Name     : {t1.Name}"); // Output: DateTime
            Console.WriteLine($"BaseType : {t1.BaseType}"); // Output: System.ValueType

            Console.WriteLine();

            // Display additional type properties
            Console.WriteLine($"IsPublic    : {t1.IsPublic}"); // Output: True
            Console.WriteLine($"Assembly    : {t1.Assembly}"); // Output: System.Private.CoreLib, Version=....
            Console.WriteLine($"IsClass     : {t1.IsClass}"); // Output: False
            Console.WriteLine($"IsValueType : {t1.IsValueType}"); // Output: True
            Console.WriteLine($"IsAbstract  : {t1.IsAbstract}"); // Output: False
            Console.WriteLine($"IsSealed    : {t1.IsSealed}"); // Output: True

            Console.WriteLine();

            // Example of obtaining type information for a 2D array of int
            Type t3 = typeof(int[,]); // 2D array of int
            Console.WriteLine($"Name: {t3.Name}"); // Output: Int32[,]
            Console.WriteLine($"FullName: {t3.FullName}"); // Output: System.Int32[,]
            Console.WriteLine($"IsArray: {t3.IsArray}"); // Output: True
            Console.WriteLine($"IsValueType: {t3.IsValueType}"); // Output: False
            Console.WriteLine($"IsByRef: {t3.IsByRef}"); // Output: False
            Console.WriteLine($"IsPointer: {t3.IsPointer}"); // Output: False
            Console.WriteLine($"IsPrimitive: {t3.IsPrimitive}"); // Output: False
            Console.WriteLine($"IsEnum: {t3.IsEnum}"); // Output: False
            Console.WriteLine($"IsClass: {t3.IsClass}"); // Output: True

            Console.WriteLine();

            // Example of reflecting nested types and creating instances
            Type[] nestedTypes = typeof(Employee).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var nestedType in nestedTypes)
            {
                Console.WriteLine(nestedType.Name);
                var instance = Activator.CreateInstance(nestedType);
                Console.WriteLine(instance);
            }

            Console.WriteLine();

            // Example of reflecting fields of the Employee class
            FieldInfo[] fields = typeof(Employee).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                Console.WriteLine(field.Name);
            }

            Console.WriteLine();

            // Creating instances of nested types using reflection
            Type fullTimeType = typeof(Employee).GetNestedType("FullTimeEmployee", BindingFlags.NonPublic | BindingFlags.Public);
            Type partTimeType = typeof(Employee).GetNestedType("PartTimeEmployee", BindingFlags.NonPublic);

            var fullTimeInstance = Activator.CreateInstance(fullTimeType);
            var partTimeInstance = Activator.CreateInstance(partTimeType);

            Console.WriteLine(fullTimeInstance);
            Console.WriteLine(partTimeInstance);

            Console.WriteLine();

            // Example of reflecting implemented interfaces
            Type intType = typeof(int);
            Type[] interfaces = intType.GetInterfaces();
            foreach (var iface in interfaces)
            {
                Console.WriteLine(iface.Name);
            }

            Console.ReadKey();
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public class FullTimeEmployee
        {
            public int Id { get; set; }
            public string Name { get; set; } = "Full Time Employee Name";
            public string Department { get; set; } = "Full Time Employee Department";

            public override string ToString()
            {
                return $"{Id} - {Name} - {Department}";
            }
        }

        private class PartTimeEmployee
        {
            public int Id { get; set; }
            public string Name { get; set; } = "Part Time Employee Name";
            public string Department { get; set; } = "Part Time Employee Department";

            public override string ToString()
            {
                return $"{Id} - {Name} - {Department}";
            }
        }
    }

    public class MyClass
    {
        private string message;
        private int number;

        public MyClass()
        {
            message = "Default Message";
            number = 0;
        }

        public MyClass(string message, int number)
        {
            this.message = message;
            this.number = number;
        }

        public override string ToString()
        {
            return $"Message: {message}, Number: {number}";
        }
    }
}
