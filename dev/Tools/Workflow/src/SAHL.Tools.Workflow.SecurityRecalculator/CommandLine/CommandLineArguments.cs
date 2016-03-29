using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SAHL.Tools.Workflow.SecurityRecalculator.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "maps", Required = false, HelpText = "X2 Workflow Map(s) to recalculate, separated by semi-colons if multiple.")]
        public string X2WorkflowMaps { get; set; }

        [Option('d', "useDevNuGetServer", Required = false, HelpText = "If 'true' will pull packages from the dev nuget server.")]
        public bool UseDevNuGetServer { get; set; }

        [Option('s', "dbName", Required = true, HelpText = "Database to publish to.")]
        public string DatabaseName { get; set; }

        [Option('u', "dbUserName", Required = true, HelpText = "Username.")]
        public string UserName { get; set; }

        [Option('p', "dbPassword", Required = true, HelpText = "Password.")]
        public string Password { get; set; }

        [Option('n', "new process id", Required = false, HelpText = "ProcessID of map to recalculate.")]
        public int ProcessID { get; set; }

        [Option('o', "old process id", Required = false, HelpText = "Old ProcessID of map to recalculate.")]
        public int OldProcessID { get; set; }
    }
}
