using System;
using System.Collections.Generic;
using System.Linq;

namespace Dictionaries
{
    public class Program
    {
        public static void Main()
        {
            var employees = new List<Employee>
            {
                new Employee { Name = "Mahmoud Mattar", Id = 1, ReportTo = null },
                new Employee { Name = "Ali Essam", Id = 2, ReportTo = 1 },
                new Employee { Name = "Alice", Id = 3, ReportTo = 1 },
                new Employee { Name = "Gmal Ahmed", Id = 4, ReportTo = 1 },
                new Employee { Name = "David malan", Id = 6, ReportTo = 2 },
                new Employee { Name = "Eve Morad", Id = 7, ReportTo = 2 },
                new Employee { Name = "Fady", Id = 8, ReportTo = 4 },
                new Employee { Name = "Hany", Id = 9, ReportTo = 4 },
                new Employee { Name = "Ibrahim", Id = 10, ReportTo = 3 },
                new Employee { Name = "Jhon", Id = 11, ReportTo = 6 },
            };

            // Organize employees by manager
            var managers = employees.ToLookup(e => e.ReportTo ?? -1).ToDictionary(g => g.Key, g => g.ToList());

            // Print the hierarchy
            foreach (var entry in managers)
            {
                if (entry.Key == -1 || !managers.ContainsKey(entry.Key))
                    continue;

                var manager = employees.FirstOrDefault(e => e.Id == entry.Key);
                if (manager == null)
                    continue;

                Console.WriteLine(manager);
                foreach (var employee in entry.Value)
                {
                    Console.WriteLine($"\t\t{employee}");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }

    public class Employee : IComparable<Employee>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int? ReportTo { get; set; }

        public int CompareTo(Employee? other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            return $"[{Id}] - {Name}";
        }
    }
}
