using System;
using System.Collections.Generic;

namespace CAHashSetsAndSortedSets
{
    public class Program
    {
        /* 
        Important Note:
        - HashSet<T> is a collection of unique elements.
        To ensure that a HashSet correctly identifies duplicates, follow these guidelines:
        - For primitive types, the HashSet can automatically detect duplicates.
        - For reference types, you need to override the `GetHashCode` and `Equals` methods 
          to ensure proper comparison of objects.
        */
        public static void Main()
        {
            var customers1 = new List<Customer>
            {
                new Customer { Name = "Mahmoud Mattar", Phone = "1+ 23456" },
                new Customer { Name = "Mahmoud Mattar", Phone = "1+ 23456" }, // duplicate
                new Customer { Name = "Ahmed Hammed", Phone = "9+ 01254" },
                new Customer { Name = "Rahma Yasser", Phone = "6+ 010458" },
                new Customer { Name = "Asmaa Adel", Phone = "1+ 24762" },
                new Customer { Name = "Bahaa Mohammed", Phone = "1+ 98431" },
                new Customer { Name = "Ahmed Hammed", Phone = "9+ 01254" }, // duplicate
                new Customer { Name = "Ahmed Nabil", Phone = "6+ 010058" },
            };

            Console.WriteLine("SortedSet");
            Console.WriteLine("--------");

            var sortedList = new SortedSet<Customer>(customers1); // sorted by name then phone

            foreach (var customer in sortedList)
            {
                Console.WriteLine(customer);
            }

            Console.ReadKey();
        }
    }

    class Customer : IComparable<Customer>, IEquatable<Customer>
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Phone})";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Customer);
        }

        public bool Equals(Customer? other)
        {
            if (other is null)
                return false;

            return this.Phone == other.Phone && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 19;
            hashCode = hashCode * 31 + (Phone?.GetHashCode() ?? 0);
            hashCode = hashCode * 31 + (Name?.GetHashCode() ?? 0);
            return hashCode;
        }

        public int CompareTo(Customer? other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (other is null)
                return 1;

            int nameComparison = this.Name.CompareTo(other.Name);
            if (nameComparison == 0)
            {
                return this.Phone.CompareTo(other.Phone);
            }

            return nameComparison;
        }
    }
}
