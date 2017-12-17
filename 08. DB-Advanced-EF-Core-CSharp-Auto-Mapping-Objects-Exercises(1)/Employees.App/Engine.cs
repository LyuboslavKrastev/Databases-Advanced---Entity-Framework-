

namespace Employees.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class Engine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        internal void Run()
        {
            while (true)
            {

                try
                {


                    var input = Console.ReadLine();

                    var Tokens = input.Split();

                    var commandName = Tokens[0];

                    var commandArgs = Tokens.Skip(1).ToArray();

                    var command = CommandParser.Parse(serviceProvider, commandName);

                    var result = command.Execute(commandArgs);

                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
