using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SAHL.Tools.Workflow.Common.AppDomainManagement;
using SAHL.Tools.Workflow.Common.Persistance;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore;
using SAHL.Tools.Workflow.Common.WorkflowElements;
using System.Linq;
using NuGet;
using SAHL.Tools.Workflow.Common.ReferenceChecking;
using System.Xml.Linq;

namespace SAHL.Tools.Workflow.Common.Compiler
{
	public class Compiler : IDisposable
	{
		private AppDomain newDomain = null;
		private string buildRelDir = string.Empty;
		private string buildDir = string.Empty;
		private string lastCompiledSourceCode;
		private string buildAssemblyPath;
		private AssemblyLoader loader;

		public Compiler()
		{
		}

		public string BuildPath
		{
			get
			{
				return this.buildDir;
			}
		}

		public void Dispose()
		{
			if (loader != null)
			{
				loader = null;
			}
		}

		public string LastCompiledSourceCode
		{
			get
			{
				return this.lastCompiledSourceCode;
			}
		}

        public bool PushNuGetPackages(string localNugetRepositoryFolder, IList<NuGetPushOptions> nugetPushOptionsList)
        {
            foreach (var nugetPushOptions in nugetPushOptionsList)
            {
                var localNuGetRepository = PackageRepositoryFactory.Default.CreateRepository(localNugetRepositoryFolder);
                var sahlPackageRepository = PackageRepositoryFactory.Default.CreateRepository(nugetPushOptions.SAHLNuGetInstallRepo);
                var agent = NuGet.HttpUtility.CreateUserAgentString("SAHL.Workflow.Builder.PushNuGetPackages");
                var packageServer = new PackageServer(nugetPushOptions.SAHLNuGetPushApi, agent);
                var nugetService = new NuGetService(packageServer, localNuGetRepository, sahlPackageRepository);

                nugetService.PushPackagesFromLocalRepository("SA Home Loans", nugetPushOptions.SAHLNuGetApiKey);                
            }
           
            return true;
        }

        public bool InstallNuGetPackages(string x2Map, string binariesFolder, string sahlNuGetInstall)
        {
            return InstallAndPushNuGetPackages(x2Map, binariesFolder, sahlNuGetInstall, String.Empty, String.Empty, null); 
        }
        public bool InstallAndPushNuGetPackages(string x2Map, string binariesFolder, string sahlNuGetInstall, string officialNuGetApi, string officialNuGetUrl, IList<NuGetPushOptions> nugetPushOptionsList)
        {
            Console.WriteLine("Deleting local binaries folder");
            if (Directory.Exists(binariesFolder))
                Directory.Delete(binariesFolder, true);

            var sahlPackageRepository = PackageRepositoryFactory.Default.CreateRepository(sahlNuGetInstall);

            var localNuGetRepository = PackageRepositoryFactory.Default.CreateRepository(binariesFolder);

            var agent = NuGet.HttpUtility.CreateUserAgentString("SAHL.Workflow.Builder.InstallNuGetPackages");
            var packageServer = new PackageServer(sahlNuGetInstall, agent);

            var nugetService = default(NuGetService);

            if (!String.IsNullOrEmpty(officialNuGetUrl))
            {
                var officialPackageRepository = PackageRepositoryFactory.Default.CreateRepository(officialNuGetApi);
                nugetService = new NuGetService(packageServer, localNuGetRepository, sahlPackageRepository, officialPackageRepository);
            }
            else
            {
                nugetService = new NuGetService(packageServer, localNuGetRepository, sahlPackageRepository);
            }

            var packagesFromMaps = Compiler.GetNuGetPackagesFromMap(x2Map);
            nugetService.InstallNuGetPackagesToLocalRepository(packagesFromMaps);

            if (nugetPushOptionsList!=null && nugetPushOptionsList.Count>0)
            {
                PushNuGetPackages(binariesFolder, nugetPushOptionsList);
            }
       
            return true;
        }

		public CompilerResults Compile(string pathToWorkflowFile, CompilerOptions compilerOptions)
		{
			if (Directory.Exists(compilerOptions.OutputDirectory))
			{
				Directory.Delete(compilerOptions.OutputDirectory, true);
			}
			Directory.CreateDirectory(compilerOptions.OutputDirectory);

            if (compilerOptions.ReloadReferences)
            {
                //  reload the references before compiling
                ReferenceChecker checker = new ReferenceChecker();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("...Reloading References.", Path.GetFileNameWithoutExtension(pathToWorkflowFile)));
                try
                {
                    checker.CheckAndUpdateReferences(new Options(pathToWorkflowFile, compilerOptions.OutputDirectory, compilerOptions.OutputDirectory));
                }
                catch (System.UnauthorizedAccessException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("...Could not access file for update.", Path.GetFileNameWithoutExtension(pathToWorkflowFile)));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format("...Done.", Path.GetFileNameWithoutExtension(pathToWorkflowFile)));
            }

