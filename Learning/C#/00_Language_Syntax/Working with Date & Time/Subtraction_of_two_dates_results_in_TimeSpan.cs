using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime dt1 = new DateTime(2023, 2, 21);
            DateTime dt2 = new DateTime(2023, 2, 25);
            TimeSpan result = dt2.Subtract(dt1);

            Console.WriteLine(result.Days);

            Console.ReadKey();
        }
    }
}