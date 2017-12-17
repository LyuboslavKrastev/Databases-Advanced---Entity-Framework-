using System;
using System.Collections.Generic;
using System.Text;

namespace Employees.Data
{
    public class Configuration
    {
        public static string ConnectionString => @"Server=ZORO\SQLEXPRESS;Database=EmployeesDB;Integrated Security=True";
    }
}
