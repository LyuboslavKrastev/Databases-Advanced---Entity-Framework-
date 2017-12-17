namespace Employees.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Employees.Services;
    class ManagerInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);

            var manager = employeeService.GetManager(employeeId);

            var result = new StringBuilder();
            result.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.ManagedEmployeesCount}");

            foreach (var emp in manager.ManagedEmployees)
            {
                result.AppendLine($"    - {emp.FirstName} {emp.LastName} - ${emp.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
