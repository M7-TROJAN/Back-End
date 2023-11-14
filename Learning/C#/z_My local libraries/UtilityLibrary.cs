namespace UtilityLibrary
{
    public static class ValidationUtility
    {
        public static bool IsInRange(int value, int lowerBound, int upperBound)
        {
            return value >= lowerBound && value <= upperBound;
        }

        public static bool IsInRange(float value, float lowerBound, float upperBound)
        {
            return value >= lowerBound && value <= upperBound;
        }

        public static bool IsInRange(long value, long lowerBound, long upperBound)
        {
            return value >= lowerBound && value <= upperBound;
        }

        public static bool IsInRange(double value, double lowerBound, double upperBound)
        {
            return value >= lowerBound && value <= upperBound;
        }

        public static bool IsInRange(char value, char lowerBound, char upperBound, bool matchCase = true)
        {
            if (matchCase)
            {
                return value >= lowerBound && value <= upperBound;
            }
            else
            {
                char lower = char.ToLower(lowerBound);
                char upper = char.ToLower(upperBound);
                char val = char.ToLower(value);
                return val >= lower && val <= upper;
            }
        }

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
            for(int i = 0; i < password.Length; i++)
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

    public static class NumberConversionUtility
    {
        public static string ConvertNumberToText(double number)
        {
            string numberString = ToText((long)number);
            int fractionalPart = (int)Math.Round((number - (int)number) * 100);
            if (fractionalPart > 0)
                numberString += "and " + ToText(fractionalPart) + "cent.";

            return numberString;
        }

        private static string ToText(long number)
        {
            string text = NumberToText(number);
            if (string.IsNullOrEmpty(text))
                return "Zero";
            return text;
        }

        private static string NumberToText(long number)
        {
            if (number == 0)
                return "";

            if (number < 0)
                return "Negative " + NumberToText(-number);

            if (number >= 1 && number <= 19)
            {
                string[] unitsText =
                {
                    "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                    "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
                };
                return unitsText[number] + " ";
            }

            if (number >= 20 && number <= 99)
            {
                string[] tensText = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                return tensText[number / 10] + " " + NumberToText(number % 10);
            }

            if (number >= 100 && number <= 999)
            {
                return NumberToText(number / 100) + "Hundred " + NumberToText(number % 100);
            }

            if (number >= 1000 && number <= 1999)
            {
                return "One Thousand " + NumberToText(number % 1000);
            }

            if (number >= 2000 && number <= 999999)
            {
                return NumberToText(number / 1000) + "Thousands " + NumberToText(number % 1000);
            }

            if (number >= 1000000 && number <= 1999999)
            {
                return "One Million " + NumberToText(number % 1000000);
            }

            if (number >= 2000000 && number <= 999999999)
            {
                return NumberToText(number / 1000000) + "Millions " + NumberToText(number % 1000000);
            }

            if (number >= 1000000000 && number <= 1999999999)
            {
                return "One Billion " + NumberToText(number % 1000000000);
            }

            if (number >= 2000000000 && number <= 999999999999)
            {
                return NumberToText(number / 1000000000) + "Billions " + NumberToText(number % 1000000000);
            }

            if (number >= 1000000000000 && number <= 1999999999999)
            {
                return "One Trillion " + NumberToText(number % 1000000000000);
            }
            else
            {
                return NumberToText(number / 1000000000000) + "Trillions " + NumberToText(number % 1000000000000);
            }
        }
    }

    public enum CharType
    {
        SmallLetter = 1,
        CapitalLetter = 2,
        Digit = 3,
        MIX = 4
    }

    public static class RandomUtility
    {
        private static readonly Random random = new Random();

        public static int RandomNumber(int from, int to)
        {
            return random.Next(from, to + 1);
        }

        public static char GetRandomCharacter(CharType charType = CharType.MIX)
        {
            switch (charType)
            {
                case CharType.SmallLetter:
                    return (char)RandomNumber(97, 122);
                case CharType.CapitalLetter:
                    return (char)RandomNumber(65, 90);
                case CharType.Digit:
                    return (char)RandomNumber(48, 57);
                default:
                    return GetRandomCharacter((CharType)RandomNumber(1, 3));
            }
        }

        public static string GetRandomWord(CharType charType = CharType.SmallLetter, int length = 0)
        {
            if (length == 0)
            {
                length = RandomNumber(4, 15);
            }

            var word = "";
            for (var i = 0; i < length; i++)
            {
                word += GetRandomCharacter(charType);
            }
            return word;
        }

        public static string GeneratePassword(int passwordLength = 16)
        {
            if (passwordLength < 1)
                return "";

            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=";

            var password = "";
            for (var i = 0; i < passwordLength; i++)
            {
                password += Characters[random.Next(Characters.Length)];
            }

            return password;
        }

        public static string GenerateKey()
        {
            const int PasswordLength = 16;
            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var password = "";
            for (var i = 0; i < PasswordLength; i++)
            {
                if (i % 4 == 0 && i > 0)
                {
                    password += '-';
                }
                password += Characters[random.Next(Characters.Length)];
            }

            return password;
        }

    }

    public static class ArrayUtility
    {

        public static void FillArrayWithRandomNumbers(int[] array, int from = 0, int to = 100)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = RandomUtility.RandomNumber(from, to);
            }
        }

        // Function to shuffle an array of integers
        public static void ShuffleArray(int[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomUtility.RandomNumber(0, length - 1);
                int randomIndex2 = RandomUtility.RandomNumber(0, length - 1);
                Swap(arr, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle an array of strings
        public static void ShuffleArray(string[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomUtility.RandomNumber(0, length - 1);
                int randomIndex2 = RandomUtility.RandomNumber(0, length - 1);
                Swap(arr, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle a List of integers
        public static void ShuffleArray(List<int> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomUtility.RandomNumber(0, length - 1);
                int randomIndex2 = RandomUtility.RandomNumber(0, length - 1);
                Swap(list, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle a List of strings
        public static void ShuffleArray(List<string> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomUtility.RandomNumber(0, length - 1);
                int randomIndex2 = RandomUtility.RandomNumber(0, length - 1);
                Swap(list, randomIndex1, randomIndex2);
            }
        }

        // Function to swap elements at two indices in a list
        private static void Swap<T>(List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        // Function to swap elements at two indices in an array
        private static void Swap<T>(T[] arr, int index1, int index2)
        {
            T temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }

        // Function to check if an array of integers is a palindrome
        public static bool IsPalindromeArray(int[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if an array of doubles is a palindrome
        public static bool IsPalindromeArray(double[] arr)
        {
            int length = arr.Length;

            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if an array of strings is a palindrome
        public static bool IsPalindromeArray(string[] arr)
        {
            int length = arr.Length;

            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of integers is a palindrome
        public static bool IsPalindromeArray(List<int> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of doubles is a palindrome
        public static bool IsPalindromeArray(List<double> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of strings is a palindrome
        public static bool IsPalindromeArray(List<string> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }
    }


    public static class EncryptionUtility
    {
        // This function takes a string and an encryption key and returns the encrypted string.
        public static string EncryptText(string text, short encryptionKey = 2)
        {
            char[] charArray = text.ToCharArray();

            // Iterate over each character in the string and add the encryption key to its ASCII code.
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = (char)(charArray[i] + encryptionKey);
            }

            return new string(charArray);
        }

        // This function takes a string and an encryption key and returns the decrypted string.
        public static string DecryptText(string text, short encryptionKey = 2)
        {
            char[] charArray = text.ToCharArray();

            // Iterate over each character in the string and subtract the encryption key from its ASCII code.
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = (char)(charArray[i] - encryptionKey);
            }

            return new string(charArray);
        }
    }
}
