
    namespace Employees.DtoModels
    {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ManagerDto
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public ICollection<EmployeeDto> ManagedEmployees { get; set; }

            public int ManagedEmployeesCount { get; set; }
        }
    }

