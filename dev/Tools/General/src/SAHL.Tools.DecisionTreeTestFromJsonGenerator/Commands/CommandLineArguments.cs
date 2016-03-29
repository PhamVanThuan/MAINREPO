using CommandLine;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Commands
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

        [Option('o', "outputPath", Required = true, HelpText = "The directory path to use when generating the files.")]
        public string OutputPath { get; set; }

        [Option('n', "namespace", Required = true, HelpText = "The namespace to use when generating the test case files.")]
        public string NameSpace { get; set; }

        [Option('b', "buildMode", Required =false, HelpText ="Debug or Release. Release only generates test case files for published trees. The default is Debug")]
        public string BuildMode { get;set; }

        [Option('i', "include", Required = false, HelpText = "file where includes can be found, whitelist")]
        public string Include { get; set; }

    }
}
