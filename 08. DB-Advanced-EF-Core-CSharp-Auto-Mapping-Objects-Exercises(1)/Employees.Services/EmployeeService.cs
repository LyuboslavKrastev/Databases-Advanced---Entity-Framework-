namespace Employees.Services
{
    using System;
    using AutoMapper;
    using Employees.Data;
    using Employees.DtoModels;
    using Employees.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;

    public class EmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public void AddEmployee(EmployeeDto employeeDTO)
        {
            var employee = Mapper.Map<Employee>(employeeDTO);

            context.Employees.Add(employee);

            context.SaveChanges();
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            employee.Address = address;

            context.SaveChanges();

            var name = $"{employee.FirstName} {employee.LastName}";
            return name;
        }

        public string SetBirthday(int employeeId, DateTime date)
        {
            var employee = context.Employees.Find(employeeId);

            employee.Birthday = date;

            context.SaveChanges();

            var name = $"{employee.FirstName} {employee.LastName}";
            return name;
        }

        public EmployeePersonalDto FullById (int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            var employeeDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeeDto;
        }


        public EmployeeManagerDto SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);

            var manager = context.Employees.Find(managerId);

            employee.Manager = manager;

            context.SaveChanges();

            var employeeManagerDto = Mapper.Map<EmployeeManagerDto>(employee);

            return employeeManagerDto;
        }

        public ManagerDto GetManager(int managerId)
        {
            var employee = context.Employees
                .Include(m => m.ManagedEmployees)
                .SingleOrDefault(m => m.Id == managerId);

            var managerDto = Mapper.Map<ManagerDto>(employee);

            return managerDto;
        }

        public List<EmployeeManagerDto> OlderThanAge(int age)
        {
            var employees = context.Employees
                .Where(e => e.Birthday != null
                            && 
                            DateTime.Now.Year - e.Birthday.Value.Year > age)
                .Include(e => e.Manager)
                .ProjectTo<EmployeeManagerDto>()
                .ToList();

            return employees;
        }
    }
}
