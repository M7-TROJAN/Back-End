using CSEnumeratorEnumerable;
using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Claims;

namespace CSEnumeratorEnumerable
{
    class Program
    {
        static void Main()
        {

            var integers = new FiveIntegers(1, 2, 3, 4, 5);


            foreach (var integer in integers)
            {
                Console.WriteLine(integer);
            }
        }
    }

    // Custom collection class FiveIntegers implementing IEnumerable
    public class FiveIntegers : IEnumerable
    {
        private int[] _values; 


        public FiveIntegers(int n1, int n2, int n3, int n4, int n5)
        {
            _values = new int[] { n1, n2, n3, n4, n5 }; 
        }

        // Implementation of the IEnumerable interface method GetEnumerator using yield
        public IEnumerator GetEnumerator()
        {
            // Yielding each element of the array
            foreach (var item in _values)
                yield return item;


            // Now the code is much cleaner and more readable compared to the previous version, 

            // By using `yield return item;` inside the `GetEnumerator` method, we're able to simplify the code significantly.
            // This single line replaces the need for a separate `Enumerator` class with explicit implementations
            // of `MoveNext`, `Reset`, and maintaining an index variable.

        }
    }

}
