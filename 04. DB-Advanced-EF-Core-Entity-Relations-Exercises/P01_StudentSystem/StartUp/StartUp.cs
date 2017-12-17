using System;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem
{ 
    class StartUp
    {
        static void Main(string[] args)
        {
            var context = new StudentSystemContext();
            ResetDatabase(context);

        }

        private static void ResetDatabase(StudentSystemContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            Seed(context);
        }

        private static void Seed(StudentSystemContext context)
        {
            var students = new[]
            {
                new Student
                {
                    Name = "Resho",
                    PhoneNumber = "1234567891",
                    RegisteredOn = DateTime.Parse("01-02-1993")
                },
                 new Student
                {
                    Name = "Gesho",
                    PhoneNumber = "1214567891",
                    RegisteredOn = DateTime.Parse("01-02-1993")
                },
                  new Student
                {
                    Name = "sesho",
                    PhoneNumber = "1234267891",
                    RegisteredOn = DateTime.Parse("01-02-1993")
                }
            };

            var courses = new[]
           {
                new Course
                {
                    Name = "niceCourse",
                    Description = "describe",
                    StartDate = DateTime.Parse("01-02-1992"),
                    EndDate = DateTime.Parse("05-03-1994"),
                    Price = 199
                },

                 new Course
                {
                    Name = "coolCourse",
                    Description = "describe",
                    StartDate = DateTime.Parse("01-02-1992"),
                    EndDate = DateTime.Parse("05-03-1994"),
                    Price = 199
                },

                  new Course
                {
                    Name = "badCourse",
                    Description = "describe",
                    StartDate = DateTime.Parse("01-02-1992"),
                    EndDate = DateTime.Parse("05-03-1994"),
                    Price = 199
                },
            };
            var resources = new[]
          {
                new Resource
                {
                    Name = "theResource",
                    Url = "helloThere",
                    ResourceType = ResourceType.Video,                   
                    CourseId = 1
                },
                  new Resource
                {
                    Name = "theSecondResource",
                    Url = "helloThere",
                    ResourceType = ResourceType.Video,
                    CourseId = 1
                },
                    new Resource
                {
                    Name = "theThirdResource",
                    Url = "helloThere",
                    ResourceType = ResourceType.Video,
                    CourseId = 1
                },

            };
            context.Resources.AddRange(resources);
            context.Students.AddRange(students);
            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
    }
}
