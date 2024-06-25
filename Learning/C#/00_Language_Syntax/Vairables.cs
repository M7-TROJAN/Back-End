using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string name = "mahmoud";
            char Gender = 'M';
            int age = 25;
            bool isMuslim = true;
            string PhoneType = "Iphone";
            double PhonePrice = 25000;

            Console.WriteLine("name: {0}", name);
            Console.WriteLine("Gender: {0}", Gender);
            Console.WriteLine("Age: " + age);
            Console.WriteLine("Is Muslim?: " + isMuslim);
            Console.WriteLine("Phone Type: " + PhoneType);
            Console.WriteLine("Phone Price: {0:C0}", PhonePrice);

            Console.WriteLine("----------------------------------------");

            int x = 5, y = 6;
            double z = 10.5;

            Console.WriteLine("x = {0}\ny = {1}", x, y);

            Console.WriteLine("z = " + z);

            Console.WriteLine("x + y = {0}", (x + y));

            Console.ReadKey();
        }
    }
}