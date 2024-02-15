/*
Requirements: App to calculates the monthly salary slip

Minimum Hours required: 176 hours (8 * 22)

Basic salary = 176 * Wage

Overtime = (additional hours * 1.25 * hourly cost) / Types of Employees

Directors: 
    manager's allowance (5% of total salary)
Maintenance:
    hardship allowance ($100 / month)
sales:
    commission percentage on volume of sale

Programmers:
    bonus 3% of his total salary if all assigned task were accomplished
*/

using System;

namespace InheritanceFullExample
{

    class Program
    {
        static void Main(string[] args)
        {
            var manger = new Manger(1, "Mahmoud M.", 180, 20);
            Console.WriteLine(manger.ToString());

            var maintanence = new Maintanence(2, "Ali Mohamed", 190, 10);
            Console.WriteLine(maintanence.ToString());


            var sales = new Sales(3, "Mona Gamal", 200, 11, 10000m, 0.10m);
            Console.WriteLine(sales.ToString());

            var developer = new Developer(id: 4, name: "Mahmoud Mohamed", 194, 17, true);

            Console.WriteLine(developer.ToString());
        }
    }
}