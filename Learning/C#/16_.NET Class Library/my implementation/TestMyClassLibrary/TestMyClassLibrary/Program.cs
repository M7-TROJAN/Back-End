using MyClassLibrary;
internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World");

        int r = RandomUtility.RandomNumber(100,20000);
        Console.WriteLine(r);

        char c = RandomUtility.GetRandomCharacter(CharType.SmallLetter);
        Console.WriteLine(c);

        string RWord = RandomUtility.GetRandomWord(length: 32);
        Console.WriteLine(RWord);

        string pass = RandomUtility.GeneratePassword();
        Console.WriteLine(pass);

        string key = RandomUtility.GenerateKey();
        Console.WriteLine(key);
    }
}