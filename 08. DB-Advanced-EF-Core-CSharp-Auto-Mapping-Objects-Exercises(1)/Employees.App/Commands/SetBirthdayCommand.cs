
namespace Employees.App.Commands
{
    using System;
    using Employees.Services;
    using System.Globalization;

    class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;
        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var date = DateTime.ParseExact(args[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var empName = employeeService.SetBirthday(employeeId, date);

            return $"{empName} birthday set to {args[1]}";
        }
    }
}
