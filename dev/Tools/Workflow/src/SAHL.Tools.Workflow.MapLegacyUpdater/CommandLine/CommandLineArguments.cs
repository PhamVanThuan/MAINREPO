using CommandLine;

namespace SAHL.Tools.Workflow.MapLegacyUpdater.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "map", Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMap { get; set; }

        [Option('l', "legacy", Required = true, HelpText = "Is the map Legacy (true/false).")]
        public string Legacy { get; set; }
    }
}