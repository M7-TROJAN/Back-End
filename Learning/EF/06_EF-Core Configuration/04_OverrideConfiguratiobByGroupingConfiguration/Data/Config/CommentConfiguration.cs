﻿using _04_OverrideConfiguratiobByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfiguratiobByGroupingConfiguration.Data.Config
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("tblComments"); // this maps to the table name in the database

            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.CommentText)
                .IsRequired()
                .HasMaxLength(280);
        }
    }
}