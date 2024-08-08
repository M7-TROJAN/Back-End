namespace _01_CodeFirstMigration.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }

        public int? OfficeId { get; set; }
        public Office? Office { get; set; } // Navigation property

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
