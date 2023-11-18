
/*
a delegate is a type that represents references to methods with a particular parameter list and return type.
It is essentially a reference type that can be used to encapsulate a method,
allowing you to treat the method as an object that can be passed around and invoked.
*/

/*
Syntax:
    <delegate> <returnsDataType> <DelegateName>( parameters.....list );
*/

/*
Declaration:
    delegate int MyDelegate(string s);
*/


namespace DeleGate
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            var emps = new Employee[]
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

            report.ProcessEmployee(emps, "Employees With $60,000+ Sales.", IsGreaterThanOrEqual60000);
            report.ProcessEmployee(emps, "Employees With Sales Between $30,000 And $59,999.", IsBetween30000And59999);
            report.ProcessEmployee(emps, "Employees With Sales Less Than $30,000.", IsLessThan30000);


            // anonymous delegate
            report.ProcessEmployee(emps, "Employees With $60,000+ Sales.", delegate (Employee emp){ return emp.TotalSales >= 60000m; });
            report.ProcessEmployee(emps, "Employees With Sales Between $30,000 And $59,999.", delegate (Employee emp) { return emp.TotalSales < 60000m && emp.TotalSales >= 30000m; });
            report.ProcessEmployee(emps, "Employees With Sales Less Than $30,000.", delegate (Employee emp) { return emp.TotalSales < 30000m; });
            
            // Lampda Expression
            report.ProcessEmployee(emps, "Employees With $60,000+ Sales.",  (Employee emp) => emp.TotalSales >= 60000m);
            report.ProcessEmployee(emps, "Employees With Sales Between $30,000 And $59,999.", (Employee emp) => emp.TotalSales < 60000m && emp.TotalSales >= 30000m);
            report.ProcessEmployee(emps, "Employees With Sales Less Than $30,000.", (Employee emp) => emp.TotalSales < 30000m);


            // another Lampda Expression
            report.ProcessEmployee(emps, "Employees With $60,000+ Sales.", emp => emp.TotalSales >= 60000m);
            report.ProcessEmployee(emps, "Employees With Sales Between $30,000 And $59,999.", emp => emp.TotalSales < 60000m && emp.TotalSales >= 30000m);
            report.ProcessEmployee(emps, "Employees With Sales Less Than $30,000.", emp => emp.TotalSales < 30000m);

        }

        static bool IsGreaterThanOrEqual60000(Employee emp) => emp.TotalSales >= 60000m;
        static bool IsBetween30000And59999(Employee emp) => emp.TotalSales < 60000m && emp.TotalSales >= 30000m;
        static bool IsLessThan30000(Employee emp) => emp.TotalSales < 30000m;
    }
}