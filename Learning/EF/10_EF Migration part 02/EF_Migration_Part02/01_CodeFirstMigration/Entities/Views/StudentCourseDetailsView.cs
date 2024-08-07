using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_CodeFirstMigration.Entities.Views
{
    public class StudentCourseDetailsView
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentGender { get; set; }
        public string CourseName { get; set; }
        public decimal CoursePrice { get; set; }
        public string InstructorName { get; set; }
        public string SectionName { get; set; }
        public string ScheduleTitle { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
