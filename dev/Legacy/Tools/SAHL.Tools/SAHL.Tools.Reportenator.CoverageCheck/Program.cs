using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Tools.Reportenator.CoverageCheck
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commandLineParser = new Parser();
            var commandLineArguments = new Options();

            if (commandLineParser.ParseArguments(args, commandLineArguments))
                if (!commandLineParser.ParseArguments(args, commandLineArguments))
                    Console.ReadLine();

            var pathToInterrogate = commandLineArguments.PathToInterrogate;
            var reportenatorFileName = commandLineArguments.ReportenatorFileName;

            if (!Directory.Exists(pathToInterrogate) || String.IsNullOrEmpty(reportenatorFileName) || !File.Exists(Path.Combine(pathToInterrogate, reportenatorFileName)))
            {
                return;
            }

            // Get report files in path
            var reportFilesInPath = Directory.GetFiles(pathToInterrogate, "*.rdl")
                .Select(x => Path.GetFileName(x))
                .ToArray();

            reportFilesInPath = reportFilesInPath.OrderBy(x => x).ToArray();

            // Get report files in reportenator.xml
            var reportenatorDocument = XDocument.Load(new FileStream(Path.Combine(pathToInterrogate, reportenatorFileName), FileMode.Open));
            var reportFilesInReportenator = reportenatorDocument.Elements("BatchParameters")
                                                    .Descendants()
                                                    .Select(x => x.Attribute("ReportName").Value)
                                                    .ToArray();
            reportFilesInReportenator = reportFilesInReportenator.OrderBy(x => x).ToArray();

            // comapre report files in path to those in reportenator.xml
            var filesInReportenatorNotInPath = FilesInReportenatorNotInPath(reportFilesInPath, reportFilesInReportenator);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(String.Format("# of Files In Reportenator not in Provided Path : {0}", filesInReportenatorNotInPath.Count));
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var fileInScriptenatorNotInPath in filesInReportenatorNotInPath)
            {
                Console.WriteLine(fileInScriptenatorNotInPath);
            }

            var filesInPathNotInReportenator = FilesInPathNotInReportenator(reportFilesInPath, reportFilesInReportenator);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(String.Format("# of Files In Path not in Reportentator file : {0}", filesInPathNotInReportenator.Count));
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var fileInPathNotInReportenator in filesInPathNotInReportenator)
            {
                Console.WriteLine(fileInPathNotInReportenator);
            }

            if (filesInPathNotInReportenator.Count > 0 || filesInReportenatorNotInPath.Count > 0)
            {
                Console.WriteLine("Failed");
                System.Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine("Success");
            }
        }

        public static List<string> FilesInReportenatorNotInPath(string[] reportFilesInPath, string[] reportFilesInScriptenator)
        {
            return reportFilesInScriptenator.Except(reportFilesInPath).ToList();
        }

        public static List<string> FilesInPathNotInReportenator(string[] reportFilesInPath, string[] reportFilesInScriptenator)
        {
            return reportFilesInPath.Except(reportFilesInScriptenator).ToList();
        }
    }

    public class Options
    {
        [Option('p', "PathToInterrogate", Required = true, HelpText = "The path to interrogate")]
        public string PathToInterrogate { get; set; }

        [Option('s', "ReportenatorFileName", Required = true, HelpText = "The Reportenator File Name")]
        public string ReportenatorFileName { get; set; }
    }
}