			Process process = this.GetProcessFromFile(pathToWorkflowFile, compilerOptions);

			IAppDomainCompiler appDomainCompiler = this.GetConfiguredCompiler(process, compilerOptions);
			CompilerResults results = appDomainCompiler.Compile(process);
			this.lastCompiledSourceCode = appDomainCompiler.LastSourceCodeCompiled;

			appDomainCompiler = null;
			AppDomain.Unload(newDomain);

			return results;
		}

		private Process GetProcessFromFile(string pathToWorkflowFile, CompilerOptions compilerOptions)
		{
			using (FileStream fs = new FileStream(pathToWorkflowFile, FileMode.Open, FileAccess.Read))
			{
				IWorkflowPersistanceStore store = new XmlStreamPersistanceStore(fs);

				Process process = store.LoadProcess();

				return process;
			}
		}

		private IAppDomainCompiler GetConfiguredCompiler(Process process, CompilerOptions compilerOptions)
		{
			buildRelDir = "build";

			buildDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, buildRelDir);

			// setup the build directory
			if (Directory.Exists(buildDir))
			{
				Directory.Delete(buildDir, true);
			}
			Directory.CreateDirectory(buildDir);

			// copy all the referenced assemblies to the build directory
			List<string> copiedAssemblies = new List<string>();
			foreach (AssemblyReference assemblyRef in process.AssemblyReferences)
			{
				string assemblyPath = assemblyRef.Path;
				assemblyPath = Path.Combine(compilerOptions.BinariesDirectory, assemblyRef.Path);
				buildAssemblyPath = Path.Combine(buildDir, Path.GetFileName(assemblyPath));
				if (File.Exists(assemblyPath))
				{
					File.Copy(assemblyPath, buildAssemblyPath, true);
					copiedAssemblies.Add(buildAssemblyPath);
				}
				else
				{
					// log a message
				}
			}

			string appName = string.Format("ProxyCompiler - {0}", process.SafeName);
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationName = appName;
			setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			setup.PrivateBinPath = buildRelDir;
			setup.ShadowCopyFiles = "true";

			newDomain = AppDomain.CreateDomain(appName, null, setup);

			Type loaderType = typeof(AssemblyLoader);
			loader = (AssemblyLoader)newDomain.CreateInstanceAndUnwrap(Assembly.GetAssembly(loaderType).FullName, loaderType.FullName);

			loader.LoadAssembly("SAHL.Tools.Workflow.Common.dll");
            foreach (string assemblyLocation in copiedAssemblies)
            {
                loader.LoadAssembly(assemblyLocation);
            }

			IAppDomainCompiler compiler = loader.GetCompiler(buildDir, compilerOptions.OutputDirectory);
			return compiler;
		}

        public static IDictionary<string, SemanticVersion> GetNuGetPackagesFromMap(string x2Map)
        {
            IDictionary<string, SemanticVersion> nugetPackages = new Dictionary<string, SemanticVersion>();

            XDocument xdoc = null;
            using (FileStream fs = new FileStream(x2Map, FileMode.Open, FileAccess.Read))
            {
                xdoc = XDocument.Load(fs);
            }

            foreach (var nugetPackageElement in xdoc.Element("ProcessName").Element("NugetPackages").Elements())
            {
                var packageName = nugetPackageElement.Attribute("PackageName").Value;
                var packageVersion = nugetPackageElement.Attribute("Version").Value;

                if (packageVersion.Contains("-")) // This is a pre release
                {
                    var versionNumbers = packageVersion.Split('-').First();
                    var prereleaseTag = packageVersion.Split('-').Last();
                    var nugetSemanticVersion = new SemanticVersion(Version.Parse(versionNumbers), prereleaseTag);
                    nugetPackages.Add(packageName, nugetSemanticVersion);
                }
                else
                {
                    var nugetSemanticVersion = new SemanticVersion(Version.Parse(packageVersion));
                    nugetPackages.Add(packageName, nugetSemanticVersion);
                }
            }
            return nugetPackages;
        }
	}
}