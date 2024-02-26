using System;
using System.Collections.Generic;

namespace CSIComparable
{
    class Program
    {
        static void Main()
        {
            var temps = new List<Temperature>(); 
            Random rnd = new Random();
            for(int i = 0; i < 100; i++)
            {
                temps.Add(new Temperature(rnd.Next(-30, 50)));
            }

            temps.Sort();
            foreach(Temperature temp in temps)
            {
                Console.WriteLine($"{temp.Value}, ");
            }
        }
    } 

    public class Temperature : IComparable<Temperature>
    {
        private int _value;

        public int Value => _value;

        public Temperature(int value)
        {
            _value = value;
        }

        public int CompareTo(Temperature other)
        {
            if (other == null)
                return 1;

            return this._value.CompareTo(other.Value);
        }
    }
}
