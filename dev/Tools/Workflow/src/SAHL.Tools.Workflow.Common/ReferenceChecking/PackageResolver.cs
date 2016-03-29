using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAHL.Tools.Workflow.Common.ReferenceChecking
{
	public class PackageResolver
	{
		private List<IPackageRepository> packageRepositories;
		private List<MapPackage> mapPackages;

        public PackageResolver(List<IPackageRepository> packageRepositories)
		{
			mapPackages = new List<MapPackage>();
            this.packageRepositories = packageRepositories;
		}

		public void ResolvePackages(string packagesToUpdate, string workflowMapLocation, string binariesLocation)
		{
			if (mapPackages.Count == 0)
			{
				XDocument map = null;
				using (var fileStream = File.OpenRead(workflowMapLocation))
				{
					map = XDocument.Load(fileStream);
				}

				mapPackages = map.Elements("ProcessName")
								  .Elements("NugetPackages")
								  .Elements("NugetPackage")
								  .Select(x => MapPackage.Parse(x))
								  .ToList();

				//If we have been asked to update a particular package
				if (!String.IsNullOrEmpty(packagesToUpdate))
				{
                    foreach (string package in packagesToUpdate.Split(',').ToList())
                    {
                        UpdatePackage(mapPackages, MapPackage.Parse(package));
                    }
				}			
			}

            UpdateMapPackageReferences(mapPackages, workflowMapLocation);

            //Install the packages
            foreach (var mapPackageToInstall in mapPackages)
            {
                DownloadPackage(mapPackageToInstall.PackageId, mapPackageToInstall.Version, binariesLocation);
            }

            var referenceChecker = new SAHL.Tools.Workflow.Common.ReferenceChecking.ReferenceChecker();
            referenceChecker.CheckAndUpdateReferences(new Options(workflowMapLocation, binariesLocation, binariesLocation));
		}

		private void UpdatePackage(List<MapPackage> mapPackages, MapPackage packageToUpdate)
		{
			var packageFromMapToUpdate = mapPackages.FirstOrDefault(x => x == packageToUpdate);
            //if (packageFromMapToUpdate == null)
            //{
            //    throw new KeyNotFoundException(String.Format("The package {0} is not part of the map packages", packageToUpdate.PackageId));
            //}
            if (packageFromMapToUpdate != null)
            {
                mapPackages.First(x => x == packageToUpdate).Version = packageToUpdate.Version;
            }
		}

        private void UpdateMapPackageReferences(IEnumerable<MapPackage> mapPackages, string workflowMapToUpdateLocation)
        {
            XDocument map = null;
            using (var fileStream = File.OpenRead(workflowMapToUpdateLocation))
            {
                map = XDocument.Load(fileStream);
            }
            map.Elements("ProcessName").Elements("NugetPackages").Elements("NugetPackage").Remove();
            foreach (var mapPackage in mapPackages)
            {
                var nugetPackageNode = new XElement("NugetPackage",
                    new XAttribute("PackageName", mapPackage.PackageId),
                    new XAttribute("Version", mapPackage.Version));
                map.Elements("ProcessName").Elements("NugetPackages").First().Add(nugetPackageNode);
            }
            map.Save(workflowMapToUpdateLocation);
        }

        private void DownloadPackage(string packageId, string version, string pathToDownloadTo)
        {
            foreach (var packageRepository in packageRepositories)
            {
                try
                {
                    var packageManager = new PackageManager(packageRepository, pathToDownloadTo);
                    packageManager.InstallPackage(packageId, new SemanticVersion(version), false, true);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(String.Format("Failed getting {0} (version : {1}) from {2}", packageId, version.ToString(), packageRepository.Source));
                }
            }
        }
	}

	public static class Extensions
	{
		public static bool CompareVersions(this string version, string anotherVersion)
		{
			return new SemanticVersion(version) == new SemanticVersion(anotherVersion);
		}
	}

	public class MapPackage
	{
		public string PackageId { get; set; }
		public string Version { get; set; }
		
		public List<MapPackage> Dependencies { get; set; }
		public IPackage NugetPackage { get; set; }

		public MapPackage(string packageId, string version, MapPackage dependsOn = null)
		{
			Dependencies = new List<MapPackage>();
			this.PackageId = packageId;
			this.Version = version;
		}
		public override bool Equals(object obj)
		{
			var packageToCompareTo = obj as MapPackage;
			if (packageToCompareTo != null)
			{
				return packageToCompareTo.PackageId == this.PackageId;
			}
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + PackageId.GetHashCode();
			return hash;
		}
		public static MapPackage Parse(string package)
		{
			if (String.IsNullOrEmpty(package))
			{
				throw new NullReferenceException("package");
			}
			return new MapPackage(package.Split('@')[0], package.Split('@')[1]);
		}
		public static MapPackage Parse(XElement element)
		{
			return new MapPackage(element.Attribute("PackageName").Value, element.Attribute("Version").Value);
		}
		public static MapPackage Parse(IPackage package)
		{
			return new MapPackage(package.Id, package.Version.ToString());
		}
		public void FindDependencies(List<IPackageRepository> packageRepositories, List<MapPackage> mapPackages)
		{
			var nugetPackage = FindPackage(packageRepositories, this.PackageId, new SemanticVersion(this.Version));
			if (nugetPackage == null)
				return;
			if (packageRepositories == null)
				throw new NullReferenceException("package repositories is null");
			if (packageRepositories == null)
				throw new NullReferenceException("nuget package");
			foreach (var dependencySet in nugetPackage.DependencySets)
			{
				foreach (var dependency in dependencySet.Dependencies)
				{
					var foundNugetPackage = FindPackage(packageRepositories, dependency.Id, dependency.VersionSpec);
					if (foundNugetPackage != null)
					{
						var foundMapPackage = new MapPackage(foundNugetPackage.Id, foundNugetPackage.Version.ToString(), null);
						
						this.Dependencies.Add(foundMapPackage);
						if (foundNugetPackage.DependencySets.Count() > 0)
						{
							InternalFindDependencies(packageRepositories, mapPackages, foundNugetPackage, this);
						}
					}
				}
			}
		}
		private static void InternalFindDependencies(List<IPackageRepository> packageRepositories, List<MapPackage> mapPackages, IPackage nugetPackage, MapPackage mapPackage)
		{
			if (nugetPackage == null)
				throw new NullReferenceException("package repository");
			if (packageRepositories == null)
				throw new NullReferenceException("nuget package");
			foreach (var dependencySet in nugetPackage.DependencySets)
			{
				foreach (var dependency in dependencySet.Dependencies)
				{
					var foundNugetPackage = FindPackage(packageRepositories, dependency.Id, dependency.VersionSpec);
					if (foundNugetPackage != null)
					{
						var foundMapPackage = new MapPackage(foundNugetPackage.Id, foundNugetPackage.Version.ToString(), null);
						
						mapPackage.Dependencies.Add(foundMapPackage);
						if (foundNugetPackage.DependencySets.Count() > 0)
						{
							InternalFindDependencies(packageRepositories, mapPackages, foundNugetPackage, mapPackage);
						}
					}
				}
			}
		}
		
		private static IPackage FindPackage(List<IPackageRepository> packageRepositories, string packageId, SemanticVersion version)
		{
			foreach (var packageRepository in packageRepositories)
			{
				IPackage foundPackage = packageRepository.FindPackage(packageId, version);
				if (foundPackage != null)
				{
					return foundPackage;
				}
			}
			throw new KeyNotFoundException(String.Format("Could not find package {0} {1} in any of the repositories provided : {2}",
															packageId,
															version.ToString(),
															String.Join(",", packageRepositories.Select(x => x.Source))));
		}
		private static IPackage FindPackage(List<IPackageRepository> packageRepositories, string packageId, IVersionSpec versionSpec)
		{
			foreach (var packageRepository in packageRepositories)
			{
				IPackage foundPackage = packageRepository.FindPackage(packageId, versionSpec, false, false);
				if (foundPackage != null)
				{
					return foundPackage;
				}
			}
			throw new KeyNotFoundException(String.Format("Could not find package {0} {1} in any of the repositories provided : {2}",
												packageId,
												versionSpec.ToString(),
												String.Join(",", packageRepositories.Select(x => x.Source))));
		}

		public static bool operator ==(MapPackage a, MapPackage b)
		{
			if (Object.ReferenceEquals(a, b))
			{
				return true;
			}
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			return a.PackageId == b.PackageId;
		}
		public static bool operator !=(MapPackage a, MapPackage b)
		{
			return !(a == b);
		}
	}
}
