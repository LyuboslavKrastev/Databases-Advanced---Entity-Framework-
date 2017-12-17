

namespace Employees.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Employees.Services;
    using Employees.DtoModels;

    class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;
        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            var employee = employeeService.FullById(employeeId);
            string birthDay = "N/A";
            if (employee.Birthday!=null)
            {
                birthDay = employee.Birthday.Value.ToString();
            }
            string address = employee.Address ?? "N/A";
            var result = $"{employeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}" + Environment.NewLine +
                $"Birthday: {birthDay}" + Environment.NewLine +
                $"Address: {address}";

            return result;
        }
    }
}
