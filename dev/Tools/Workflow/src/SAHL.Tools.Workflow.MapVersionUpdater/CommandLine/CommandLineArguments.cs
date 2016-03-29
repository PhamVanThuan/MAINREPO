using CommandLine;

namespace SAHL.Tools.Workflow.WorkflowMapVersionUpdater
{
    public class CommandLineArguments
    {
        [Option('m', "map", Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMap { get; set; }

        [Option('v', "version", Required = true, HelpText = "Version.")]
        public string Version { get; set; }
    }
}