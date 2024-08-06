
using System.ComponentModel.DataAnnotations;

namespace _02_CodeFirstMigration.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[fm]$", ErrorMessage = "Gender must be 'f' or 'm'")]
        public char Gender { get; set; }

        // Navigation property
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
