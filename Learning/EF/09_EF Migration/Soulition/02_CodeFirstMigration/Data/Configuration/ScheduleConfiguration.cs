using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System;


namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.Id); // this line is added to set the Id as the primary key
            builder.Property(s => s.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.Property(s => s.Title)
                .HasColumnType("VARCHAR") // this line is added to set the data type of the Title to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the Title to 50
                .IsRequired(); // this line is added to make the Title required

            builder.Property(s => s.SUN)
                .IsRequired(); // this line is added to make the SUN required

            builder.Property(s => s.MON)
                .IsRequired(); // this line is added to make the MON required

            builder.Property(s => s.TUE)
                .IsRequired(); // this line is added to make the TUE required

            builder.Property(s => s.WED)
                .IsRequired(); // this line is added to make the WED required

            builder.Property(s => s.THU)
                .IsRequired(); // this line is added to make the THU required

            builder.Property(s => s.FRI)
                .IsRequired(); // this line is added to make the FRI required

            builder.Property(s => s.SAT)
                .IsRequired(); // this line is added to make the SAT required


            builder.HasMany(s => s.SectionSchedules) // This line specifies that the Schedule entity has a one-to-many relationship with the SectionSchedule entity
                .WithOne(ss => ss.Schedule) // This line specifies that the SectionSchedule entity has a one-to-one relationship with the Schedule entity
                .HasForeignKey(ss => ss.ScheduleId); // This line specifies the foreign key property in the SectionSchedule entity


            builder.ToTable("Schedules"); // this line is added to set the table name to Schedules

            // this line is added to insert data into the Schedules table
            builder.HasData(LoadSchedules());
        }

        private Schedule[] LoadSchedules()
        {
            return new Schedule[]
            {
                new Schedule { Id = 1, Title = "Daily", SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = false, SAT = false },
                new Schedule { Id = 2, Title = "DayAfterDay", SUN = true, MON = false, TUE = true, WED = false, THU = true, FRI = false, SAT = false },
                new Schedule { Id = 3, Title = "Twice-a-Week", SUN = false, MON = true, TUE = false, WED = true, THU = false, FRI = false, SAT = false },
                new Schedule { Id = 4, Title = "Weekend", SUN = false, MON = false, TUE = false, WED = false, THU = false, FRI = true, SAT = true },
                new Schedule { Id = 5, Title = "Compact", SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = true, SAT = true }
            };
        }
    }
}
