namespace _01_CodeFirstMigration.Entities.Views
{
    public class CourseDetailsView
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string SectionName { get; set; }
        public string InstructorName { get; set; }
        public string ScheduleTitle { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool SUN { get; set; }
        public bool MON { get; set; }
        public bool TUE { get; set; }
        public bool WED { get; set; }
        public bool THU { get; set; }
        public bool FRI { get; set; }
        public bool SAT { get; set; }
    }
}
