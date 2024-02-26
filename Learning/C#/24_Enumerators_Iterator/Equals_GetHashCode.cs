// The Example demonstrates how to compare objects for equality based on their property values 
// and how to generate consistent hash codes for objects with overridden Equals and GetHashCode methods.

using System;
namespace CSEnumeratorEnumerable
{
    class Program
    {
        static void Main()
        {

            var e1 = new Employee { Id = 1, Name = "Mahmoud Mattar", Department = "IT", Salary = 50000 };

            // Create a new employee with the same details as e1
            var e2 = new Employee { Id = 1, Name = "Mahmoud Mattar", Department = "IT", Salary = 50000 };

            // Compare e1 with e2 

            // Check for reference equality using the == operator 
            Console.WriteLine(e1 == e2); // false

            // Check for value equality using the Equals method
            Console.WriteLine(e1.Equals(e2)); // true

            // Check for hash code equality
            Console.WriteLine(e1.GetHashCode() == e2.GetHashCode()); // true

        }
    }


    public class Employee
    {

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public decimal Salary { get; set; }

        // Override Equals method for value comparison
        public override bool Equals(object? obj)
        {
            // Check for null and type mismatch
            if (obj == null || !(obj is Employee))
                return false;

            // Reference equality check
            if (ReferenceEquals(this, obj))
                return true;

            // Cast obj to Employee
            Employee other = (Employee)obj;

            // Compare properties for equality
            return this.Id == other.Id 
                && this.Name == other.Name 
                && this.Department == other.Department 
                && this.Salary == other.Salary;
        }

        // Override GetHashCode for consistent hashing
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Department, Salary);


            // we also can make our algorthism 
            int hash = 17;
            hash = (hash * 23) + Id.GetHashCode();
            hash = (hash * 13) ^ Name.GetHashCode();
            hash = (hash * 7) + Department.GetHashCode();
            hash = (hash * 7) | Salary.GetHashCode();
            return hash;
        }
    }
}
