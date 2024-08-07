using _01_CodeFirstMigration.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class StudentCourseDetailViewConfiguration : IEntityTypeConfiguration<StudentCourseDetailsView>
    {
        public void Configure(EntityTypeBuilder<StudentCourseDetailsView> builder)
        {
            builder.HasNoKey();
            builder.ToView("StudentCourseDetailsView");
        }
    }
    internal class CourseDetailsViewConfiguration : IEntityTypeConfiguration<CourseDetailsView>
    {
        public void Configure(EntityTypeBuilder<CourseDetailsView> builder)
        {
            builder.HasNoKey();
            builder.ToView("CourseDetailsView");
        }
    }
}
