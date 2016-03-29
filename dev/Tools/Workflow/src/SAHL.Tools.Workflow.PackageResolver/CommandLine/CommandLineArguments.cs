using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Workflow.PackageResolver
{
	public class CommandLineArguments
	{
		[Option('p', null, Required = false, HelpText = "Packages to Update.")]
		public string PackagesToUpdate { get; set; }

		[Option('m', null, Required = true, HelpText = "X2 Workflow Map to load (*.x2p).")]
		public string X2WorkflowMap { get; set; }

		[Option('n', null, Required = true, HelpText = "Comma separated list of nuget repositories.")]
		public string NugetRepositories { get; set; }
	}
}
