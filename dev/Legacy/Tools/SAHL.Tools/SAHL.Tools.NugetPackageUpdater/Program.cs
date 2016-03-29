using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.NugetPackageUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.WriteLine("Please supply a Working Directory, a Config with the Packages to Update as well as a Nuget Repository to use.");
				return;
			}
			var workingDirectory = args[0];
			var configFilePath = args[1];
			var nugetRepository = args[2];
			var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));

			var nugetLocation = Path.Combine(workingDirectory, config.NugetExeLocation);
			if (!File.Exists(nugetLocation))
			{
				Console.Error.WriteLine("The nuget location does not exist : {0}", nugetLocation);
				return;
			}

			var repositoryLocation = Path.Combine(workingDirectory, config.RepositoryLocation);
			if (!Directory.Exists(repositoryLocation))
			{
				Console.Error.WriteLine("The repository location does not exist : {0}", repositoryLocation);
				return;
			}

			foreach (var projectToUpdate in config.ProjectsToUpdate)
			{
				foreach (var packageToUpdate in projectToUpdate.PackagesToUpdate)
				{
					var projectLocation = Path.Combine(workingDirectory, projectToUpdate.Location);
					if (!Directory.Exists(projectLocation))
					{
						Console.Error.WriteLine("Could not update package : {0}. The project location does not exist : {1}", packageToUpdate, projectLocation);
						continue;
					}

					Process process = new Process();

					process.StartInfo = new ProcessStartInfo(nugetLocation, String.Format("update {0} -Id {1} -RepositoryPath \"{2}\" -Source {3}", projectToUpdate.Name, packageToUpdate, repositoryLocation, nugetRepository));
					process.StartInfo.WorkingDirectory = Path.Combine(workingDirectory, projectToUpdate.Location);
					process.StartInfo.RedirectStandardError = false;
					process.StartInfo.RedirectStandardOutput = false;
					process.StartInfo.UseShellExecute = false;
					process.Start();
					process.WaitForExit();
				}
			}
		}
	}
	public class Config
	{
		public string NugetExeLocation { get; set; }
		public string RepositoryLocation { get; set; }
		public PackageToUpdate[] ProjectsToUpdate { get; set; }
	}
	public class PackageToUpdate
	{
		public string Name { get; set; }
		public string Location { get; set; }
		public string[] PackagesToUpdate { get; set; }
	}
}
