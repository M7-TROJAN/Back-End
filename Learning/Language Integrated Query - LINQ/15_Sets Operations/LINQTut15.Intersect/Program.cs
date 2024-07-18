using Shared;
using System;
using System.Linq;

namespace LINQTut15.Intersect
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var set1 = Repository.Meeting1.Participants;
            var set2 = Repository.Meeting2.Participants;

            set1.Print($"========= Meeting 1 Participants ({set1.Count()})");
            set2.Print($"========= Meeting 2 Participants ({set2.Count()})");

            var set3 = set1.Intersect(set2); // set1 ∩ set2 => emplyees that are in both set1 and set2 based on reference equality
            set3.Print($"========= set1.Intersect(set2) Participants ({set3.Count()})");

            var set4 = set1.IntersectBy(set2.Select(x => x.EmployeeNo), x => x.EmployeeNo); // set1 ∩ set2 => emplyees that are in both set1 and set2 based on EmployeeNo
            set4.Print($"========= set1.IntersectBy(set2.Select(x => x.EmployeeNo) Participants ({set4.Count()})");
            Console.ReadKey();
        }
    }
}
