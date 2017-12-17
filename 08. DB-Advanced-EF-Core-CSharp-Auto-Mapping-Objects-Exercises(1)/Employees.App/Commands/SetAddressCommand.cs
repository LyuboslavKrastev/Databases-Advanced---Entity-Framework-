
namespace Employees.App.Commands
{
    using System;
    using Employees.Services;
    using System.Globalization;
    using System.Linq;

    class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;
        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var addrres = string.Join(" ", args.Skip(1));

            var empName = employeeService.SetAddress(employeeId, addrres);

            return $"{empName} address set to {args[1]}";
        }
    }
}
