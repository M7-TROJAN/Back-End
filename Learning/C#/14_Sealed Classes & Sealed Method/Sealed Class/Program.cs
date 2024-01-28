// when we don't want a class to be inherited by another class, we can declare the class as a sealed class.

using System;
namespace SealedClass
{
    sealed  class clsA
    {

    }

    // trying to inherit sealed class
    // Error Code
    class clsB : clsA
    {

    }

    class Program
    {
        static void Main(string[] args)
        {

            // create an object of B class
            clsB B1 = new clsB();

            Console.ReadKey();
        }
    }
}
