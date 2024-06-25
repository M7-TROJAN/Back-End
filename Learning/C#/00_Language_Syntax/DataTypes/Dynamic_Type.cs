/*
C# 4.0 (.NET 4.5) introduced a new type called dynamic that avoids compile-time type checking. 
A dynamic type escapes type checking at compile-time; instead, it resolves type at run time.

A dynamic type variables are defined using the dynamic keyword.
*/

using System;

namespace Main
    {
        internal class Program
        {

        static void Main(string[] args)
            {

            dynamic MyDynamicVar = 100;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = "Hello World!!";
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = true;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = DateTime.Now;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            Console.ReadKey();

            }
        }
    }

