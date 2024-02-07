using System;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the Stock class for Amazon
            Stock amazonStock = new Stock("Amazon");

            // Set the initial price of the stock
            amazonStock.Price = 100;

            // Subscribe to the OnPriceChanged event with the event handler Stock_OnPriceChanged
            amazonStock.OnPriceChanged += Stock_OnPriceChanged;

            // Simulate price changes
            amazonStock.ChangePriceBy(0.05m);
            amazonStock.ChangePriceBy(-0.02m);
            amazonStock.ChangePriceBy(0.00m);
            amazonStock.ChangePriceBy(0.07m);
            amazonStock.ChangePriceBy(0.03m);
            amazonStock.ChangePriceBy(-0.09m);
            amazonStock.ChangePriceBy(0.00m);
            amazonStock.ChangePriceBy(0.10m);

            Console.ReadKey();
        }

        // Event handler for the OnPriceChanged event
        private static void Stock_OnPriceChanged(Stock stock, decimal oldPrice)
        {
            string priceChangeDirection = DeterminePriceChangeDirection(stock.Price, oldPrice);

            // Display stock information and price change direction
            Console.WriteLine($"{stock.Name}: ${stock.Price} - {priceChangeDirection}");

            // Write the information to a text file
            WritePriceChangeToTextFile(stock.Name, oldPrice, stock.Price);

            // Reset console color to default
            Console.ResetColor();
        }

        // Determines the direction of price change and sets the console color accordingly
        private static string DeterminePriceChangeDirection(decimal newPrice, decimal oldPrice)
        {
            if (newPrice > oldPrice)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return "UP";
            }
            else if (newPrice < oldPrice)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return "Down";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                return "No Change";
            }
        }

        // Write the price change information to a text file
        private static void WritePriceChangeToTextFile(string stockName, decimal oldPrice, decimal newPrice)
        {
            string filePath = "PriceChanges.txt";
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = newPrice > oldPrice ? "Increase" : (newPrice < oldPrice ? "Decrease" : "No Change");

            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"Stock: {stockName}, Old Price: {oldPrice}, New Price: {newPrice}, Status: {status}, Date/Time: {timeStamp}");
            }
        }

    }

    // Delegate type for the Stock OnPriceChanged event
    public delegate void StockPriceChangeHandler(Stock stock, decimal oldPrice);

    // Stock class representing a stock with price-related functionality
    public class Stock
    {
        private string name;
        private decimal price;

        // Event for notifying subscribers about price changes
        public event StockPriceChangeHandler OnPriceChanged;

        // Constructor to initialize the Stock object with a name
        public Stock(string stockName)
        {
            this.name = stockName;
        }

        // Property to get the stock name
        public string Name => this.name;

        // Property for the stock price with event triggering on set
        public decimal Price
        {
            get => this.price;
            set
            {
                decimal oldPrice = this.price;
                this.price = Math.Round(value, 2);

                // Notify subscribers about the price change
                OnPriceChanged?.Invoke(this, oldPrice);
            }
        }

        // Method to change the stock price by a given percentage
        public void ChangePriceBy(decimal percent)
        {
            // Convert percent to a value between 0 and 1 if it's greater than or equal to 1 or it's a negative number
            if (percent >= 1 || percent <= -1)
            {
                percent = percent / 100;
            }

            // Calculate the new price
            decimal oldPrice = this.price;
            this.price += Math.Round(this.Price * percent, 2);

            // Check if there are subscribers before invoking the event
            // Notify subscribers about the price change
            
            // Using Explicit Null Check: This approach was commonly used before .NET 6.
            //if (OnPriceChanged != null)
            //    OnPriceChanged(this, oldPrice);
            
            // Using Null-Conditional Operator: Introduced in .NET 6, it simplifies null checks.
            OnPriceChanged?.Invoke(this, oldPrice);
        }
    }
}
