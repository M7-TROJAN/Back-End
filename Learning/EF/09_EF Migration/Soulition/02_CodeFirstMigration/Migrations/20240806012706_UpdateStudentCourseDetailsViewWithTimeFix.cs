using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentCourseDetailsViewWithTimeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                // Drop the existing view
                migrationBuilder.Sql("DROP VIEW IF EXISTS StudentCourseDetails;");

                // Create the new view with the updated query
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
                    CONVERT(VARCHAR(5), SectionSchedules.StartTime, 108) AS StartTime, 
                    CONVERT(VARCHAR(5), SectionSchedules.EndTime, 108) AS EndTime
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
                    Schedules ON SectionSchedules.ScheduleId = Schedules.Id;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
                // Drop the updated view
                migrationBuilder.Sql("DROP VIEW IF EXISTS StudentCourseDetails;");

                // Optionally, recreate the previous version of the view
                // If this is the initial creation, you can omit this part
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
                    CONVERT(VARCHAR(8), SectionSchedules.StartTime, 108) AS StartTime, 
                    CONVERT(VARCHAR(8), SectionSchedules.EndTime, 108) AS EndTime
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
                    Schedules ON SectionSchedules.ScheduleId = Schedules.Id;
            ");
        }
    }
}
