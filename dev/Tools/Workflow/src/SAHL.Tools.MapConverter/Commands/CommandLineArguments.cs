using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace SAHL.Tools.MapConverter.Commands
{
    class CommandLineArguments
    {
        [Option('m', "map", Required = false, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMap { get; set; }

        [Option('b', "bakDir", Required = false, HelpText = "Target backup directory for compiled workflow.")]
        public string OutputDirectory { get; set; }

        [Option('r', "rootDir", Required = true, HelpText = "Root directory for the codebase.")]
        public string RootDirectory { get; set; }
    }
}
