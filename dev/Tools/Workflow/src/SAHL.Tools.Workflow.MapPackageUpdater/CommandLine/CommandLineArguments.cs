using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Workflow.MapPackageUpdater.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "map", Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMap { get; set; }

        [Option('p', "package", Required = true, HelpText = "package name.")]
        public string PackageName { get; set; }

        [Option('v', "version", Required = true, HelpText = "Version.")]
        public string Version { get; set; }
    }
}
