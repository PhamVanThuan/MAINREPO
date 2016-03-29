using System;
using System.IO.Abstractions;
using CommandLine;
using SAHL_Tools.WebApiGenerator.Commands;

namespace SAHL_Tools.WebApiGenerator
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
                SAHL_Tools.Common.WebApiGenerator generator = new Common.WebApiGenerator(new FileSystem());
                generator.GenerateObjects(commands.InputFile, commands.OutputFile);

                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
                Environment.Exit(-1);
            }
        }
    }
}