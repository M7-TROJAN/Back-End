
using System.ComponentModel.DataAnnotations;

namespace _02_CodeFirstMigration.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public bool SUN { get; set; }

        [Required]
        public bool MON { get; set; }

        [Required]
        public bool TUE { get; set; }

        [Required]
        public bool WED { get; set; }

        [Required]
        public bool THU { get; set; }

        [Required]
        public bool FRI { get; set; }

        [Required]
        public bool SAT { get; set; }

        // Navigation property
        public ICollection<SectionSchedule> SectionSchedules { get; set; }
    }
}
