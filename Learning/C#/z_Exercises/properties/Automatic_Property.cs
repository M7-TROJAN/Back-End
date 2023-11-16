using System;

namespace CAProperty
{
    class Program
    {
        static void Main(string[] args)
        {
            Dollar dollar = new Dollar(1.99m);

            Console.WriteLine($"{dollar.Amount:C}");
        }
    }

    public class Dollar
    {
        // Automatic Property
        public decimal Amount { get; set; }

        public Dollar(decimal amount)
        {
            Amount = amount;
        }
    }
}