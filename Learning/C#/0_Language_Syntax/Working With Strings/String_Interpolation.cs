/*
String Interpolation
String interpolation is a better way of concatenating strings. We use + sign to concatenate string variables with static strings.

C# 6 includes a special character $ to identify an interpolated string. 
An interpolated string is a mixture of static string and string variable where string variables should be in {} brackets.
*/


using System;

namespace Main
{
    internal class Program
    {

        static void Main(string[] args)
        {

            //  String Interpolation

            string firstName = "Mahmoud";
            string lastName = "Mattar";
            string code = "107";

            //You shold use $ to $ to identify an interpolated string 
            string fullName = $"Mr. {firstName} {lastName}, Code: {code}";

            Console.WriteLine(fullName);

            Console.ReadKey();

        }
    }
}

