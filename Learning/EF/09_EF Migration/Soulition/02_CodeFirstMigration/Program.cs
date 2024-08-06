using _02_CodeFirstMigration.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _02_CodeFirstMigration
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (var context = new AppDbContext())
            {
                try
                {
                    var instructors = context.Instructors
                                 .Include(i => i.Sections) // Eager loading the Sections
                                 .Include(i => i.Office)
                                 .ToList();

                    Console.WriteLine("\nInstructors:\n");
                    foreach (var instructor in instructors)
                    {
                        Console.WriteLine(instructor.Name);
                        Console.WriteLine($"\tOffice: {instructor.Office.OfficeName}");
                        Console.WriteLine("\tSections:");
                        foreach (var section in instructor.Sections)
                        {
                            Console.WriteLine($"\t\t{section.SectionName}");
                        }
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var courses = context.Courses
                                 .Include(c => c.Sections) // Eager loading the Sections
                                 .ThenInclude(s => s.Instructor) // Eager loading the Instructor
                                 .ThenInclude(i => i.Office) // Eager loading the Office
                                 .ToList();

                    Console.WriteLine("\nCourses:\n");
                    foreach (var course in courses)
                    {
                        Console.WriteLine(course.CourseName);
                        Console.WriteLine("\tSections:");
                        foreach (var section in course.Sections)
                        {
                            Console.WriteLine($"\t\t{section.SectionName} - {section.Instructor.Name} - {section.Instructor.Office.OfficeName}");
                        }
                    }



                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var schedules = context.Schedules.ToList();

                    Console.WriteLine("\nSchedules:\n");
                    foreach (var schedule in schedules)
                    {
                        Console.WriteLine(schedule.Title);
                        Console.WriteLine($"\tSUN: {schedule.SUN}");
                        Console.WriteLine($"\tMON: {schedule.MON}");
                        Console.WriteLine($"\tTUE: {schedule.TUE}");
                        Console.WriteLine($"\tWED: {schedule.WED}");
                        Console.WriteLine($"\tTHU: {schedule.THU}");
                        Console.WriteLine($"\tFRI: {schedule.FRI}");
                        Console.WriteLine($"\tSAT: {schedule.SAT}");
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var sectionSchedules = context.SectionSchedules
                                 .Include(ss => ss.Section)
                                 .Include(ss => ss.Schedule)
                                 .ToList();

                    Console.WriteLine("\nSectionSchedules:\n");
                    foreach (var sectionSchedule in sectionSchedules)
                    {
                        Console.WriteLine($"{sectionSchedule.Section.SectionName} - {sectionSchedule.Schedule.Title}");
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var offices = context.Offices
                        .Include(o => o.Instructors)
                        .ToList();

                    Console.WriteLine("\nOffices:\n");
                    foreach (var office in offices)
                    {
                        Console.WriteLine(office.OfficeName);
                        Console.WriteLine("\tInstructors:");
                        foreach (var instructor in office.Instructors)
                        {
                            Console.WriteLine($"\t\t{instructor.Name}");
                        }
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var sections = context.Sections
                        .Include(s => s.Course)
                        .Include(s => s.Instructor)
                        .Include(s => s.SectionSchedules)
                        .ToList();

                    Console.WriteLine("\nSections:\n");
                    foreach (var section in sections)
                    {
                        Console.WriteLine($"{section.SectionName} - {section.Course.CourseName} - {section.Instructor.Name}");
                        Console.WriteLine("\tSchedules:");
                        foreach (var sectionSchedule in section.SectionSchedules)
                        {
                            Console.WriteLine($"\t\t{sectionSchedule.Schedule.Title}");
                        }
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var students = context.Students
                        .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Section)
                        .ThenInclude(s => s.Course)
                        .ToList();

                    Console.WriteLine("\nStudents:\n");
                    foreach (var student in students)
                    {
                        Console.WriteLine(student.Name);
                        Console.WriteLine("\tEnrollments:");
                        foreach (var enrollment in student.Enrollments)
                        {
                            Console.WriteLine($"\t\t{enrollment.Section.SectionName} - {enrollment.Section.Course.CourseName}");
                        }
                    }


                    Console.WriteLine("\n\n--------------------------------------\n\n");

                    var enrollments = context.Enrollments
                        .Include(e => e.Student)
                        .Include(e => e.Section)
                        .ThenInclude(s => s.Course)
                        .ToList();

                    Console.WriteLine("\nEnrollments:\n");
                    foreach (var enrollment in enrollments)
                    {
                        Console.WriteLine($"{enrollment.Student.Name} - {enrollment.Section.SectionName} - {enrollment.Section.Course.CourseName}");
                    }

                    Console.WriteLine("\n\n--------------------------------------\n\n");


                    var details = context.StudentCourseDetailViews.ToList();

                    Console.WriteLine("\nStudentCourseDetailViews:\n");
                    foreach (var detail in details)
                    {
                        Console.WriteLine($"{detail.StudentName} ({detail.Gender}) is enrolled in {detail.CourseName} taught by {detail.InstructorName} at {detail.ScheduleTitle} from {detail.StartTime} to {detail.EndTime}\n");
                    }


                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Done!");
                }

            }

            Console.ReadKey();
        }
    }
}
