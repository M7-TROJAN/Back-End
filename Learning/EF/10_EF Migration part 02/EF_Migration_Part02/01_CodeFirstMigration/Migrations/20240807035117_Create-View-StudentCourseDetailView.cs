using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _01_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class CreateViewStudentCourseDetailView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW StudentCourseDetailsView AS
            SELECT 
                Students.Id AS StudentId, 
                Students.FName + ' ' + Students.LName AS StudentName, 
                CASE 
                    WHEN Students.Gender = 'm' THEN 'Male'
                    WHEN Students.Gender = 'f' THEN 'Female'
                    ELSE Students.Gender
                END AS StudentGender,
                Courses.CourseName,
                Courses.Price As CoursePrice,
                Instructors.FName + ' ' + Instructors.LName AS InstructorName,
                Sections.SectionName, 
                Schedules.Title As ScheduleTitle, 
                CONVERT(VARCHAR(5), SectionSchedules.StartTime, 108) AS StartTime, 
                CONVERT(VARCHAR(5), SectionSchedules.EndTime, 108) AS EndTime
            FROM 
                Students
            JOIN Enrollments ON Students.Id = Enrollments.StudentId
            JOIN Sections ON Enrollments.SectionId = Sections.Id
            JOIN Courses ON Sections.CourseId = Courses.Id
            JOIN SectionSchedules ON Sections.Id = SectionSchedules.SectionId
            JOIN Schedules ON SectionSchedules.ScheduleId = Schedules.Id
            JOIN Instructors ON Sections.InstructorId = Instructors.Id;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS StudentCourseDetailsView;");
        }
    }
}
