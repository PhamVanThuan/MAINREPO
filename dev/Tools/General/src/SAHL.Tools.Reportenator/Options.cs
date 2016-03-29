using CommandLine;

namespace SAHL.Tools.Reportenator
{
    public class CommandLineArguments
    {
        [Option('s', "ServerName", Required = true, HelpText = "Report Server Name e.g. (SAHLS15).")]
        public string ServerName { get; set; }

        [Option('d', "reportstDirectory", Required = true, HelpText = "Reportenator Directory e.g. (c:/Reports/).")]
        public string ReportsDirectory { get; set; }

        [Option('f', "fileName", Required = true, HelpText = "Reportenator File to Execute e.g. (2.36.0.1.a.xml).")]
        public string ScriptFileName { get; set; }

        [Option('u', "username", Required = true, HelpText = "ReportService UserName")]
        public string UserName { get; set; }

        [Option('p', "password", Required = true, HelpText = "ReportService Password")]
        public string Password { get; set; }

        [Option('o', "domain", Required = true, HelpText = "ReportService Domain")]
        public string Domain { get; set; }
    }
}