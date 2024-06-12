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
                new Customer { Name = "Bahaa Mohammed", Phone = "1+ 98431" }
            };

            var custHashSet1 = new HashSet<Customer>(customers1);
            custHashSet1.Add(new Customer { Name = "Rahma Yasser", Phone = "6+ 010458" }); // duplicate

            Console.WriteLine("HashSet1");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet1)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // --------------------------------------------

            var customers2 = new Customer[]
            {
                new Customer { Name = "Khaled Nabil", Phone = "1+ 36124" },
                new Customer { Name = "Mahmoud Ramadan Ahmed", Phone = "9+ 04254" },
                new Customer { Name = "Rahma Yasser", Phone = "6+ 010458" }, // duplicate, it's in the first list
                new Customer { Name = "Asmaa Adel", Phone = "1+ 24762" }, // duplicate, it's in the first list
                new Customer { Name = "Gamal Ali", Phone = "1+ 71345" }
            };

            var custHashSet2 = new HashSet<Customer>(customers2);

            Console.WriteLine("HashSet2");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet2)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // --------------------------------------------

            var custHashSet3 = new HashSet<Customer>(custHashSet1.Union(custHashSet2)); // union of two hashsets

            Console.WriteLine("HashSet3: Union of HashSet1 and HashSet2");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet3)
            {
                Console.WriteLine(customer);
            }

            var custHashSet4 = new HashSet<Customer>(custHashSet1.Intersect(custHashSet2)); // intersection of two hashsets

            Console.WriteLine("\n\n");
            Console.WriteLine("HashSet4: Intersection of HashSet1 and HashSet2");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet4)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // --------------------------------------------

            var custHashSet5 = new HashSet<Customer>(custHashSet1.Except(custHashSet2)); // elements in the first hashset but not in the second hashset

            Console.WriteLine("HashSet5: Difference of HashSet1 and HashSet2");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet5)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // --------------------------------------------

            var custHashSet6 = new HashSet<Customer>(custHashSet2.Except(custHashSet1)); // elements in the second hashset but not in the first hashset

            Console.WriteLine("HashSet6: Difference of HashSet2 and HashSet1");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet6)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // --------------------------------------------

            custHashSet1.SymmetricExceptWith(custHashSet2); // elements in the first hashset or the second hashset but not in both

            Console.WriteLine("HashSet1: Symmetric difference of HashSet1 and HashSet2");
            Console.WriteLine("--------");
            foreach (var customer in custHashSet1)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\n\n");

            // -------------------------------------------- 
            Console.ReadKey();
        }
    }

    class Customer
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Phone})";
        }

        public override bool Equals(object? obj)
        {
            var customer = obj as Customer;
            if (customer is null)
                return false;

            return this.Phone == customer.Phone;
        }

        public override int GetHashCode()
        {
            return Phone.GetHashCode();
        }
    }
}
