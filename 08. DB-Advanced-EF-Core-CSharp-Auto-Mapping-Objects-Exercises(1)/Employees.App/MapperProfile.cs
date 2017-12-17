
namespace Employees.App
{
    using AutoMapper;
    using Employees.Models;
    using Employees.DtoModels;
    class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Employee, EmployeePersonalDto>();
            CreateMap<Employee, EmployeePersonalDto>();

            CreateMap<Employee, ManagerDto>()
                .ForMember(
                ManagerDto => ManagerDto.ManagedEmployeesCount,
                opt => opt.MapFrom(employee => employee.ManagedEmployees.Count));

            CreateMap<Employee, EmployeeManagerDto>();
        }
    }
}
