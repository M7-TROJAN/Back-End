namespace NumberToWordsConverter
{
    public static class Converter
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
}

