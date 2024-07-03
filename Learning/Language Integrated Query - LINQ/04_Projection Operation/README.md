make an structure md file about this topic in linq


```
﻿using LINQTut03.Shared;
using System;
using System.Linq;
namespace LINQTut03.Ex01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employees = Repository.LoadEmployees();
            var femaleWithFnameStartsWithS01 = employees
                .Filter(x =>
                x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));

            femaleWithFnameStartsWithS01.Print("female With Fname Starts With S / Filter");

            var femaleWithFnameStartsWithS02 = employees
               .Where(x =>
               x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));

            femaleWithFnameStartsWithS02.Print("female With Fname Starts With S / Where");
            Console.ReadKey();
        }
    }
}
```

```﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQTut03.Ex02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //var evenNumbers = numbers.Where(x => x % 2 == 0);
            IEnumerable<int> evenNumbers = 
                numbers.Where(x => x % 2 == 0); // construction (lazy loading)
    
            numbers.Add(10);
            numbers.Add(12);
            numbers.Remove(4);

            // [1]  ===>   2, 4, 6, 8
            // [2]  ===>   2, 6, 8, 10, 12
            foreach (var n in evenNumbers) // enumeration (immediate execution)
            {
                Console.Write($" {n}");
            }

            Console.ReadKey();
        }
    }
}
```

```﻿using LINQTut03.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQTut03.Ex03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var evenNumbersUsingExtensionWhere =
                numbers.Where(x => x % 2 == 0);

            var evenNumbersUsingEnumerableWhereMethod =
                Enumerable.Where(numbers, x => x % 2 == 0);

            // Select n fROM numbers where n % 2 = 0;
            var evenNumbersUsingQuerySyntax =
                 from n in numbers
                 where n % 2 == 0
                 select n;


            evenNumbersUsingExtensionWhere.Print("evenNumbersUsingExtensionWhere");
            evenNumbersUsingEnumerableWhereMethod.Print("evenNumbersUsingEnumerableWhereMethod");
            evenNumbersUsingQuerySyntax.Print("evenNumbersUsingQuerySyntax");


            Console.ReadKey();
        }
    }
}
```

```

﻿using LINQTut03.Shared;
using System;
using System.Linq;

namespace LINQTut03.Ex04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employees = Repository.LoadEmployees();

            var empMale = employees.Where(x => x.Gender == "male");

            var empsSalaryOver300K = employees.Where(x => x.Salary >= 300_000);

            empMale.Print("Male Employees");
            
            empsSalaryOver300K.Print("Employees with Salary >= 300K");

            var empMaleInHRDepartment = 
                empMale.Where(x => x.Department.ToLowerInvariant() == "hr");
            empMaleInHRDepartment.Print("Male Employees In HR");

            Console.ReadKey();
        }
    }
}

```


```
﻿
using System;

namespace LINQTut03.Shared
{
    public class Employee
    {
        public Employee() { }
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public string Gender { get; set; }

        public string Department { get; set; }
         
        public bool HasHealthInsurance { get; set; }

        public bool HasPensionPlan { get; set; }

        public decimal Salary { get; set; }

        public override string ToString()
        {
            return
                    string.Format($"" +
                    $"{Id}\t" +
                    $" {String.Concat(LastName, ", ", FirstName).PadRight(15, ' ')}\t" + 
                    $"{HireDate.Date.ToShortDateString()}\t" +
                    $"{Gender.PadRight(10, ' ')}\t" +
                    $"{Department.PadRight(10, ' ')}\t" +
                    $"{HasHealthInsurance}\t" +
                    $"{HasPensionPlan}\t" +
                    $"${Salary.ToString("0.00")}");
        }
    }

    
}
```

```
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQTut03.Shared
{
    public static class ExtensionFunctional
    {

        public static IEnumerable<Employee> Filter
            (this IEnumerable<Employee> source, Func<Employee, bool> predicate)
        {
             
            foreach (var employee in source)
            { 
                if (predicate(employee))
                {
                    yield return employee;
                }
            }
        } 

        public static void Print<T>(this IEnumerable<T> source, string title)
        {
            if (source == null)
                return;
            Console.WriteLine();
            Console.WriteLine("┌───────────────────────────────────────────────────────┐");
            Console.WriteLine($"│   {title.PadRight(52, ' ')}│");
            Console.WriteLine("└───────────────────────────────────────────────────────┘");
            Console.WriteLine();
            foreach (var item in source)
            {
                if (typeof(T).IsValueType) 
                    Console.Write($" {item} "); // 1, 2, 3
                else
                    Console.WriteLine(item);
            }
                
         
        }
    }
}
```


