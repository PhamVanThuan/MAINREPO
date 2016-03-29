using System;
using System.IO.Abstractions;
using CommandLine;
using SAHL.Tools.ObjectModelGenerator.Commands;
using System.Diagnostics;

namespace SAHL.Tools.ObjectModelGenerator
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
                SAHL.Tools.ObjectModelGenerator.Lib.ObjectModelGenerator generator = new SAHL.Tools.ObjectModelGenerator.Lib.ObjectModelGenerator(new FileSystem());
                string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User Id={2};Password={3};Connect Timeout=300;",
                                                        commands.Server, commands.Database, commands.UserName, commands.Password);
                generator.GenerateObjects(connectionString, commands.OutputDirectory, commands.Include, commands.Schema, commands.Database, commands.Namespace);

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