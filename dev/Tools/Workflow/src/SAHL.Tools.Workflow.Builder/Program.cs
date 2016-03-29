using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using CommandLine;
using SAHL.Tools.Workflow.Builder.CommandLine;
using SAHL.Tools.Workflow.Common.Compiler;
using SAHL.Tools.Workflow.Common.ReferenceChecking;
using NuGet;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace SAHL.Tools.Workflow.Builder
{
	public class Program
	{
		private static void Main(string[] args)
		{
			var compiler = new Compiler();

			var arguments = new CommandLineArguments();
			var parser = new Parser();
			bool parserResult = parser.ParseArguments(args, arguments);
			if (parserResult)
			{
				var workflowMapsToCompile = arguments.X2WorkflowMaps.Split(',');
				string rootDirectory = String.Empty;
				string outputDirectory = String.Empty;

				var binariesDirectory = Path.Combine(Path.GetTempPath(), "WorkflowMapBinaries");

				if (Directory.Exists(binariesDirectory))
				{
					Directory.Delete(binariesDirectory, true);
				}

				var officialNuGetUrl = ConfigurationManager.AppSettings["OfficialNuGetUrl"];
				var officialNuGetApi = ConfigurationManager.AppSettings["OfficialNuGetApi"];

				var sahlDeployNuGetInstall = ConfigurationManager.AppSettings["SAHLDeployNuGetInstall"];
                var nugetInstallApi = sahlDeployNuGetInstall;
                if (!String.IsNullOrEmpty(arguments.NugetPullApiUrl))
                {
                    nugetInstallApi = arguments.NugetPullApiUrl;
                }
                var sahlDeployNuGetPushApi = ConfigurationManager.AppSettings["SAHLDeployNuGetPushApi"];
				var sahlDeployNuGetApiKey = ConfigurationManager.AppSettings["SAHLDeployNuGetApiKey"];

				Console.WriteLine("The nuget binaries will be installed to the following location : " + binariesDirectory);
				if (arguments.BuildServerMode)
				{
					compiler.InstallNuGetPackages(workflowMapsToCompile.First(), binariesDirectory, nugetInstallApi);
				}
				else
				{
					IList<NuGetPushOptions> pushOptionsList = new List<NuGetPushOptions>();
					NuGetPushOptions nugetPushOptions = new NuGetPushOptions(nugetInstallApi, sahlDeployNuGetPushApi, sahlDeployNuGetApiKey);
					pushOptionsList.Add(nugetPushOptions);
					nugetPushOptions = new NuGetPushOptions(nugetInstallApi, sahlDeployNuGetPushApi, sahlDeployNuGetApiKey);
					pushOptionsList.Add(nugetPushOptions);

					compiler.InstallAndPushNuGetPackages(workflowMapsToCompile.First(), binariesDirectory, nugetInstallApi, officialNuGetApi, officialNuGetUrl, pushOptionsList);
				}

				try
				{
					foreach (var workflowMapToCompile in workflowMapsToCompile)
					{
						var workflowMapName = Path.GetFileName(workflowMapToCompile);
						// set the compiler options
						bool reloadReferences = arguments.CheckReferences;
						CompilerOptions compilerOptions = new CompilerOptions(Path.Combine(Path.GetDirectoryName(workflowMapToCompile), "Binaries"), binariesDirectory, reloadReferences);

						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine(string.Format("Compiling workflow map {0}:", Path.GetFileNameWithoutExtension(workflowMapName)));

						// compile the X2 process file
						CompilerResults results = compiler.Compile(workflowMapToCompile, compilerOptions);

						if (results.Errors.HasErrors)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine(string.Format("Compiling workflow map {0} failed!", workflowMapName));
							Console.WriteLine();
							foreach (CompilerError error in results.Errors)
							{
								if (error.IsWarning)
								{
									continue;
								}
								int line = error.Line;
								int column = error.Column;
								string errorText = error.ErrorText;
								Console.WriteLine(string.Format("Line [{0}], Column [{1}] - {2}", line, column, errorText));
							}

							Console.WriteLine("");
							Console.WriteLine(string.Format("Error compiling workflow map! {0} Errors.", results.Errors.Count));
							compiler.Dispose();
							Environment.Exit(-1);
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine(string.Format("Compiling workflow map {0} Succeeded!", workflowMapName));
						}
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(ex.Message);
					Console.WriteLine("An error occurred during the compilation process in the compiler. This shouldn't happen ask a dev why");
                    Environment.Exit(-1);
				}
				finally
				{
					if (compiler != null)
					{
						compiler.Dispose();
						compiler = null;
					}
				}
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There was an error parsing the arguments.");
				Environment.Exit(-1);
			}
		}
	}
}