using System;

namespace Enumerations
{
    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    class Program
    {
        static void Main()
        {
            // Select a month
            Month selectedMonth = Month.April;

            // Display the value of the selected month
            Console.WriteLine($"Value of selected month: {(int)selectedMonth}\n");

            // Display the name of the selected month
            Console.WriteLine($"Name of selected month: {selectedMonth}\n");

            // Prompt user to enter a month
            Console.Write("Enter a month: ");
            var inputValue = Console.ReadLine();

            // Try to parse the input as a month enum
            if (Enum.TryParse(inputValue, true, out Month parsedMonth))
            {
                Console.WriteLine($"Parsed month: {parsedMonth}\n");
            }
            else
            {
                Console.WriteLine("Invalid input\n");
            }

            // Compare the selected month with April
            Console.WriteLine($"Comparison result with April: {selectedMonth.CompareTo(Month.April)} \n");

            // Check if the selected month is defined in the Month enum
            if (Enum.IsDefined(typeof(Month), selectedMonth))
            {
                Console.WriteLine($"Yes, {selectedMonth} is a valid month.\n");
            }
            else
            {
                Console.WriteLine("No\n");
            }

            // Display all months using Enum.GetValues
            Console.WriteLine("\nAll months:\n");
            foreach (Month month in Enum.GetValues(typeof(Month)))
            {
                Console.WriteLine($"{month}");
            }

            // Display all month names using Enum.GetNames
            Console.WriteLine("All month names:");
            foreach (var monthName in Enum.GetNames(typeof(Month)))
            {
                Console.WriteLine($"{monthName}");
            }
        }
    }
}
