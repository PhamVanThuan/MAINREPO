using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using SAHL.Tools.Scriptenator.lib;

namespace SAHL.Tools.Scriptenator
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
                    var scriptenator = new Script();
                    scriptenator.ExecuteScriptenatorFile(commandLineArguments.ConnectionString, commandLineArguments.ScriptDirectory, commandLineArguments.ScriptFileName, commandLineArguments.DatabaseRestore);
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("...FAILURE: Error is: {0}", e.Message));
                    Console.WriteLine("An error occured, script processing aborted!");
                    Environment.ExitCode = -1;
                }
            }
        }
    }
}