using NuGet;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    public sealed class NuGetService
    {
        private IPackageRepository localPackageRepository;
        private PackageServer packageServer;
        private IPackageRepository sahlNugetRepository;
        private IPackageRepository aggregateRepository;

        public NuGetService(PackageServer packageServer, IPackageRepository localPackageRepository, IPackageRepository sahlNugetRepository, IPackageRepository officialNugetRepository)
        {
            this.packageServer = packageServer;
            this.localPackageRepository = localPackageRepository;
            this.sahlNugetRepository = sahlNugetRepository;
            this.aggregateRepository = new AggregateRepository(new[] { sahlNugetRepository, officialNugetRepository });
        }
        public NuGetService(PackageServer packageServer, IPackageRepository localPackageRepository, IPackageRepository sahlNugetRepository)
        {
            this.packageServer = packageServer;
            this.localPackageRepository = localPackageRepository;
            this.sahlNugetRepository = sahlNugetRepository;
            this.aggregateRepository = new AggregateRepository(new[] { sahlNugetRepository });
        }


        public void InstallNuGetPackagesToLocalRepository(IDictionary<string, SemanticVersion> packagesToInstall)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("...Installing NuGet Packages from : {0}.", this.packageServer.Source);
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var nugetPackage in packagesToInstall)
            {
                Console.WriteLine(string.Format("........Package : {0}({1}) ", nugetPackage.Key, nugetPackage.Value));
                PackageManager manager = new PackageManager(this.aggregateRepository, this.localPackageRepository.Source);
                manager.InstallPackage(nugetPackage.Key, nugetPackage.Value, false, true);
            }
        }
        public void PushPackagesFromLocalRepository(string packageAuthorExclusion, string sahlNuGetApiKey)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("...Pushing NuGet Packages to : {0}", this.sahlNugetRepository.Source);
            Console.ForegroundColor = ConsoleColor.White;

            var packagesToPush = this.localPackageRepository.GetPackages().Where(x => x.Authors.FirstOrDefault() != packageAuthorExclusion);
            foreach (var package in packagesToPush)
            {
                if (!this.sahlNugetRepository.Exists(package))
                {
                    Console.WriteLine("Pushing Package: {0} Version: {1}", package.Id, package.Version.SpecialVersion);
                    packageServer.PushPackage(sahlNuGetApiKey, package, 20000);
                }
                else
                {
                    Console.WriteLine("Package: {0} Version: {1} already exists", package.Id, package.Version.SpecialVersion);
                }
            }
        }
    }
}
