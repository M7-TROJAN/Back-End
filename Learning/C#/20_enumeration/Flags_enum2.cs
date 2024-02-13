using System;

namespace Enumeration
{
    class Program
    {
        static void Main()
        {
            var day = Day.Saturday | Day.Sunday;

            // Check if the combined days include a weekend
            if (day.HasFlag(Day.Weekend))
            {
                Console.WriteLine("Enjoy Your Weekend!");
            }
        }
    }

    [Flags]
    enum Day
    {
        None         = 0b_0000_0000, // 0
        Monday       = 0b_0000_0001, // 1
        Tuesday      = 0b_0000_0010, // 2
        Wednesday    = 0b_0000_0100, // 4
        Thursday     = 0b_0000_1000, // 8
        Friday       = 0b_0001_0000, // 16
        Saturday     = 0b_0010_0000, // 32
        Sunday       = 0b_0100_0000, // 64
        Weekend      = Saturday | Sunday, // Combine Saturday and Sunday -> Saturday + Sunday  32 + 64 = 96
        BusinessDays = Monday | Tuesday | Wednesday | Thursday | Friday,
    }
}
