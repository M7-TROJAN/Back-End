using _01_CodeFirstMigration.Data;
using Microsoft.EntityFrameworkCore;

namespace _01_CodeFirstMigration
{
    internal class Program
    {
        static void Main()
        {
            using (var context = new AppDbContext())
            {

                var instructors = context.Instructors
                    .Include(x => x.Sections)
                    .ThenInclude(x => x.Course)
                    .Include(x => x.Office);

                Console.WriteLine("-------------------------------------------------------------------------------------");
                Console.WriteLine("| Id |  Name                | Office | Courses Taught                               |");
                Console.WriteLine("|----|----------------------|--------|----------------------------------------------|");

                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"| {instructor.Id.ToString().PadLeft(2, '0')} | {instructor.FName + " " + instructor.LName,-20} | {instructor.Office?.OfficeName,-6} | {string.Join(", ", instructor.Sections.Select(x => x.Course.CourseName)),-44} |");
                }
                Console.WriteLine("-------------------------------------------------------------------------------------");


                Console.WriteLine("\n\n------------------------------------------------------\n\n");

                var sections = context.Sections
                    .Include(x => x.Course)
                    .Include(x => x.Instructor)
                    .Include(x => x.Schedule);

                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("| Id |  Course          | Section | Instructor           | Schedule       | Time Slot     | SUN | MON | TUE | WED | THU | FRI | SAT |");
                Console.WriteLine("|----|------------------|---------|----------------------|----------------|---------------|-----|-----|-----|-----|-----|-----|-----|");

                foreach (var section in sections)
                {
                    string sunday = section.Schedule.SUN ? " ✓" : "";
                    string monday = section.Schedule.MON ? " ✓" : "";
                    string tuesday = section.Schedule.TUE ? " ✓" : "";
                    string wednesday = section.Schedule.WED ? " ✓" : "";
                    string thursday = section.Schedule.THU ? " ✓" : "";
                    string friday = section.Schedule.FRI ? " ✓" : "";
                    string saturday = section.Schedule.SAT ? " ✓" : "";

                    Console.WriteLine($"| {section.Id.ToString().PadLeft(2, '0')} | {section.Course.CourseName,-16} | {section.SectionName,-7} | {(section.Instructor?.FName + " " + section.Instructor?.LName),-20} | {section.Schedule.Title,-14} | {section.TimeSlot,-9} | {sunday,-3} | {monday,-3} | {tuesday,-3} | {wednesday,-3} | {thursday,-3} | {friday,-3} | {saturday,-3} |");
                }

                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");



                Console.WriteLine("\n\n------------------------------------------------------\n\n");

                var students = context.Students
                    .Include(x => x.Sections)
                    .ThenInclude(x => x.Course);

                Console.WriteLine("----------------------------------------------------------------------------");
                Console.WriteLine("| Id |  Name                | Courses Enrolled                             |");
                Console.WriteLine("|----|----------------------|----------------------------------------------|");

                foreach (var student in students)
                {
                    Console.WriteLine($"| {student.Id.ToString().PadLeft(2, '0')} | {student.FName + " " + student.LName,-20} | {string.Join(", ", student.Sections.Select(x => x.Course.CourseName)),-44} |");
                }
                Console.WriteLine("----------------------------------------------------------------------------");

            }

            Console.ReadKey();
        }

    }
}
