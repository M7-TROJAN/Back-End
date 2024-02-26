

using System;
using System.Collections;
namespace CSIComparable
{
    class Program
    {
        static void Main()
        {
            var temps = new List<Tempreture>(); 
            Random rnd = new Random();
            for(int i = 0; i < 100; i++)
            {
                temps.Add(new Tempreture(rnd.Next(-30, 50)));
            }

            temps.Sort();
            foreach(Tempreture temp in temps)
            {
                Console.WriteLine($"{temp.Value}, ");
            }

        }
    } 

    public class Tempreture : IComparable
    {
        private int _value;

        public int Value => _value;

        public Tempreture(int value)
        {
            _value = value;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var other = obj as Tempreture;

            if (other == null)
                throw new ArgumentException("Object Is Not a Tempreture");

            return this._value.CompareTo(other.Value);


            // or we canimplement our code :)
            if (this.Value < other.Value) return -1;
            if (this.Value > other.Value) return 1;
            return 0;
        }

        
    }

}
