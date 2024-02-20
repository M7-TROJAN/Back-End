using System;
namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage with integers
            var intItems = new Any<int>();

            intItems.Add(1);
            intItems.DisplayList();
            intItems.Add(2);
            intItems.DisplayList();

            intItems.RemoveAt(1);
            intItems.DisplayList();

            // Example usage with strings
            var stringItems = new Any<string>(new string[] { "Mahmoud", "Mohamed", "Abdalaziz", "Mousa" });

            stringItems.DisplayList();

            stringItems.Add("Mattar");
            stringItems.RemoveFirstElement();
            stringItems.DisplayList();

            stringItems.InsertAt(2, "Test");
            stringItems.DisplayList();

            stringItems.Clear();
            stringItems.DisplayList();

            // Example usage with custom class Person
            var people = new Any<Person>();

            people.Add(new Person { Fname = "Mahmoud", Lname = "Mattar" });
            people.Add(new Person { Fname = "Moustafa", Lname = "Mattar" });

            people.DisplayList();
        }
    }

    class Person
    {
        public string Fname { get; set; } = string.Empty;
        public string Lname { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"'{Fname} {Lname}'";
        }
    }
    class Any<T>
    {
        private  T[] _items;

        public bool IsEmpty => _items.Length == 0 || _items is null;
        public int Count => _items is null ? 0 : _items.Length;


        public Any()
        {
            _items = Array.Empty<T>();
        }

        public Any(T[] items)
        {
            _items = items ?? Array.Empty<T>();

            // `items ?? Array.Empty<T>()`: This is the null - coalescing operator (`??`).
            // It checks if `items` is `null`. If `items` is not `null`, it assigns its value to `_items`.
            // If `items` is `null`, it assigns `Array.Empty<T>()` to `_items`.

            // So, the line `_items = items ?? Array.Empty<T>();` ensures that `_items` is always initialized
            // with a valid array, either with the provided array `items`
            // or with an empty array if `items` is `null`.
            // This prevents null reference exceptions and simplifies subsequent logic
            // by guaranteeing that `_items` is never null.

        }

        
        public void Add(T item)
        {
                // old lojic:
                /*
                var length = _items.Length;

                T[] dest = new T[length + 1];

                Array.Copy(_items, dest, length);
                dest[dest.Length - 1] = item;

                _items = dest;
                */

                // new lojic
                Array.Resize(ref _items, _items.Length + 1);
                _items[_items.Length - 1] = item;
        }
        public void RemoveAt(int position)
        {
            // Check if the index is valid
            if (position < 0 || position >= _items.Length || IsEmpty)
                return;

            // Create a new array with one less element
            T[] newArray = new T[_items.Length - 1];

            // Copy elements from the original array to the new array
            int index = 0; // Initialize a new index counter
            for (int i = 0; i < _items.Length; i++)
            {
                // Skip the element at the specified index
                if (i == position)
                    continue;

                // Copy the element to the new array
                newArray[index++] = _items[i];
            }

            // Update the reference to the array
            _items = newArray;
        }

        public void RemoveFirstElement()
        {
            if (!IsEmpty)
            {
                RemoveAt(0);
            }
        }

        public void RemoveLastElement()
        {
            if (!IsEmpty)
            {
                RemoveAt(_items.Length - 1);
            }
            
        }

        public void InsertAt(int index, T value)
        {
            if (index < 0 || index >= _items.Length)
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
            //if (_items is null)
            //{
            //    Console.WriteLine("null");
            //    return;
            //}

            //var type = typeof(T).ToString().Replace("System.", ""); // Get the type of the value

            //Console.WriteLine($"{type} Array Elements:");
            //Console.Write("{");
            //for(int i = 0; i < _items.Length; i++)
            //{
            //    Console.Write(_items[i]);

            //    if(i < _items.Length - 1)
            //        Console.Write(",");
            //}
            //Console.WriteLine("}");

            Console.WriteLine($"{typeof(T).Name} Array Elements: {{{string.Join(", ", _items)}}}");
        }
    }
}


/*
 Old logic:

        public void InsertAt(int position, T value)
        {
            // if the index is valid
            if (position < 0 || position >= _items.Length || IsEmpty)
                return;

            // new array with one more element
            T[] newArray = new T[_items.Length + 1];

            // Copy elements before position from the original array to the new array
            for (int i = 0; i < position; i++)
            {
                newArray[i] = _items[i];
            }

            newArray[position] = value;

            // Copy elements after from the original array to the new array
            for (int i = position; i < _items.Length; i++)
            {
                newArray[i+1] = _items[i];
            }

            // Update the reference to the array
            _items = newArray;
        }
 
*/
