/* 
you can get the current date time using :

    DateTime dt1 = new DateTime();
    dt1 = DateTime.Now;
*/
using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            //assigns default value 01/01/0001 00:00:00
            DateTime dt1 = new DateTime();

            dt1 = DateTime.Now;
            Console.WriteLine(dt1);

            Console.ReadKey();

            }
        }
    }