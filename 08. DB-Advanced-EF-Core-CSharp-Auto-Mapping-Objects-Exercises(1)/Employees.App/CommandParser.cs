

namespace Employees.App
{
    using System;
    using System.Reflection;
    using Employees.App.Commands;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    internal class CommandParser
    {
        public static ICommand Parse(IServiceProvider serviceProvider, string commandName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand)));

            var defineCommand = commandTypes.SingleOrDefault(c => c.Name == $"{commandName}Command");

            if (defineCommand==null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var constructor = defineCommand.GetConstructors().FirstOrDefault();

            var constParams = constructor.GetParameters().Select(p=>p.ParameterType);

            var args = constParams.Select(p => serviceProvider.GetService(p)).ToArray();

            var command = constructor.Invoke(args);

            return (ICommand)command;
        }
    }
}
