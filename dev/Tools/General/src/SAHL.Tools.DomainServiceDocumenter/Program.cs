using CommandLine;
using SAHL.Tools.DomainServiceDocumenter.Commands;
using System;

namespace SAHL.Tools.DomainServiceDocumenter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            if (parserResult == true)
            {
                SAHL.Tools.DomainServiceDocumenter.Lib.Documenter generator = new SAHL.Tools.DomainServiceDocumenter.Lib.Documenter();

                try
                {
                    generator.GenerateObjects(commands.ServiceName, commands.InputPath, commands.OutputPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("");
                    Console.WriteLine("stack: " + ex.StackTrace);
                    Environment.Exit(-1);
                }
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
            }
            Environment.Exit(0);
        }
    }
}