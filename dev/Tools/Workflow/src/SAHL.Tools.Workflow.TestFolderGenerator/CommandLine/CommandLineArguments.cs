using CommandLine;

namespace SAHL.Tools.Workflow.Builder.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "map", Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMap { get; set; }

        [Option('o', "outDir", Required = false, HelpText = "Target output directory for compiled workflow.")]
        public string OutputDirectory { get; set; }

        [Option('r', "rootDir", Required = false, HelpText = "Root directory for the codebase.")]
        public string RootDirectory { get; set; }
    }
}