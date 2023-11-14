/*

*/

using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var person = new
            {
                id = 1,
                firstName = "Mahmoud",
                lastName = "Mattar",
                Email = "Matterm74@gmail.com",
                phone = "01018630762",
                address = new { id = 5, city = "Cairo", country = "Egypt" }
            };

            Console.WriteLine("FirstName: " + person.firstName);
            Console.WriteLine("lastName: " + person.lastName);
            Console.WriteLine($"FullName: {person.firstName} {person.lastName}");
            Console.WriteLine("Email: " + person.Email);
            Console.WriteLine("phone: " + person.phone);
            Console.WriteLine("address-ID: " + person.address.id);
            Console.WriteLine("address-city: " + person.address.city);
            Console.WriteLine("address-country: " + person.address.country);

            Console.WriteLine("----------------------------------------------");

            Console.WriteLine(person);

            Console.ReadLine();
        }
    }
}