﻿using _04_OverrideConfiguratiobByGroupingConfiguration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _04_OverrideConfiguratiobByGroupingConfiguration.Data.Config
{
    public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.ToTable("tblTweets"); // this maps to the table name in the database

            builder.HasKey(t => t.TweetId);

            builder.Property(t => t.TweetText)
                .IsRequired()
                .HasMaxLength(280);
        }
    }
}
