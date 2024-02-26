using System;
using System.Collections;

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

        // Implementation of the IEnumerable interface method GetEnumerator
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this); // Return an instance of the custom Enumerator class
        }

        // Custom enumerator class Enumerator implementing IEnumerator
        private class Enumerator : IEnumerator
        {
            private int _currentIndex = -1; // Index to track the current position
            private readonly FiveIntegers _integers; // Reference to the parent FiveIntegers collection

            public Enumerator(FiveIntegers integers)
            {
                _integers = integers;
            }

            
            public object Current
            {
                get
                {
                    // Throw exception if enumeration has not started or has ended
                    if (_currentIndex == -1)
                        throw new InvalidOperationException("Enumeration not started");

                    if (_currentIndex == _integers._values.Length)
                        throw new InvalidOperationException("Enumeration ended");

                    // Return the current element in the collection
                    return _integers._values[_currentIndex];
                }
            }

            
            public bool MoveNext()
            {
                // Return false if the end of the collection is reached
                if (_currentIndex >= _integers._values.Length - 1)
                    return false;

                // Increment the current index and return true
                return ++_currentIndex < _integers._values.Length;
            }

            
            public void Reset()
            {
                _currentIndex = -1; // Set the current index back to -1
            }
        }
    }
}
