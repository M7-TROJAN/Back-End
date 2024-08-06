using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class SectionScheduleConfiguration : IEntityTypeConfiguration<SectionSchedule>
    {
        public void Configure(EntityTypeBuilder<SectionSchedule> builder)
        {
            builder.HasKey(ss => ss.Id); // this line is added to set the Id as the primary key
            builder.Property(ss => ss.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.Property(ss => ss.StartTime)
                .HasColumnType("TIME") // this line is added to set the data type of the StartTime to TIME
                .IsRequired(); // this line is added to make the StartTime required

            builder.Property(ss => ss.EndTime)
                .HasColumnType("TIME") // this line is added to set the data type of the EndTime to TIME
                .IsRequired(); // this line is added to make the EndTime required

            // Define the relationship with Section
            builder.HasOne(ss => ss.Section) // This line specifies that the SectionSchedule entity has a one-to-one relationship with the Section entity
                .WithMany(s => s.SectionSchedules) // This line specifies that the Section entity has a one-to-many relationship with the SectionSchedule entity
                .HasForeignKey(ss => ss.SectionId); // This line specifies the foreign key property in the SectionSchedule entity

            // Define the relationship with Schedule
            builder.HasOne(ss => ss.Schedule) // This line specifies that the SectionSchedule entity has a one-to-one relationship with the Schedule entity
                .WithMany(s => s.SectionSchedules) // This line specifies that the Schedule entity has a one-to-many relationship with the SectionSchedule entity
                .HasForeignKey(ss => ss.ScheduleId); // This line specifies the foreign key property in the SectionSchedule entity

            builder.ToTable("SectionSchedules"); // this line is added to set the table name to SectionSchedules

            builder.HasIndex(ss => ss.ScheduleId); // This line is added to create an index on the ScheduleId column

            // this line is added to insert data into the SectionSchedules table
            builder.HasData(LoadSectionSchedules());

        }

        private SectionSchedule[] LoadSectionSchedules()
        {
            return new SectionSchedule[]
            {
                new SectionSchedule { Id = 1, SectionId = 1, ScheduleId = 1, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new SectionSchedule { Id = 2, SectionId = 2, ScheduleId = 3, StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
                new SectionSchedule { Id = 3, SectionId = 3, ScheduleId = 4, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(15, 0, 0) },
                new SectionSchedule { Id = 4, SectionId = 4, ScheduleId = 1, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                new SectionSchedule { Id = 5, SectionId = 5, ScheduleId = 1, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
                new SectionSchedule { Id = 6, SectionId = 6, ScheduleId = 2, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new SectionSchedule { Id = 7, SectionId = 7, ScheduleId = 3, StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
                new SectionSchedule { Id = 8, SectionId = 8, ScheduleId = 4, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
                new SectionSchedule { Id = 9, SectionId = 9, ScheduleId = 4, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
                new SectionSchedule { Id = 10, SectionId = 10, ScheduleId = 3, StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(15, 0, 0) },
                new SectionSchedule { Id = 11, SectionId = 11, ScheduleId = 5, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(11, 0, 0) }
            };
        }
    }
}
