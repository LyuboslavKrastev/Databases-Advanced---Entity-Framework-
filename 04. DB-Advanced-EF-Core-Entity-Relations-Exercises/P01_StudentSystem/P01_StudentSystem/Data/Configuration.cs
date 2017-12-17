using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class Configuration
    {
        public static string ConnectionString { get; set; } = @"Server=ZORO\SQLEXPRESS;Database=StudentSystem;Integrated Security=True";
    }
}
