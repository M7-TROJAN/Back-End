
using System;

namespace Revision
{
    class Program
    {
        

        static void Main(string[] args)
        {
            // using == with refrence type

            // compare the values (value type)
            var x = 10;
            var y = 10;
            var z = x == y;
            Console.WriteLine(z); // True 

            // compare the refrences (refrence type)
            var s1 = "Mahmoud";
            var s2 = "Mahmoud";
            var s3 = s1 == s2;
            Console.WriteLine(z); // True  But Why?

            // Although each variable has a different address from the other,
            // the CLR internally calls the `Equals()` method for string comparison.
            // This behavior is due to string interning, and operator overloading is executed.

        }
    }
}
