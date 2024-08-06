namespace _02_CodeFirstMigration.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string? SectionName { get; set; }
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }

        public Course Course { get; set; }
        public Instructor Instructor { get; set; }

        // Navigation properties
        public ICollection<SectionSchedule> SectionSchedules { get; set; } = new List<SectionSchedule>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

}
