using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configurations
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(b => b.HomeworkId);

            builder.Property(b => b.Content)
                .IsUnicode(false);

        }
    }
}
