using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Learning
{
    class Program
    {
        static void Main()
        {
            // Creating and displaying an integer collection
            Collection<int> intCollection = new Collection<int>(5, 3, 1, 4, 2);
            Console.WriteLine("Integer Collection (Before Sorting):");
            Print(intCollection);

            intCollection.Sort();
            Console.WriteLine("Integer Collection (After Sorting):");
            Print(intCollection);

            // Creating and displaying a string collection
            Collection<string> stringCollection = new Collection<string>("d", "a", "c", "b", "e");
            Console.WriteLine("String Collection (Before Sorting):");
            Print(stringCollection);

            stringCollection.Sort();
            Console.WriteLine("String Collection (After Sorting):");
            Print(stringCollection);

            // Creating and displaying a double collection
            Collection<double> doubleCollection = new Collection<double>(4.4, 1.1, 3.3, 2.2, 5.5);
            Console.WriteLine("Double Collection (Before Sorting):");
            Print(doubleCollection);

            doubleCollection.Sort();
            Console.WriteLine("Double Collection (After Sorting):");
            Print(doubleCollection);
        }

        // Helper method to print collection items
        private static void Print<T>(Collection<T> collection) where T : IComparable<T>
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }

    class Collection<T> : IEnumerable<T>, IComparable<Collection<T>> where T : IComparable<T>
    {
        private T[] _items;

        public Collection(params T[] items)
        {
            _items = items;
        }

        public Collection()
        {
            _items = Array.Empty<T>();
        }

        // Compare two collections based on their first elements
        public int CompareTo([AllowNull] Collection<T> other)
        {
            if (other == null) return 1;
            if (_items.Length == 0 && other._items.Length == 0) return 0;
            if (_items.Length == 0) return -1;
            if (other._items.Length == 0) return 1;
            return _items[0].CompareTo(other._items[0]);
        }

        // Sort the items in the collection
        public void Sort()
        {
            Array.Sort(_items);
        }

        // Enumerator implementation
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