```
﻿
using System;
using System.Collections.Generic; 

namespace LINQTut03.Shared
{
    public static class Repository
    {

        public static IEnumerable<Employee> LoadEmployees()
        {
            return new List<Employee> 
            {
                new Employee
                {
                        Id = 1001,
                        FirstName = "Cochran",
                        LastName = "Cole",
                        HireDate = new DateTime(2017, 11, 2),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 103200m
                },
                new Employee
                {
                        Id = 1002,
                        FirstName = "Jaclyn",
                        LastName = "Wolfe",
                        HireDate = new DateTime(2018, 4, 14),
                        Gender = "female",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 192400m
                },
                new Employee
                {
                        Id = 1003,
                        FirstName = "Warner",
                        LastName = "Jones",
                        HireDate = new DateTime(2016, 12, 13),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 172800m
                },
                new Employee
                {
                        Id = 1004,
                        FirstName = "Hester",
                        LastName = "Evans",
                        HireDate = new DateTime(2016, 8, 17),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 155500m
                },
                new Employee
                {
                        Id = 1005,
                        FirstName = "Wallace",
                        LastName = "Buck",
                        HireDate = new DateTime(2014, 5, 12),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 315800m
                },
                new Employee
                {
                        Id = 1006,
                        FirstName = "Acevedo",
                        LastName = "Wall",
                        HireDate = new DateTime(2020, 10, 30),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 343700m
                },
                new Employee
                {
                        Id = 1007,
                        FirstName = "Jacqueline",
                        LastName = "Pickett",
                        HireDate = new DateTime(2021, 2, 17),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 370000m
                },
                new Employee
                {
                        Id = 1008,
                        FirstName = "Oconnor",
                        LastName = "Espinoza",
                        HireDate = new DateTime(2017, 3, 12),
                        Gender = "male",
                        Department = "HR",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 155600m
                },
                new Employee
                {
                        Id = 1009,
                        FirstName = "Allie",
                        LastName = "Elliott",
                        HireDate = new DateTime(2020, 4, 20),
                        Gender = "female",
                        Department = "Accounting",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 315400m
                },
                new Employee
                {
                        Id = 1010,
                        FirstName = "Elva",
                        LastName = "Decker",
                        HireDate = new DateTime(2016, 9, 6),
                        Gender = "female",
                        Department = "HR",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 345900m
                },
                new Employee
                {
                        Id = 1011,
                        FirstName = "Hayes",
                        LastName = "Beasley",
                        HireDate = new DateTime(2020, 4, 25),
                        Gender = "male",
                        Department = "HR",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 372700m
                },
                new Employee
                {
                        Id = 1012,
                        FirstName = "Florine",
                        LastName = "Cervantes",
                        HireDate = new DateTime(2015, 3, 25),
                        Gender = "female",
                        Department = "FIMAMCE",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 338700m
                },
                new Employee
                {
                        Id = 1013,
                        FirstName = "Bullock",
                        LastName = "Carney",
                        HireDate = new DateTime(2017, 1, 3),
                        Gender = "male",
                        Department = "Accounting",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 214400m
                },
                new Employee
                {
                        Id = 1014,
                        FirstName = "Carroll",
                        LastName = "Cantu",
                        HireDate = new DateTime(2021, 5, 26),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 343200m
                },
                new Employee
                {
                        Id = 1015,
                        FirstName = "Debra",
                        LastName = "Hogan",
                        HireDate = new DateTime(2019, 10, 4),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 249100m
                },
                new Employee
                {
                        Id = 1016,
                        FirstName = "Winnie",
                        LastName = "Mccall",
                        HireDate = new DateTime(2019, 7, 17),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 287300m
                },
                new Employee
                {
                        Id = 1017,
                        FirstName = "Manuela",
                        LastName = "Berger",
                        HireDate = new DateTime(2015, 12, 11),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 172500m
                },
                new Employee
                {
                        Id = 1018,
                        FirstName = "Lakeisha",
                        LastName = "Lowe",
                        HireDate = new DateTime(2017, 1, 18),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 314300m
                },
                new Employee
                {
                        Id = 1019,
                        FirstName = "Stewart",
                        LastName = "Lott",
                        HireDate = new DateTime(2016, 12, 12),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 146600m
                },
                new Employee
                {
                        Id = 1020,
                        FirstName = "Stafford",
                        LastName = "Peck",
                        HireDate = new DateTime(2014, 9, 25),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 320700m
                },
                new Employee
                {
                        Id = 1021,
                        FirstName = "Barron",
                        LastName = "Bird",
                        HireDate = new DateTime(2020, 5, 18),
                        Gender = "male",
                        Department = "HR",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 151200m
                },
                new Employee
                {
                        Id = 1022,
                        FirstName = "Nona",
                        LastName = "Brooks",
                        HireDate = new DateTime(2015, 12, 4),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 136500m
                },
                new Employee
                {
                        Id = 1023,
                        FirstName = "Clara",
                        LastName = "Reeves",
                        HireDate = new DateTime(2014, 12, 6),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 245800m
                },
                new Employee
                {
                        Id = 1024,
                        FirstName = "Karin",
                        LastName = "Blanchard",
                        HireDate = new DateTime(2018, 1, 20),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 341200m
                },
                new Employee
                {
                        Id = 1025,
                        FirstName = "Burris",
                        LastName = "Morgan",
                        HireDate = new DateTime(2019, 7, 6),
                        Gender = "male",
                        Department = "Accounting",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 360300m
                },
                new Employee
                {
                        Id = 1026,
                        FirstName = "Owen",
                        LastName = "Cortez",
                        HireDate = new DateTime(2021, 12, 9),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 193700m
                },
                new Employee
                {
                        Id = 1027,
                        FirstName = "Letha",
                        LastName = "Finch",
                        HireDate = new DateTime(2016, 12, 18),
                        Gender = "female",
                        Department = "Accounting",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 357200m
                },
                new Employee
                {
                        Id = 1028,
                        FirstName = "Sondra",
                        LastName = "Rojas",
                        HireDate = new DateTime(2016, 4, 22),
                        Gender = "female",
                        Department = "Accounting",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 309700m
                },
                new Employee
                {
                        Id = 1029,
                        FirstName = "Hoover",
                        LastName = "Cook",
                        HireDate = new DateTime(2020, 12, 17),
                        Gender = "male",
                        Department = "HR",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 282200m
                },
                new Employee
                {
                        Id = 1030,
                        FirstName = "Wanda",
                        LastName = "Bender",
                        HireDate = new DateTime(2021, 6, 17),
                        Gender = "female",
                        Department = "Accounting",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 294200m
                },
                new Employee
                {
                        Id = 1031,
                        FirstName = "Sanford",
                        LastName = "Craig",
                        HireDate = new DateTime(2020, 2, 27),
                        Gender = "male",
                        Department = "Accounting",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 278200m
                },
                new Employee
                {
                        Id = 1032,
                        FirstName = "Christy",
                        LastName = "Middleton",
                        HireDate = new DateTime(2021, 4, 2),
                        Gender = "female",
                        Department = "FIMAMCE",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 377400m
                },
                new Employee
                {
                        Id = 1033,
                        FirstName = "Day",
                        LastName = "Brady",
                        HireDate = new DateTime(2019, 1, 23),
                        Gender = "male",
                        Department = "HR",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 142600m
                },
                new Employee
                {
                        Id = 1034,
                        FirstName = "Powers",
                        LastName = "Beard",
                        HireDate = new DateTime(2014, 4, 25),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 224000m
                },
                new Employee
                {
                        Id = 1035,
                        FirstName = "Arline",
                        LastName = "Pratt",
                        HireDate = new DateTime(2017, 8, 12),
                        Gender = "female",
                        Department = "FIMAMCE",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 360300m
                },
                new Employee
                {
                        Id = 1036,
                        FirstName = "Sharpe",
                        LastName = "Cardenas",
                        HireDate = new DateTime(2017, 11, 28),
                        Gender = "male",
                        Department = "Accounting",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 266100m
                },
                new Employee
                {
                        Id = 1037,
                        FirstName = "Madeleine",
                        LastName = "Stanton",
                        HireDate = new DateTime(2020, 7, 17),
                        Gender = "female",
                        Department = "Accounting",
                        HasHealthInsurance = true,
                        HasPensionPlan = true,
                        Salary = 198300m
                },
                new Employee
                {
                        Id = 1038,
                        FirstName = "Spears",
                        LastName = "Noble",
                        HireDate = new DateTime(2014, 10, 6),
                        Gender = "male",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 176300m
                },
                new Employee
                {
                        Id = 1039,
                        FirstName = "Gonzalez",
                        LastName = "Gilliam",
                        HireDate = new DateTime(2021, 4, 29),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 394300m
                },
                new Employee
                {
                        Id = 1040,
                        FirstName = "Abigail",
                        LastName = "Bradford",
                        HireDate = new DateTime(2018, 4, 2),
                        Gender = "female",
                        Department = "FIMAMCE",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 296100m
                },
                new Employee
                {
                        Id = 1041,
                        FirstName = "Ashlee",
                        LastName = "Farmer",
                        HireDate = new DateTime(2020, 9, 24),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = true,
                        Salary = 125300m
                },
                new Employee
                {
                        Id = 1042,
                        FirstName = "Glover",
                        LastName = "Lloyd",
                        HireDate = new DateTime(2014, 2, 15),
                        Gender = "male",
                        Department = "IT",
                        HasHealthInsurance = true,
                        HasPensionPlan = false,
                        Salary = 123000m
                },
                new Employee
                {
                        Id = 1043,
                        FirstName = "Cleo",
                        LastName = "Mays",
                        HireDate = new DateTime(2018, 4, 24),
                        Gender = "female",
                        Department = "IT",
                        HasHealthInsurance = false,
                        HasPensionPlan = false,
                        Salary = 214900m
            };
        } 
    }
}
```


