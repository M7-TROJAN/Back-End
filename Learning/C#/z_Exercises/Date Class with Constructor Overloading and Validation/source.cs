using System;

class Program
{
    static void Main(string[] args)
    {
        // Creating instances of the Date class
        Date d1 = new Date(29, 2, 2020);
        Console.WriteLine(d1.GetDate()); // Output: 29/02/2020

        Date d2 = new Date(2022);
        Console.WriteLine(d2.GetDate()); // Output: 01/01/2022

        Date d3 = new Date(2, 2023);
        Console.WriteLine(d3.GetDate()); // Output: 01/02/2022
    }
}

public class Date
{
    // Note:
    //      The 'readonly' field can only be modified within the constructor; once set, it cannot be changed elsewhere in the class.
    
    private static readonly int[] daysToMonth365 = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    private static readonly int[] daysToMonth366 = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    private readonly int _day;
    private readonly int _month;
    private readonly int _year;

    // Remember ya sadeky Static methods should be named in PascalCase
    private static bool IsValidYear(int year) => (year >= 1 && year <= 9999);
    private static bool IsLeapYear(int year) => (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    private static bool IsValidMonth(int month) => (month >= 1 && month <= 12);

    private static bool IsValidDate(int day, int month, int year)
    {
        if (IsValidYear(year) && IsValidMonth(month))
        {
            int[] days = IsLeapYear(year) ? daysToMonth365 : daysToMonth366;
            return (day >= 1 && day <= days[month]);
        }
        return false;
    }

    public Date(int day, int month, int year)
    {
        // Use guard clauses for better readability
        if (!IsValidDate(day, month, year))
        {
            throw new ArgumentException("Error: Invalid date.");
        }

        this._day = day;
        this._month = month;
        this._year = year;
    }

    // Example of constructor overloading and reusing the logic of another constructor
    public Date(int year) : this(1, 1, year) { }
    public Date(int month, int year) : this(1, month, year) { }

    public string GetDate()
    {
        // Use interpolation for better readability (Best Practice)
        return $"{_day:D2}/{_month:D2}/{_year:D4}";

        // bad practice:
        return $"{_day.ToString().PadLeft(2, '0')}/{_month.ToString().PadLeft(2, '0')}" +
                $"/{_year.ToString().PadLeft(4,'0')}";
        
    }
}
