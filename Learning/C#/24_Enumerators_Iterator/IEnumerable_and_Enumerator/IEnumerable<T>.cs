using System;
using System.Collections;
using System.Collections.Generic;

namespace Learning
{
    class Program
    {
        static void Main()
        {
            // Example usage of the fiveItems class
            var fiveInts = new FiveItems<int>(1, 2, 3, 4, 5);
            foreach (var item in fiveInts)
            {
                Console.WriteLine(item);
            }

            var fiveStrings = new FiveItems<string>("A", "B", "C", "D", "E");
            foreach (var item in fiveStrings)
            {
                Console.WriteLine(item);
            }
        }
    }

    class FiveItems<T> : IEnumerable<T>
    {
        private readonly T[] items;
        private readonly int length;

        public FiveItems()
        {
            items = new T[5];
            length = items.Length;
        }

        public FiveItems(T n1, T n2, T n3, T n4, T n5)
        {
            items = [n1, n2, n3, n4, n5];
            length = items.Length;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                return items[index];
            }
            set
            {
                if (index < 0 || index >= items.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                items[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new fiveItemsEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class fiveItemsEnumerator(FiveItems<T> collection) : IEnumerator<T>
        {
            private readonly FiveItems<T> _collection = collection;
            private int _currentIndex = -1;
            private T _currentItem = default;

            public T Current => _currentItem;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (++_currentIndex >= _collection.length)
                {
                    return false;
                }
                else
                {
                    _currentItem = _collection[_currentIndex];
                    return true;
                }
            }

            public void Reset()
            {
                _currentIndex = -1;
                _currentItem = default;
            }

            public void Dispose()
            {
                // No resources to dispose
            }
        }
    }
}
