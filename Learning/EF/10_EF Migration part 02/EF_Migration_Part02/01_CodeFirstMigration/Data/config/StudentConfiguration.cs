using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedNever();

            builder.Property(s => s.FName)
                .HasColumnName("FName")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.LName)
                .HasColumnName("LName")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Gender)
                .HasColumnName("Gender")
                .HasColumnType("CHAR")
                .HasMaxLength(1)
                .IsRequired();


            builder.ToTable(t => t.HasCheckConstraint("chk_Gender", "Gender IN ('f', 'm')"));
            
            builder.ToTable("Students");

            builder.HasData(LoadStudents());
        }

        private Student[] LoadStudents()
        {
            return new Student[]
            {
                new Student { Id = 1, FName = "Fatima", LName = "Ali", Gender = 'f' },
                new Student { Id = 2, FName = "Noor", LName = "Saleh", Gender = 'f' },
                new Student { Id = 3, FName = "Omar", LName = "Youssef", Gender = 'm' },
                new Student { Id = 4, FName = "Huda", LName = "Ahmed", Gender = 'm' },
                new Student { Id = 5, FName = "Amira", LName = "Tariq", Gender = 'f' },
                new Student { Id = 6, FName = "Zainab", LName = "Ismail", Gender = 'f' },
                new Student { Id = 7, FName = "Yousef", LName = "Farid", Gender = 'm' },
                new Student { Id = 8, FName = "Layla", LName = "Mustafa", Gender = 'f' },
                new Student { Id = 9, FName = "Mohammed", LName = "Adel", Gender = 'm' },
                new Student { Id = 10, FName = "Samira", LName = "Nabil", Gender = 'f' }
            };
        }
    }
}
