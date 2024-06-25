using System;

namespace Revision
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string str = null;

            // Attempting to call ToLower() on a null string will result in a NullReferenceException.
            var str2 = str.ToLower(); // NullReferenceException

            // you are trying to acsses to nothing! null is nothing
            // انت بتحاول تنفذ حاجة علي شيء مش موجود اصلا


            // You are trying to access a method (ToLower()) on a string variable (str),
            // but the variable is currently set to null. Null represents the absence of an object reference,
            // so attempting to perform an operation on it will lead to a NullReferenceException.

            // We can prevent the exception using the 'Null Conditional Operator' (?)
            var str3 = str?.ToLower();
            // By adding ? after the variable name, we are saying if it's not null, call the ToLower() method;
            // otherwise, return null. This prevents a NullReferenceException and assigns null to str3 if str is null.
            
        }
    }
}
