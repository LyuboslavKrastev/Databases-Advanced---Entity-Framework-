using System;
using System.Collections.Generic;
using System.Text;

namespace Employees.App.Commands
{
    using System;
    using Employees.Services;
    using System.Globalization;
    using System.Linq;

    class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;
        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            var employee = employeeService.ById(employeeId);

            return $"ID: {employeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}";
        }
    }
}
