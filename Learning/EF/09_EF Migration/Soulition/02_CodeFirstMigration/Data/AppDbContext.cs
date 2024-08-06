
using _02_CodeFirstMigration.Models;
using _02_CodeFirstMigration.Models.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace _02_CodeFirstMigration.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<SectionSchedule> SectionSchedules { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<StudentCourseDetailView> StudentCourseDetailViews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // apply the configurations

            // -- method #1 --

            // modelBuilder.ApplyConfiguration(new CourseConfiguration()); // not best practice because you have to add each configuration manually if you have 100 tables you have to add 100 lines

            // -- method #2 --
            
            // the best practice is to use the following line
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // this will apply all configurations in the same assembly as the AppDbContext

            // or
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
