using System;
using CommandLine;

namespace SAHL.Tools.Reportenator.CommandLine
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