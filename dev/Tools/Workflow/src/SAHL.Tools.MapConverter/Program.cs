using CommandLine;
using SAHL.Tools.MapConverter.Commands;

using NHibernate;
using NHibernate.Linq;
using System.Linq;
using SAHL.Tools.Workflow.Common.Database;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;
using System.Collections.Generic;
using System;
using System.Xml.Linq;


namespace SAHL.Tools.MapConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            Controller c = new Controller(commands);
        }
    }
}
