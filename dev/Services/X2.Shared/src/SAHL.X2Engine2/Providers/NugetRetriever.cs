using System;
using NuGet;

namespace SAHL.X2Engine2.Providers
{
    public class NugetRetriever : INugetRetriever
    {
        IX2EngineConfigurationProvider configurationProvider;
        System.IO.Abstractions.IFileSystem fileSystem;

        public NugetRetriever(IX2EngineConfigurationProvider configurationProvider, System.IO.Abstractions.IFileSystem fileSystem)
        {
            this.configurationProvider = configurationProvider;
            this.fileSystem = fileSystem;
        }

        public void InstallPackage(string packageName, string nugetBinariesPath, string packageVersion = null)
        {
            var nugetCacheEnvironmentVariableName = this.configurationProvider.GetNuGetCacheEnvironmentVariableName();
            var nugetCachePath = this.configurationProvider.GetNuGetCachePath();

            var uri = new Uri(nugetCachePath);

            if (!uri.IsAbsoluteUri)
            {
            }

            if (!this.fileSystem.Directory.Exists(nugetCachePath))
                this.fileSystem.Directory.CreateDirectory(nugetCachePath);

            Environment.SetEnvironmentVariable(nugetCacheEnvironmentVariableName, nugetCachePath, EnvironmentVariableTarget.Process);

            var sahlNuGetServerUrl = this.configurationProvider.GetSAHLNugetServerURL();
            var sahlNuGetServerPackageRepository = PackageRepositoryFactory.Default.CreateRepository(sahlNuGetServerUrl);

            PackageManager manager = new PackageManager(sahlNuGetServerPackageRepository, nugetBinariesPath);
            SemanticVersion nugetSemanticVersion = null;

            if (packageVersion == null) // get latest version of package
            {
                IPackage package = manager.SourceRepository.FindPackage(packageName);
                nugetSemanticVersion = package.Version;
                packageVersion = package.Version.ToString();
            }
            else
            {
                nugetSemanticVersion = new SemanticVersion(packageVersion);
            }

            manager.InstallPackage(packageName, nugetSemanticVersion, false, true);
        }
    }
}