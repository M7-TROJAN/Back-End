// C# ALLOWS YOU TO USE CONSTRAINTS TO RESTRICT CLIENT CODE TO SPECIFY CERTAIN TYPES WHILE INSTANTIATING GENERIC TYPES. 
// IT WILL GIVE A COMPILE-TIME ERROR IF YOU TRY TO INSTANTIATE A GENERIC TYPE USING A TYPE THAT IS NOT ALLOWED BY THE SPECIFIED CONSTRAINTS

// syntax: where T: Constraint1, Constraint2, Constraint3, .........

using System;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage with custom class Person
            var people = new Any<Person>();

            people.Add(new Person { Fname = "Mahmoud", Lname = "Mattar" });
            people.Add(new Person { Fname = "Moustafa", Lname = "Mattar" });

            people.DisplayList();
        }
    }

    class Person
    {
        public string Fname { get; set; }
        public string Lname { get; set; }

        public override string ToString()
        {
            return $"'{Fname} {Lname}'";
        }
    }
    class Any<T> where T : class
    {
        private T[] _items;

        public bool IsEmpty => _items?.Length == 0;
        public int Count => _items?.Length ?? 0;

        public Any()
        {
            _items = Array.Empty<T>();
        }

        public Any(T[] items)
        {
            _items = items ?? Array.Empty<T>();
        }

        public void Add(T item)
        {
            Array.Resize(ref _items, _items.Length + 1);
            _items[^1] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _items?.Length)
                return;

            T[] newArray = new T[_items.Length - 1];
            for (int i = 0, j = 0; i < _items.Length; i++)
            {
                if (i != index)
                    newArray[j++] = _items[i];
            }

            _items = newArray;
        }

        public void RemoveFirstElement()
        {
            RemoveAt(0);
        }

        public void RemoveLastElement()
        {
            RemoveAt(_items.Length - 1);
        }


        public void InsertAt(int index, T value)
        {
            if (index < 0 || index >= _items?.Length)
                return;

            Array.Resize(ref _items, _items.Length + 1);
            for (int i = _items.Length - 1; i > index; i--)
            {
                _items[i] = _items[i - 1];
            }
            _items[index] = value;
        }

        public void Clear()
        {
            _items = Array.Empty<T>();
        }

        public void DisplayList()
        {
            Console.WriteLine($"{typeof(T).Name} Array Elements: " +
                $"{{{string.Join(", ", Array.ConvertAll(_items, x => x.ToString()))}}}");
        }
    }

}
