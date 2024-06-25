using System;

namespace DeleGate
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public decimal TotalSales { get; set; }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var employees = new Employee[]
            {
                new Employee {Id =  1, Name = "Mahmoud M", Gender = "Male", TotalSales = 65000m},
                new Employee {Id =  2, Name = "Mpustafa A", Gender = "Male", TotalSales = 50000m},
                new Employee {Id =  3, Name = "Rahma Y", Gender = "Female", TotalSales = 65000m},
                new Employee {Id =  4, Name = "Mohamed S", Gender = "Male", TotalSales = 40000m},
                new Employee {Id =  5, Name = "Asmaa A", Gender = "Female", TotalSales = 42000m},
                new Employee {Id =  6, Name = "Bahaa M", Gender = "Male", TotalSales = 30000m},
                new Employee {Id =  7, Name = "Ahmed N", Gender = "Male", TotalSales = 16000m},
                new Employee {Id =  8, Name = "Sare E", Gender = "Female", TotalSales = 15000m}
            };

            var report = new Report();

            // Using named methods
            report.ProcessEmployee(employees, "Employees With $60,000+ Sales.", IsGreaterThanOrEqual60000);
            report.ProcessEmployee(employees, "Employees With Sales Between $30,000 And $59,999.", IsBetween30000And59999);
            report.ProcessEmployee(employees, "Employees With Sales Less Than $30,000.", IsLessThan30000);

            // Using anonymous delegates
            report.ProcessEmployee(employees, "Employees With $60,000+ Sales.", delegate(Employee emp) { return emp.TotalSales >= 60000m; });
            report.ProcessEmployee(employees, "Employees With Sales Between $30,000 And $59,999.", delegate(Employee emp) { return emp.TotalSales < 60000m && emp.TotalSales >= 30000m; });
            report.ProcessEmployee(employees, "Employees With Sales Less Than $30,000.", delegate(Employee emp) { return emp.TotalSales < 30000m; });

            // Using lambda expressions
            report.ProcessEmployee(employees, "Employees With $60,000+ Sales.", emp => emp.TotalSales >= 60000m);
            report.ProcessEmployee(employees, "Employees With Sales Between $30,000 And $59,999.", emp => emp.TotalSales < 60000m && emp.TotalSales >= 30000m);
            report.ProcessEmployee(employees, "Employees With Sales Less Than $30,000.", emp => emp.TotalSales < 30000m);
        }

        private static bool IsGreaterThanOrEqual60000(Employee emp) => emp.TotalSales >= 60000m;
        private static bool IsBetween30000And59999(Employee emp) => emp.TotalSales < 60000m && emp.TotalSales >= 30000m;
        private static bool IsLessThan30000(Employee emp) => emp.TotalSales < 30000m;
    }

    public class Report
    {
        public delegate bool EligibleSales(Employee emp);

        public void ProcessEmployee(Employee[] employees, string title, EligibleSales isEligible)
        {
            Console.WriteLine(title);
            Console.WriteLine("_____________________________________________");

            foreach (var employee in employees)
            {
                if (isEligible(employee))
                {
                    Console.WriteLine($"{employee.Id} | {employee.Name.PadRight(10, ' ')} | {employee.Gender.PadRight(6, ' ')} | {employee.TotalSales}");
                }
            }
            Console.WriteLine("\n\n");
        }
    }
}
