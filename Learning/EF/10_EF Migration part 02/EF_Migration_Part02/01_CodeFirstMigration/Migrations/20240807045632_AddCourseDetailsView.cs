using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _01_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW CourseDetailsView AS
            SELECT
                Courses.Id,
                Courses.CourseName,
                Sections.SectionName,
                Instructors.FName + ' ' + Instructors.LName AS InstructorName,
                Schedules.Title AS ScheduleTitle,
                CONVERT(VARCHAR(5), SectionSchedules.StartTime, 108) AS StartTime, 
                CONVERT(VARCHAR(5), SectionSchedules.EndTime, 108) AS EndTime,
                Schedules.SUN,
                Schedules.MON,
                Schedules.TUE,
                Schedules.WED,
                Schedules.THU,
                Schedules.FRI,
                Schedules.SAT
            FROM Sections
            JOIN Courses ON Sections.CourseId = Courses.Id
            JOIN Instructors ON Instructors.Id = Sections.InstructorId
            JOIN SectionSchedules ON SectionSchedules.SectionId = Sections.Id
            JOIN Schedules ON Schedules.Id = SectionSchedules.ScheduleId
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS CourseDetailsView");
        }
    }
}
