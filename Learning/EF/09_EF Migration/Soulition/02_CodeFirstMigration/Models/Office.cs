namespace _02_CodeFirstMigration.Models
{
    public class Office
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
        public string OfficeLocation { get; set; }

        // Navigation property
        public ICollection<Instructor> Instructors { get; set; }
    }
}
