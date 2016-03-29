using CommandLine;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Commands
{
    public class CommandLineArguments
    {
        [Option('s', "server", Required = true, HelpText = "The database server to connect to.")]
        public string Server { get; set; }

        [Option('d', "database", Required = true, HelpText = "The default database on the server.")]
        public string Database { get; set; }

        [Option('u', "username", Required = false, HelpText = "The username to use when authenticating with the database.")]
        public string UserName { get; set; }

        [Option('p', "password", Required = false, HelpText = "The password to use when authenticating with the database.")]
        public string Password { get; set; }

        [Option('o', "treeOutputPath", DefaultValue = null, Required = false, HelpText = "The output file with full path for the generated tree code.")]
        public string TreeOutputPath { get; set; }

        [Option('q', "queryOutputPath", DefaultValue = null, Required = false, HelpText = "The output file with full path for the generated tree query code.")]
        public string QueryOutputPath { get; set; }

        [Option('t', "decisionTreeName", Required = false, HelpText = "The decision tree name to use.")]
        public string DecisionTreeName { get; set; }

        [Option('v', "version", Required = false, DefaultValue = "*", HelpText = "The version of the decision tree, * indicates use the latest.")]
        public string DecisionTreeVersion { get; set; }

        [Option('c', "config", Required = false, HelpText = "config file location.")]
        public string Config { get; set; }

        [Option('b', "buildMode", Required = false, HelpText = "Defaults to debug if not set otherwise set to Release to only use published globals.")]
        public string BuildMode { get; set; }

    }
}