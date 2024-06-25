/*
In C#, struct is the value type data type that represents data structures.

struct can be used to hold small data values that do not require inheritance, e.g. coordinate points, key-value pairs, 
and complex data structure.

A struct object can be created with or without the new operator, same as primitive type variables.

If you declare a variable of struct type without using new keyword, it does not call any constructor, 
so all the members remain unassigned. Therefore, you must assign values to each member before accessing them, 
otherwise, it will give a compile-time error.

using new does not mean it's allocated in heap.
structure is allocated in stack as long as it's not part of class.
*/


using System;

namespace Main
{
    internal class Program
    {
        private struct stStudent
        {
            public string FirstName;
            public string LastName;
        }

        private static void Main(string[] args)
        {
            //A struct object can be created with or without the new operator,
            //same as primitive type variables.

            stStudent Student;
            stStudent Student2 = new stStudent();

            Student.FirstName = "Mahmoud";
            Student.LastName = "Mattar";

            Console.WriteLine(Student.FirstName);
            Console.WriteLine(Student.LastName);

            Student2.FirstName = "Ali";
            Student2.LastName = "Ahmed";

            Console.WriteLine(Student2.FirstName);
            Console.WriteLine(Student2.LastName);

            Console.ReadKey();
        }
    }
}