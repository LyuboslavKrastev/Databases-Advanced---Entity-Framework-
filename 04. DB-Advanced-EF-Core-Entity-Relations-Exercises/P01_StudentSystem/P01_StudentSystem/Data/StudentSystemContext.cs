﻿using System;

namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using P01_StudentSystem.Data.Models;
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Models.Configurations;
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext() { }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Course> Courses { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguraton());
            modelBuilder.ApplyConfiguration(new HomeworkConfiguration());
            modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
        }
    }
}


