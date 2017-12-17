using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.ResourceId);

            builder.Property(r => r.Name)
                .IsUnicode().HasMaxLength(50);

            builder.Property(r => r.Url)
                .IsUnicode(false);

            builder.HasOne(r => r.Course)
                .WithMany(res => res.Resources)
                .HasForeignKey(fk => fk.CourseId);
                
        }
    }
}
