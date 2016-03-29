using CommandLine;
using SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Commands;
using SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            if (parserResult == true)
            {
                if (commands.BuildMode == null)
                {
                    commands.BuildMode = "Debug";
                }
                DecisionTreeGenerator generator = new DecisionTreeGenerator();

                string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User Id={2};Password={3};Connect Timeout=300;",
                                                        commands.Server, commands.Database, commands.UserName, commands.Password);

                try
                {
                    //generator.GenerateTreeObject(connectionString, commands.TreeOutputPath, commands.QueryOutputPath, commands.DecisionTreeName, commands.DecisionTreeVersion, commands.BuildMode);
                    if (string.IsNullOrWhiteSpace(commands.TreeOutputPath) && string.IsNullOrWhiteSpace(commands.QueryOutputPath))
                        throw new ArgumentNullException("OutputPath", "Both TreeOutputPath and QueryOutputPath cannot be null");

                    generator.GenerateTreeObjects(connectionString, commands.TreeOutputPath, commands.QueryOutputPath, commands.BuildMode, commands.Config);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("Error: " + ex.StackTrace);
                    Environment.Exit(-1);
                }
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
                Environment.Exit(-1);
            }

            Environment.Exit(0);
        }
    }
}