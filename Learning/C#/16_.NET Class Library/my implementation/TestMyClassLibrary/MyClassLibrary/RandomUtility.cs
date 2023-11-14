namespace MyClassLibrary
{
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

        public static int RandomNumber(int from = 0, int to = 100)
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
}