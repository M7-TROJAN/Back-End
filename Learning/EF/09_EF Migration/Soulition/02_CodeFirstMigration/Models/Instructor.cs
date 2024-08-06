namespace _02_CodeFirstMigration.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OfficeId { get; set; }

        // Navigation property
        public ICollection<Section> Sections { get; set; } = new List<Section>();

        // Navigation property
        public Office Office { get; set; }
    }
}
