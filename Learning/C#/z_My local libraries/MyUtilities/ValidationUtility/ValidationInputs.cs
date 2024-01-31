namespace ValidationInputsUtility
{
    public static class ValidationInputs
    {
        public static int ReadPositiveNumber(string message = "")
        {
            int number;
            do
            {
                Console.Write(message);
                if (!int.TryParse(Console.ReadLine(), out number) || number <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer.");
                }
            } while (number <= 0);
            return number;
        }

        public static int ReadNumberInRange(int from, int to, string message = "")
        {
            int number;
            do
            {
                Console.Write(message);
                if (!int.TryParse(Console.ReadLine(), out number) || number < from || number > to)
                {
                    Console.WriteLine($"Invalid input. Please enter a valid integer between {from} and {to}.");
                }
            } while (number < from || number > to);
            return number;
        }

        public static int GetInt(string message = "")
        {
            string userInput;
            int result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!int.TryParse(userInput, out result));

            return result;
        }

        public static long GetLongInt(string message = "")
        {
            string userInput;
            long result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!long.TryParse(userInput, out result));

            return result;
        }

        public static float GetFloat(string message = "")
        {
            string userInput;
            float result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!float.TryParse(userInput, out result));

            return result;
        }

        public static double GetDouble(string message = "")
        {
            string userInput;
            double result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!double.TryParse(userInput, out result));

            return result;
        }

        public static char GetChar(string message = "")
        {
            string userInput;
            char result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!char.TryParse(userInput, out result) || userInput.Length != 1);

            return result;
        }

        public static DateTime GetDate(string message = "")
        {
            string userInput;
            DateTime result;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (!DateTime.TryParse(userInput, out result));

            return result;
        }

        public static string GetString(string message = "")
        {
            string userInput;
            do
            {
                Console.Write(message);
                userInput = Console.ReadLine();
            } while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

        public static string ReadPinCode(string message = "Please enter Pin Code: ")
        {
            string pinCode;
            do
            {
                Console.Write(message);
                pinCode = Console.ReadLine();

                if (!int.TryParse(pinCode, out _) || pinCode.Length != 4 || !pinCode.All(char.IsDigit))
                {
                    Console.Error.WriteLine("Invalid input! Pin Code should be a 4-digit numeric value.");
                }
            } while (!int.TryParse(pinCode, out _) || pinCode.Length != 4 || !pinCode.All(char.IsDigit));

            return pinCode;
        }

        public static string ReadPhoneNumber(string message = "Please enter phone number: ")
        {
            string phoneNumber;
            do
            {
                Console.Write(message);
                phoneNumber = Console.ReadLine();

                if (!long.TryParse(phoneNumber, out _) || phoneNumber.Length != 11 || !phoneNumber.All(char.IsDigit))
                {
                    Console.Error.WriteLine("Invalid input! Phone number should be an 11-digit numeric value.");
                }
                else if (!phoneNumber.StartsWith("010") && !phoneNumber.StartsWith("011") &&
                         !phoneNumber.StartsWith("012") && !phoneNumber.StartsWith("015"))
                {
                    Console.Error.WriteLine("Invalid input! Please Enter a Valid Phone number.");
                }
            } while (!long.TryParse(phoneNumber, out _) || phoneNumber.Length != 11 || !phoneNumber.All(char.IsDigit) ||
                     !phoneNumber.StartsWith("010") && !phoneNumber.StartsWith("011") &&
                     !phoneNumber.StartsWith("012") && !phoneNumber.StartsWith("015"));

            return phoneNumber;
        }

        public static string ReadPassword(string message = "Please enter a password (at least 4 characters, no spaces): ")
        {
            string password;
            do
            {
                Console.Write(message);
                password = Console.ReadLine();

                if (password == null || password.Length < 4 || password.Contains(" "))
                {
                    Console.Error.WriteLine("Invalid input! Password must be at least 4 characters long and should not contain spaces.");
                }
            } while (password == null || password.Length < 4 || password.Contains(" "));

            return password;
        }


        public static string ReadStrongPassword(
            string message = "Please enter a password (at least 8 characters, 1 uppercase letter, " +
            "1 lowercase letter, 1 digit, 1 symbol and no spaces): ")
        {
            string password;

            do
            {
                password = ReadPassword(message);
            } while (!VerifyPassword(password));

            return password;
        }

        public static bool VerifyPassword(string password)
        {
            // check if password is Null or the length is insufficient or has spaces
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Contains(" "))
            {
                return false;
            }

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




//public static bool IsInRange(int value, int lowerBound, int upperBound)
//{
//    return value >= lowerBound && value <= upperBound;
//}

//public static bool IsInRange(float value, float lowerBound, float upperBound)
//{
//    return value >= lowerBound && value <= upperBound;
//}

//public static bool IsInRange(long value, long lowerBound, long upperBound)
//{
//    return value >= lowerBound && value <= upperBound;
//}

//public static bool IsInRange(double value, double lowerBound, double upperBound)
//{
//    return value >= lowerBound && value <= upperBound;
//}

//public static bool IsInRange(char value, char lowerBound, char upperBound, bool matchCase = true)
//{
//    if (matchCase)
//    {
//        return value >= lowerBound && value <= upperBound;
//    }
//    else
//    {
//        char lower = char.ToLower(lowerBound);
//        char upper = char.ToLower(upperBound);
//        char val = char.ToLower(value);
//        return val >= lower && val <= upper;
//    }
//}