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
        private decimal _amount;

        public Dollar(decimal amount)
        {
            Amount = amount; // Utilize the property for consistency
        }

        // Property with Getter and Setter
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = ProcessValue(value); }
        }

        private decimal ProcessValue(decimal value) => value <= 0 ? 0 : Math.Round(value, 2);
    }
}
