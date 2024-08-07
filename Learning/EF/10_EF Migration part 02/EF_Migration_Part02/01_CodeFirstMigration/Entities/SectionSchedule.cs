namespace _01_CodeFirstMigration.Entities
{
    public class SectionSchedule
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public int SectionId { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}