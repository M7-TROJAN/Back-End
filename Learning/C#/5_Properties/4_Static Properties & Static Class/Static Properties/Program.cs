using System;

internal static class Settings
{
    // Property to retrieve the current date in the format "YYYY-MM-DD"
    public static string CurrentDate
    {
        get { return DateTime.Today.ToString("yyyy-MM-dd"); }
    }

    // Property to obtain the current time in 12-hour format with hours and minutes
    public static string CurrentTime
    {
        get { return DateTime.Now.ToString("hh:mm tt"); }
    }

    // Property to retrieve the name of the day for the current date
    public static string CurrentDayName
    {
        get { return DateTime.Today.ToString("dddd"); }

        // or
        // get { return DateTime.Today.DayOfWeek.ToString(); }
    }

    // Property to get the day of the month
    public static int DayOfMonth
    {
        get { return DateTime.Today.Day; }
    }

    // Property to get the number of the month
    public static int MonthNumber
    {
        get { return DateTime.Today.Month; }
    }

    // Property to get the year
    public static int Year
    {
        get { return DateTime.Today.Year; }
    }

    public static string ProjectPath
    {
        get;
        set;
    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        // Output using the properties to display current time, day name, and date
        Console.WriteLine("Current Time: " + Settings.CurrentTime);
        Console.WriteLine("Current Day: " + Settings.CurrentDayName);
        Console.WriteLine("Current Date: " + Settings.CurrentDate);

        // Accessing specific properties of the DateTime structure directly
        Console.WriteLine("Day of the month: " + Settings.DayOfMonth);

        // Accessing specific properties of the DateTime structure directly
        Console.WriteLine("Number of the month: " + Settings.MonthNumber);

        // Accessing specific properties of the DateTime structure directly
        Console.WriteLine("Year: " + Settings.Year);

        Console.WriteLine(DateTime.Today.DayOfWeek.ToString());

        Settings.ProjectPath = @"C:\Users\matte\Desktop";

        Console.WriteLine(Settings.ProjectPath);

        Console.ReadKey();
    }
}