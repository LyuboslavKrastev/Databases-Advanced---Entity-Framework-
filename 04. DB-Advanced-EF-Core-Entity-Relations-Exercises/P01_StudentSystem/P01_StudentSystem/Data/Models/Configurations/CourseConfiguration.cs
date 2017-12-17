using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configurations
{
    class CourseConfiguration : IEntityTypeConfiguration<Course>
    {       
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.CourseId);

            builder.Property(p => p.Name)
                .IsUnicode()
                .HasMaxLength(80);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .IsUnicode();

            builder.Property(p => p.StartDate)
                .HasColumnType("DATETIME2");

            builder.Property(p => p.EndDate)
                .HasColumnType("DATETIME2");

            builder.HasMany(se => se.StudentsEnrolled)
                .WithOne(c => c.Course).HasForeignKey(fk => fk.StudentId);

            builder.HasMany(r => r.Resources)
                .WithOne(c => c.Course)
                .HasForeignKey(fk => fk.ResourceId);

            builder.HasMany(hw => hw.HomeworkSubmissions)
                .WithOne(c => c.Course)
                .HasForeignKey(fk => fk.HomeworkId);
        }
    }
}

