namespace _02_CodeFirstMigration.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Section Section { get; set; }
    }
}