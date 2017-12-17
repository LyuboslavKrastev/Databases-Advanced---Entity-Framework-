using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class Configuration
    {
        public static string ConnectionString { get; set; } = "Server=.;Database=Hospital;Integrated Security=True";
    }
}
