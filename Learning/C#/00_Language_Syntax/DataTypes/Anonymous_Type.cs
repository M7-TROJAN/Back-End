/*
anonymous means => مجهول
anonymous type  => نوع مجهول
In C#, an anonymous type is a type (class) without any name that can contain public read-only properties only. 
It cannot contain other members, such as fields, methods, events, etc.
*/

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //you don't specify any type here , automatically will be specified
            var student = new { Id = 20, FirstName = "Mahmoud", LastName = "Mattar" };

            Console.WriteLine("\nExample1:\n");
            Console.WriteLine(student.Id); //output: 20
            Console.WriteLine(student.FirstName); //output: Mahmoud
            Console.WriteLine(student.LastName); //output: Mattar

            //You can print like this:
            Console.WriteLine(student);

            //anonymous types are read-only
            //you cannot change the values of properties as they are read-only.

            // student.Id = 2;//Error: cannot change value
            // student.FirstName = "Ali";//Error: cannot change value

            //An anonymous type's property can include another anonymous type.
            var student2 = new
            {
                Id = 20,
                FirstName = "Mahmoud",
                LastName = "Mattar",
                Address = new { Id = 1, City = "Cairo", Country = "Egypt" }
            };

            Console.WriteLine("\nExample2:\n");
            Console.WriteLine(student2.Id);
            Console.WriteLine(student2.FirstName);
            Console.WriteLine(student2.LastName);

            Console.WriteLine(student2.Address.Id);
            Console.WriteLine(student2.Address.City);
            Console.WriteLine(student2.Address.Country);
            Console.WriteLine(student2.Address);

            Console.WriteLine(student2);

            Console.ReadKey();
        }
    }
}