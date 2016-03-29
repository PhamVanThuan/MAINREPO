using CommandLine;

namespace SAHL.Tools.EnumGenerator.Commands
{
    public class CommandLineArguments
    {
        [Option('s', "server", Required = true, HelpText = "The database server to connect to.")]
        public string Server { get; set; }

        [Option('d', "database", Required = true, HelpText = "The default database on the server.")]
        public string Database { get; set; }

        [Option('u', "username", Required = true, HelpText = "The username to use when authenticating with the database.")]
        public string UserName { get; set; }

        [Option('p', "password", Required = true, HelpText = "The password to use when authenticating with the database.")]
        public string Password { get; set; }

        [Option('o', "outputdirectory", Required = true, HelpText = "The output directory for the generated business models.")]
        public string OutputDirectory { get; set; }
    }
}