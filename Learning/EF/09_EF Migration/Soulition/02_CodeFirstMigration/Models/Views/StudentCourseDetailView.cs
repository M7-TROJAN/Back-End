namespace _02_CodeFirstMigration.Models.Views
{
    public record StudentCourseDetailView(int StudentID, string StudentName, string Gender, string CourseName, decimal CoursePrice, string SectionName, string InstructorName, string ScheduleTitle, TimeSpan StartTime, TimeSpan EndTime);
}
