using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using System.Xml.Linq;
using SAHL.Tools.Workflow.Common.Compiler;
using SAHL.Tools.Workflow.PackageResolver;
using CommandLine;
using CommandLine.Extensions;

namespace SAHL.Tools.Workflow.PackageResolver
{
	public class Program
	{
		static void Main(string[] args)
		{
			var commandLineArguments = new CommandLineArguments();
			var parser = new Parser();
			var result = parser.ParseArguments(args, commandLineArguments);


			if (result == false)
			{
				Console.Error.WriteLine("Something went wrong, please ensure that your arguments are correct");
				Console.Error.WriteLine("-----------");
				foreach (var argument in args)
				{
					Console.Error.Write(argument + " ");
				}
				Environment.Exit(-1);
			}

            var packageRepositories = new List<IPackageRepository>();
			foreach (var repository in commandLineArguments.NugetRepositories.Split(','))
			{
				packageRepositories.Add(PackageRepositoryFactory.Default.CreateRepository(repository));
			}
            var packageResolver = new SAHL.Tools.Workflow.Common.ReferenceChecking.PackageResolver(packageRepositories);

			var originalColor = Console.ForegroundColor;
			try
			{
				var workflowMaps = commandLineArguments.X2WorkflowMap.Split(',');
				var multipleWorkflowMaps = workflowMaps.Length > 1;
				var multipleWorkflowMapsBinariesPath = Path.Combine(Path.GetTempPath(), "WorkflowMapBinaries");
				if (multipleWorkflowMaps && Directory.Exists(multipleWorkflowMapsBinariesPath))
				{
					Console.WriteLine("Clearing out " + multipleWorkflowMapsBinariesPath);
					Directory.Delete(multipleWorkflowMapsBinariesPath, true);
				}
				foreach(var workflowMap in workflowMaps)
				{
					var binariesPath = multipleWorkflowMaps ? multipleWorkflowMapsBinariesPath : Path.Combine(Path.GetDirectoryName(workflowMap), "Binaries");
					Console.WriteLine(String.Format("Updating map {0} using Binaries Location ({1})", Path.GetFileName(workflowMap), binariesPath));
					packageResolver.ResolvePackages(commandLineArguments.PackagesToUpdate, workflowMap, binariesPath);
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("succesfully updated " + Path.GetFileName(commandLineArguments.X2WorkflowMap));
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine(ex.Message);
				Environment.Exit(-1);
			}
			Console.ForegroundColor = originalColor;
		}
	}
}
