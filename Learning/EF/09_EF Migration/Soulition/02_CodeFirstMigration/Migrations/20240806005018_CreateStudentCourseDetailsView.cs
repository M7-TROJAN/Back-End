using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentCourseDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW StudentCourseDetails AS
            SELECT 
                Students.Id AS StudentID, 
                Students.Name AS StudentName, 
                CASE 
                    WHEN Students.Gender = 'f' THEN 'Female'
                    WHEN Students.Gender = 'm' THEN 'Male'
                END AS Gender, 
                Courses.CourseName, 
                Courses.Price AS CoursePrice, 
                Sections.SectionName, 
                Instructors.Name AS InstructorName, 
                Schedules.Title AS ScheduleTitle, 
                SectionSchedules.StartTime, 
                SectionSchedules.EndTime
            FROM 
                Students
            JOIN 
                Enrollments ON Students.Id = Enrollments.StudentId
            JOIN 
                Sections ON Enrollments.SectionId = Sections.Id
            JOIN 
                Courses ON Sections.CourseId = Courses.Id
            JOIN 
                Instructors ON Sections.InstructorId = Instructors.Id
            JOIN 
                SectionSchedules ON Sections.Id = SectionSchedules.SectionId
            JOIN 
                Schedules ON SectionSchedules.ScheduleId = Schedules.Id;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS StudentCourseDetails;");
        }
    }
}
