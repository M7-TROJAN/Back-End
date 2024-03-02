using System;
using System.Text;
using System.Xml.Linq;

namespace XMLDocumentation
{
    class Program
    {
        static void Main()
        {
            var id = Generator.GenerateID("Mahmoud", "Mattar", new DateTime(2020, 7, 15));
            Console.WriteLine(id);

            var password = Generator.GeneratePassword(20);
            Console.WriteLine(password);

            for(var i = 0; i < 10; i++)
            {
                Console.WriteLine(Generator.GeneratePassword(20));
            }
            Console.ReadKey();
        }
    }

    /// <include file='Documentation.xml' path='docs/members[@name="generator"]/Generator/*'/>
    public static class Generator
    {
        private static Random? random;
        public static int LastIdSequence { get; private set; } = 1;

        /// <include file='Documentation.xml' path='docs/members[@name="generator"]/GenerateID/*'/>
        public static string GenerateID(string firstName, string lastName, DateTime? hireDate)
        {
            if (firstName is null)
                throw new ArgumentNullException(nameof(firstName), "First name cannot be null");

            if (lastName is null)
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be null");

            if (hireDate is null)
                throw new ArgumentNullException(nameof(hireDate), "Hire date cannot be null");

            if (hireDate.Value.Date > DateTime.Now.Date)
                throw new ArgumentException("Hire date cannot be in the future", nameof(hireDate));

            // Generate employee initials (first two letters of first and last name)
            string initials = $"{firstName.Substring(0, Math.Min(2, firstName.Length))}" +
                              $"{lastName.Substring(0, Math.Min(2, lastName.Length))}";

            // Extract year, month, day from the hire date
            string year = hireDate.Value.ToString("yy"); // Ensure 2-digit format
            string month = hireDate.Value.ToString("MM");
            string day = hireDate.Value.ToString("dd");

            // Extract hour, minute, second, and millisecond from current time
            string time = DateTime.Now.ToString("HHmmssfff");

            // Get the sequence number and increment the LastIdSequence
            string sequence = LastIdSequence++.ToString();

            // Construct the ID using the specified format
            string employeeId = $"{initials}{year}{month}{day}{time}{sequence}";

            return employeeId;
        }

        /// <include file='Documentation.xml' path='docs/members[@name="generator"]/GeneratePassword/*'/>
        public static string GeneratePassword(int passwordLength = 16)
        {
            if (passwordLength < 1)
                return "";

            random = new Random();

            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=";

            StringBuilder passwordBuilder = new StringBuilder();
            for (var i = 0; i < passwordLength; i++)
            {
                passwordBuilder.Append(Characters[random.Next(Characters.Length)]);
            }

            VerifyPassword(passwordBuilder.ToString());
            return passwordBuilder.ToString();

        }

        /// <include file='Documentation.xml' path='docs/members[@name="generator"]/VerifyPassword/*'/>
        private static bool VerifyPassword(string password)
        {
            // check if password is Null or the length is insufficient or has spaces
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Contains(' '))
                return false;

            bool has_upper = false;
            bool has_lower = false;
            bool has_digit = false;
            bool has_symbol = false;

            // check for each of the required characters
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsUpper(password[i]))
                {
                    has_upper = true;
                }

                if (char.IsLower(password[i]))
                {
                    has_lower = true;
                }

                if (char.IsDigit(password[i]))
                {
                    has_digit = true;
                }

                if (char.IsPunctuation(password[i]))
                {
                    has_symbol = true;
                }
            }

            // return false if any required characters is not present
            if (!has_upper)
                return false;
            if (!has_lower)
                return false;
            if (!has_digit)
                return false;
            if (!has_symbol)
                return false;

            // if we reach here the password it must be valid
            return true;
        }
    }
}