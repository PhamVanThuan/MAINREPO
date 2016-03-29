using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommandLine;

namespace SAHL.Tools.Scriptenator.CoverageCheck
{
	class Program
	{
		static void Main(string[] args)
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

			if (!Directory.Exists(path) || String.IsNullOrEmpty(scriptenatorFileName) || !File.Exists(Path.Combine(path, scriptenatorFileName)))
			{
				return;
			}

			var scriptFilesInPath = Directory.GetFiles(path, "*.sql")
										.Select(x=>Path.GetFileName(x))
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
			Console.WriteLine(String.Format("# of Files In Scriptenator not in Provided Path : {0}", filesInScriptenatorNotInPath.Count));
			Console.ForegroundColor = ConsoleColor.Red;
			foreach (var fileInScriptenatorNotInPath in filesInScriptenatorNotInPath)
			{
				Console.WriteLine(fileInScriptenatorNotInPath);
			}

			var filesInPathNotInScriptenator = FilesInPathNotInScriptenator(scriptFilesInPath, scriptFilesInScriptenator);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(String.Format("# of Files In Path not in the Scriptenator file : {0}", filesInPathNotInScriptenator.Count));
			Console.ForegroundColor = ConsoleColor.Gray;
			foreach (var fileInPathNotInScriptenator in filesInPathNotInScriptenator)
			{
				Console.WriteLine(fileInPathNotInScriptenator);
			}
            if (filesInPathNotInScriptenator.Count > 0 || filesInScriptenatorNotInPath.Count > 0)
            {
                Console.WriteLine("Failed");
                System.Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine("Success");
            }
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
	}
}
