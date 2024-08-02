using _04_OverrideConfiguratiobByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfiguratiobByGroupingConfiguration.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tblUsers"); // this maps to the table name in the database
            
            builder.HasKey(u => u.UserId); 
            
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
