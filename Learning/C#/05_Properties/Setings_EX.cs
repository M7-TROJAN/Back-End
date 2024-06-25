using System;
using System.Reflection.Metadata.Ecma335;

namespace Revision
{


    public static class Settings
    {
        public static string DayName => DateTime.Today.DayOfWeek.ToString();

        public static int DayNumber => DateTime.Today.Day;

        public static string FormattedDate() => DateTime.Now.ToString("ddd dd-MM-yyyy hh:mm:ss tt");
        // ddd => abbreviated day name
        // dd-MM-yyyy => day-month-year format
        // hh:mm:ss => hours:minutes:seconds format
        // tt => 12-hour clock format with AM/PM indicator

        public static string ProjectPath
        {
            get; set;
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Settings.DayName);
            Console.WriteLine(Settings.DayNumber);
            Console.WriteLine(Settings.FormattedDate());

            Settings.ProjectPath = @"C:\MyProjects\bla bla bla";
            // When you prefix a string  with the '@' symbol, we are creating a verbatim string,
            // which means that escape characters are not processed.
            // This can be useful when dealing with file paths or other strings that contain backslashes.

            Console.WriteLine(Settings.ProjectPath);
            Console.ReadKey();


        }
    }
}
