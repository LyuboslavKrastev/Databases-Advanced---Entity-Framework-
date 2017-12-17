

namespace Employees.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Employees.Services;
    class SetManagerCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var managerId = int.Parse(args[1]);

            var employeeManagerDTO = employeeService.SetManager(employeeId, managerId);

            return $"{employeeManagerDTO.Manager.FirstName} {employeeManagerDTO.Manager.LastName} " +
                $"is successfuly set as a manager " +
                $"to {employeeManagerDTO.FirstName} {employeeManagerDTO.LastName}";
        }
    }
}
