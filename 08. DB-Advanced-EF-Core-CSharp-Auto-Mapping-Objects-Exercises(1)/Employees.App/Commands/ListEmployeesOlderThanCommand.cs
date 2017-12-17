namespace Employees.App.Commands
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Employees.Services;
    using Employees.DtoModels;
    class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = employeeService.OlderThanAge(age).OrderByDescending(e=>e.Salary);

            if (employees.Count()==0)
            {
                throw new ArgumentException($"No employees older than {age} found");
            }

            var result = new StringBuilder();
            foreach (var emp in employees)
            {
                result.Append($"{emp.FirstName} {emp.LastName} - ${emp.Salary:F2} - Manager: ");

                if (emp.Manager == null)
                {
                    result.Append("No Manager");
                }
                else
                {
                    result.Append(emp.Manager.LastName);
                }
                result.AppendLine();
            }
            return result.ToString().TrimEnd();
        }
    }
}
