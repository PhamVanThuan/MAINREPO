using CommandLine;

namespace SAHL.Tools.Workflow.Publisher.CommandLine
{
    public class CommandLineArguments
    {
        [Option('m', "maps", Required = true, HelpText = "X2 Workflow Map(s) to load (*.x2p), separated by semi-colons if multiple.")]
        public string X2WorkflowMaps { get; set; }

        [Option('s', "dbName", Required = true, HelpText = "Database to publish to.")]
        public string DatabaseName { get; set; }

        [Option('u', "dbUserName", Required = true, HelpText = "Username.")]
        public string UserName { get; set; }

        [Option('p', "dbPassword", Required = true, HelpText = "Password.")]
        public string Password { get; set; }

        [Option('b', "binaryFolder", Required = false, HelpText = "Path to binaries.")]
        public string BinaryFolder { get; set; }
    }
}