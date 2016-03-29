using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Tools.Scriptenator.CoverageCheck
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commandLineParser = new Parser();
            var commandLineArguments = new Options();
            if (commandLineParser.ParseArguments(args, commandLineArguments))
                if (!commandLineParser.ParseArguments(args, commandLineArguments))
                {
                    Console.ReadLine();
                }
            string path = commandLineArguments.PathToInterrogate;
            string scriptenatorFileName = commandLineArguments.ScriptenatorFileName;
            bool teamcityOutput = commandLineArguments.TeamcityOutput;

            if (!Directory.Exists(path) || String.IsNullOrEmpty(scriptenatorFileName) || !File.Exists(Path.Combine(path, scriptenatorFileName)))
            {
                TeamCityOutput("Invalid Path", string.Format("Invalid Arguments Path: {0} ScriptenatorFileName: {1}", path, scriptenatorFileName), TeamcityStatus.ERROR, teamcityOutput);
                Environment.Exit(1);
                return;
            }

            var scriptFilesInPath = Directory.GetFiles(path, "*.sql")
                                        .Select(x => Path.GetFileName(x))
                                        .ToArray();

            scriptFilesInPath = scriptFilesInPath.OrderBy(x => x).ToArray();

            var scriptenatorDocument = XDocument.Load(new FileStream(Path.Combine(path, scriptenatorFileName), FileMode.Open));
            var scriptFilesInScriptenator = scriptenatorDocument.Elements("BatchParameters")
                                                    .Descendants()
                                                    .Select(x => x.Attribute("FileName").Value)
                                                    .ToArray();
            scriptFilesInScriptenator = scriptFilesInScriptenator.OrderBy(x => x).ToArray();

            var filesInScriptenatorNotInPath = FilesInScriptenatorNotInPath(scriptFilesInPath, scriptFilesInScriptenator);

            Console.ForegroundColor = ConsoleColor.Cyan;
            TeamCityOutput(String.Format("{0} # of Files In Scriptenator not in Provided Path : {1}", scriptFilesInScriptenator.Count(), filesInScriptenatorNotInPath.Count), "", filesInScriptenatorNotInPath.Count > 0 ? TeamcityStatus.FAILURE : TeamcityStatus.NORMAL, teamcityOutput);

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var fileInScriptenatorNotInPath in filesInScriptenatorNotInPath)
            {
                TeamCityOutput(string.Format("File missing:{0}", fileInScriptenatorNotInPath), "", TeamcityStatus.FAILURE, teamcityOutput);
            }

            var filesInPathNotInScriptenator = FilesInPathNotInScriptenator(scriptFilesInPath, scriptFilesInScriptenator);
            Console.ForegroundColor = ConsoleColor.Cyan;
            TeamCityOutput(String.Format("{0} # of Files In Path not in the Scriptenator file : {1}", scriptFilesInPath.Count(), filesInPathNotInScriptenator.Count), "", filesInPathNotInScriptenator.Count > 0 ? TeamcityStatus.FAILURE : TeamcityStatus.NORMAL, teamcityOutput);
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var fileInPathNotInScriptenator in filesInPathNotInScriptenator)
            {
                TeamCityOutput(string.Format("Missing in scriptenator file:{0}", fileInPathNotInScriptenator), "", TeamcityStatus.FAILURE, teamcityOutput);
            }
            if (filesInPathNotInScriptenator.Count > 0 || filesInScriptenatorNotInPath.Count > 0)
            {
                Console.WriteLine("Failed");
                Environment.Exit(1);
                return;
            }
            else
            {
                Console.WriteLine("Success");
                Environment.Exit(0);
                return;
            }
        }

        public static void TeamCityOutput(string message, string error, TeamcityStatus status, bool teamcityOutput)
        {
            if (teamcityOutput)
            {
                Console.WriteLine(string.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", message, error, status));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(error))
                    Console.WriteLine(string.Format("Message:{0}", message));
                else
                    Console.WriteLine(string.Format("Message:{0} Error:{1}", message, error));
            }
        }

        public enum TeamcityStatus
        {
            NORMAL,
            WARNING,
            FAILURE,
            ERROR
        }

        public static List<string> FilesInScriptenatorNotInPath(string[] scriptFilesInPath, string[] scriptFilesInScriptenator)
        {
            return scriptFilesInScriptenator.Except(scriptFilesInPath).ToList();
        }

        public static List<string> FilesInPathNotInScriptenator(string[] scriptFilesInPath, string[] scriptFilesInScriptenator)
        {
            return scriptFilesInPath.Except(scriptFilesInScriptenator).ToList();
        }
    }

    public class Options
    {
        [Option('p', "PathToInterrogate", Required = true, HelpText = "The path to interrogate")]
        public string PathToInterrogate { get; set; }

        [Option('s', "ScriptenatorFileName", Required = true, HelpText = "The Scriptenator File Name")]
        public string ScriptenatorFileName { get; set; }

        [Option('t', "Use teamcity output", Required = false)]
        public bool TeamcityOutput { get; set; }
    }
}