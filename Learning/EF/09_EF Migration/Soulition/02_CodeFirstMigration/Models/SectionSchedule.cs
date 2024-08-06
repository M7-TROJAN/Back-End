
using System.ComponentModel.DataAnnotations;

namespace _02_CodeFirstMigration.Models
{
    public class SectionSchedule
    {
        public int Id { get; set; }

        public int SectionId { get; set; }

        public int ScheduleId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        // Navigation properties
        public Section Section { get; set; }
        public Schedule Schedule { get; set; }
    }

}
