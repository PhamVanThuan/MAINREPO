using CommandLine;

namespace SAHL.Tools.Workflow.Builder.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "map", Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
        public string X2WorkflowMaps { get; set; }

        [Option('b', "buildServerMode", Required = true, HelpText = "If 'true' will not do any pushing of packages and will pull from sahl deploy nuget server.")]
        public bool BuildServerMode { get; set; }

        [Option('n', "nugetPullApiUrl", Required = false, HelpText = "Specify the nuget gallery to pull packages from.")]
        public string NugetPullApiUrl { get;set; }

        [Option('c', "checkrefs", Required = false, HelpText = "Choose whether to check and update references before compilation.")]
        public bool CheckReferences { get; set; }
    }
}