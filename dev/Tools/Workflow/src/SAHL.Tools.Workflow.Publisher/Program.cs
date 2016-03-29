using CommandLine;
using SAHL.Tools.Workflow.Common.Database.Publishing;
using SAHL.Tools.Workflow.Publisher.CommandLine;
using System;
using System.IO;
using publishing = SAHL.Tools.Workflow.Common.Database.Publishing;

namespace SAHL.Tools.Workflow.Publisher
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments arguments = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, arguments);

            if (parserResult == true)
            {
                PrePublishChecker checker = new PrePublishChecker();
                ProcessFromXmlGenerator procGen = new ProcessFromXmlGenerator();
                publishing.Publisher publisher = new publishing.Publisher(checker, procGen);

                string[] maps = arguments.X2WorkflowMaps.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                string connectionString = string.Format("Data Source={0};Initial Catalog=X2;User ID={1};Password={2};Timeout=300", arguments.DatabaseName, arguments.UserName, arguments.Password);

                foreach (string map in maps)
                {
                    string workflowMapDirectory = Path.GetDirectoryName(map);

                    if (!Path.IsPathRooted(workflowMapDirectory))
                    {
                        string currentDir = Directory.GetCurrentDirectory();
                        workflowMapDirectory = Path.Combine(currentDir, workflowMapDirectory);
                    }

                    if (File.Exists(map))
                    {
                        string binaryFolder = arguments.BinaryFolder;
                        //  if binary folder is not specified we need to setup the binary directory by convention
                        if (string.IsNullOrEmpty(binaryFolder))
                        {
                            binaryFolder = Path.Combine(workflowMapDirectory, "Binaries");
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Starting {0}", Path.GetFileNameWithoutExtension(map));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Publish " + map);
                        publisher.PublishProcess(map, binaryFolder, connectionString);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Done");
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                throw new Exception("Error parsing command line arguments");
            }
        }
    }
}