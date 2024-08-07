using _01_CodeFirstMigration.Data;
using _01_CodeFirstMigration.Entities;
using _01_CodeFirstMigration.Entities.Views;
using Microsoft.EntityFrameworkCore;

namespace _01_CodeFirstMigration
{
    internal class Program
    {
        static void Main()
        {
            var coursesDetails = GetCourseDetails();

            PrintTable(coursesDetails);

            Console.ReadKey();
        }

        public static List<CourseDetailsView> GetCourseDetails()
        {
            using (var context = new AppDbContext())
            {
                return context.courseDetailsViews.ToList();
            }
        }

        public static void PrintTable(List<CourseDetailsView> details)
        {
            string[] days = { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };

            Console.WriteLine("| Id | Course Name      | Section Name | Instructor Name | ScheduleTitle | Start Time | End Time | SUN | MON | TUE | WED | THU | FRI | SAT |");
            Console.WriteLine("|----|------------------|--------------|-----------------|---------------|------------|----------|-----|-----|-----|-----|-----|-----|-----|");

            foreach (var detail in details)
            {
                Console.WriteLine($"| {detail.Id.ToString("D2")} | {detail.CourseName.PadRight(16,' ')} | {detail.SectionName.PadRight(12, ' ')} | {detail.InstructorName.PadRight(15, ' ')} | {detail.ScheduleTitle.PadRight(13, ' ')} | {detail.StartTime.PadRight(10, ' ')} | {detail.EndTime.PadRight(8, ' ')} | {FormatDay(detail.SUN).PadRight(3, ' ')} | {FormatDay(detail.MON).PadRight(3, ' ')} | {FormatDay(detail.TUE).PadRight(3, ' ')} | {FormatDay(detail.WED).PadRight(3, ' ')} | {FormatDay(detail.THU).PadRight(3, ' ')} | {FormatDay(detail.FRI).PadRight(3, ' ')} | {FormatDay(detail.SAT).PadRight(3, ' ')} |");
            }

            Console.WriteLine("|----|------------------|--------------|-----------------|---------------|------------|----------|-----|-----|-----|-----|-----|-----|-----|");
        }

        public static string FormatDay(bool isScheduled)
        {
            return isScheduled ? "✓" : " ";
        }

    }
}
