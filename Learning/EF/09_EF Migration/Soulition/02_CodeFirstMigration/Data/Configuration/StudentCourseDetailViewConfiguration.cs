using _02_CodeFirstMigration.Models.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class StudentCourseDetailViewConfiguration : IEntityTypeConfiguration<StudentCourseDetailView>
    {
        public void Configure(EntityTypeBuilder<StudentCourseDetailView> builder)
        {
            builder.HasNoKey();
            builder.ToView("StudentCourseDetails");
        }
    }
}
