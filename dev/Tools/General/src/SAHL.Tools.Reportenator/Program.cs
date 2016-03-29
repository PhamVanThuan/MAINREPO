using System;
using CommandLine;
using SAHL.Tools.Reportenator.lib;

namespace SAHL.Tools.Reportenator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commandLineParser = new Parser();
            var commandLineArguments = new CommandLineArguments();
            if (commandLineParser.ParseArguments(args, commandLineArguments))
            {
                try
                {
                    var reportenator = new ReportenatorRunner(commandLineArguments.ServerName, commandLineArguments.UserName, commandLineArguments.Password, commandLineArguments.Domain);
                    reportenator.ExecuteReportenatorFile(commandLineArguments.ReportsDirectory, commandLineArguments.ScriptFileName);
                }
                catch
                {
                    Console.WriteLine("An error occurred, script processing aborted!");
                    Environment.ExitCode = -1;
                }
            }
        }
    }
}