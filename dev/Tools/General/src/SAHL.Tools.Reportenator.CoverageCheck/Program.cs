using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Tools.Reportenator.CoverageCheck
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var origConsolColour = Console.ForegroundColor;
            var commandLineParser = new Parser();
            var commandLineArguments = new Options();

            if (commandLineParser.ParseArguments(args, commandLineArguments))
                if (!commandLineParser.ParseArguments(args, commandLineArguments))
                    Console.ReadLine();

            string pathToInterrogate = commandLineArguments.PathToInterrogate;
            string reportenatorFileName = commandLineArguments.ReportenatorFileName;

            if (!Directory.Exists(pathToInterrogate) || String.IsNullOrEmpty(reportenatorFileName) || !File.Exists(Path.Combine(pathToInterrogate, reportenatorFileName)))
            {
                Console.WriteLine(string.Format("Invalid Arguments Path: {0} ReportenatorFileName: {1}", pathToInterrogate, reportenatorFileName));
                Environment.Exit(1);
                return;
            }

            // Get report files in path
            string[] reportFilesInPath = Directory.GetFiles(pathToInterrogate, "*.rdl")
                .Select(x => Path.GetFileName(x))
                .ToArray();

            reportFilesInPath = reportFilesInPath.OrderBy(x => x).ToArray();

            // Get report files in reportenator.xml
            var reportenatorDocument = XDocument.Load(new FileStream(Path.Combine(pathToInterrogate, reportenatorFileName), FileMode.Open));
            string[] reportFilesInReportenator = new string[] {};
            bool fail = false;
            if (reportenatorDocument.Elements("BatchParameters").Descendants().Count() == 0)
            {
                Console.WriteLine("Reportenator file is empty.");
            }
            else
            {
                reportFilesInReportenator = reportenatorDocument.Elements("BatchParameters")
                                                 .Descendants()
                                                 .Select(x => x.Attribute("ReportName").Value)
                                                 .ToArray();
                reportFilesInReportenator = reportFilesInReportenator.OrderBy(x => x).ToArray();
                // comapre report files in path to those in reportenator.xml
                var filesInReportenatorNotInPath = FilesInReportenatorNotInPath(reportFilesInPath, reportFilesInReportenator);
                if (filesInReportenatorNotInPath.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(String.Format("# of Files In Reportenator not in Provided Path : {0}", filesInReportenatorNotInPath.Count));
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var fileInScriptenatorNotInPath in filesInReportenatorNotInPath)
                    {
                        Console.WriteLine(fileInScriptenatorNotInPath);
                    }
                    fail = true;
                }
            }
            if (reportFilesInPath.Count() == 0)
            {
                Console.WriteLine("There are no reports in the report directory.");
            }
            else
            {
                var filesInPathNotInReportenator = FilesInPathNotInReportenator(reportFilesInPath, reportFilesInReportenator);
                if (filesInPathNotInReportenator.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(String.Format("# of Files In Path not in the Reportenator file : {0}", filesInPathNotInReportenator.Count));
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var fileInScriptenatorNotInPath in filesInPathNotInReportenator)
                    {
                        Console.WriteLine(fileInScriptenatorNotInPath);
                    }
                    fail = true;
                }
            }


            if (fail)
            {
                Console.ForegroundColor = origConsolColour;
                Environment.Exit(1);
            }
        }

        private static void ExitFailure(ConsoleColor origConsolColour)
        {
           
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