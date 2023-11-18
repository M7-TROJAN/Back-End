using System;


namespace DeleGate
{
    public class Report
    {

        public delegate bool IllegibleSales(Employee emp);

        public void ProcessEmployee(Employee[] employees, string tittle, IllegibleSales isIllegible)
        {
            Console.WriteLine(tittle);
            Console.WriteLine("_____________________________________________");

            foreach (Employee e in employees)
            {
                if (isIllegible(e))
                {
                    Console.WriteLine($"{e.Id}  | {e.Name.PadRight(10, ' ')} | {e.Gender.PadRight(6, ' ')}  | {e.TotalSales}");
                }
            }
            Console.WriteLine("\n\n");
        }
    }
}
