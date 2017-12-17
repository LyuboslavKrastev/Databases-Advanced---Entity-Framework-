namespace Employees.App
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Employees.Data;
    using Microsoft.EntityFrameworkCore;
    using Employees.Services;
    using AutoMapper;

    class StartUp
    {
        static void Main(string[] args)
        {

                var serviceProvider = ConfigureServices();
                var engine = new Engine(serviceProvider);
                engine.Run();
                
        }

        static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddDbContext<EmployeesContext>(op => op.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());



            var serviceProvider = serviceCollection.BuildServiceProvider();

           

            return serviceProvider;
        }
    }
}
