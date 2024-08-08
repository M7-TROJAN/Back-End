using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace _01_CodeFirstMigration.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }


        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5, // Number of retry attempts
                    maxRetryDelay: TimeSpan.FromSeconds(5), // Delay between retries
                    errorNumbersToAdd: null); // Optional: specify error numbers to retry on
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new CourseConfiguration()); 
            // not best practice because you have to add each configuration manually if you have 100 tables you have to add 100 lines

            // the best practice is to use the following line
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            // this will apply all configurations in the same assembly as the AppDbContext

        }
    }
}
