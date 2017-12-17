using System;
using System.Collections.Generic;
using System.Text;

namespace Employees.App.Commands
{
    class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.WriteLine("See you soon!");
            Environment.Exit(0);
            return "";
        }

    }
}
