using System;


namespace DeleGate
{
    
    public static class Report
    {
        public delegate bool EligibleSales(Employee employee);

        private const int IdColumnWidth = 2;
        private const int NameColumnWidth = 10;
        private const int GenderColumnWidth = 6;
        private const int TotalSalesColumnWidth = 8;

        public static void ProcessEmployee(Employee[] employees, string title, EligibleSales isEligible)
        {
            int eligibleCount = 0;

            Console.WriteLine(title);
            Console.WriteLine("=====================================");
            Console.WriteLine($"{"Id".PadRight(IdColumnWidth)} | {"Name".PadRight(NameColumnWidth)} | {"Gender".PadRight(GenderColumnWidth)} | {"TotalSales".PadRight(TotalSalesColumnWidth)}");

            foreach (var emp in employees)
            {
                if (isEligible(emp))
                {
                    eligibleCount++;
                    Console.WriteLine($"{emp.Id.ToString().PadRight(IdColumnWidth)} | {emp.Name.PadRight(NameColumnWidth)} | {emp.Gender.PadRight(GenderColumnWidth)} | {emp.TotalSales.ToString().PadRight(TotalSalesColumnWidth)}");
                }
            }

            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Total Eligible Employees: {eligibleCount}\n\n");
        }
    }
}
