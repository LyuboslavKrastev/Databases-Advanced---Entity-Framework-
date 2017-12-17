using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configurations
{
    public class StudentConfiguraton : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(b => b.StudentId);

            builder.Property(n => n.Name)
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(b => b.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasColumnType("CHAR(10)");

            builder.Property(b => b.Birthday)
                .IsRequired(false);

            builder.HasMany(hw => hw.HomeworkSubmissions)
                .WithOne(s => s.Student).HasForeignKey(fk => fk.HomeworkId);

            builder.HasMany(cr =>cr.CourseEnrollments)
          .WithOne(s => s.Student).HasForeignKey(fk => fk.CourseId);

        }
    }
}
