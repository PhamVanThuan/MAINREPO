using CommandLine;

namespace SAHL.Tools.ObjectFromJsonGenerator.Commands
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

        [Option('c', "classnamespace", Required = true, HelpText = "The namespace to use for generated classes.")]
        public string Namespace { get; set; }

        [Option('o', "outputPath", Required = true, HelpText = "The output file with full path for the generated code.")]
        public string OutputPath { get; set; }

        [Option('m', "messageversion", Required = true, HelpText = "The MessageSet version we are using.")]
        public string MsgSetVersion { get; set; }

        [Option('v', "variableversion", Required = true, HelpText = "The MessageSet version we are using.")]
        public string VarSetVersion { get; set; }

        [Option('e', "messageversion", Required = true, HelpText = "The MessageSet version we are using.")]
        public string EnumSetVersion { get; set; }
        //link in ID when these things exist

        [Option('b', "buildMode", Required = false, HelpText = "Defaults to debug if not set otherwise set to Release to only use published globals.")]
        public string BuildMode { get; set; }
    }
}