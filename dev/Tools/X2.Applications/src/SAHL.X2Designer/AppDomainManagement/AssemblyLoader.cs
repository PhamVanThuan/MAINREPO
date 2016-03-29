using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAHL.X2Designer.AppDomainManagement
{
    public class AssemblyLoader : MarshalByRefObject
    {
        private List<AssemblyReference> assemblyCache;

        public AssemblyLoader()
        {
            this.assemblyCache = new List<AssemblyReference>();
        }

        public string RootDirectory
        {
            get;
            set;
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            try
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
                Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                this.AddToCache(asmToLoad, assemblyPath);

                return asmToLoad;
            }
            finally
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
            }
        }

        public string AddReferencedAssembly(string assemblyPath, List<string> availableNamespaces)
        {
            Assembly asmToLoad = LoadAssembly(assemblyPath);
            IEnumerable<string> namespaces = asmToLoad.GetExportedTypes()
                            .Where(x => !string.IsNullOrEmpty(x.Namespace))
                            .Select(t => t.Namespace)
                            .Distinct();

            foreach (string namespaceStr in namespaces)
            {
                if (!availableNamespaces.Contains(namespaceStr))
                {
                    availableNamespaces.Add(namespaceStr);
                }
            }

            return asmToLoad.FullName;
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("System"))
            {
                Assembly asmToLoad = Assembly.ReflectionOnlyLoad(args.Name);
                this.AddToCache(asmToLoad, "");
                return asmToLoad;
            }

            string tofind = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";

            // search the known directories
            string internalBins = Path.Combine(this.RootDirectory, "Internal Binaries");
            string externalBins = Path.Combine(this.RootDirectory, "External Binaries");
            string domainServiceBins = Path.Combine(this.RootDirectory, "DomainService\\DomainServiceBinaries");

            if (Directory.Exists(internalBins))
            {
                string findDll = Path.Combine(internalBins, tofind);
                if (File.Exists(findDll))
                {
                    Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                    this.AddToCache(asmToLoad, findDll);

                    return asmToLoad;
                }
            }

            if (Directory.Exists(externalBins))
            {
                string findDll = Path.Combine(externalBins, tofind);
                if (File.Exists(findDll))
                {
                    Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                    this.AddToCache(asmToLoad, findDll);

                    return asmToLoad;
                }
            }

            if (Directory.Exists(domainServiceBins))
            {
                string findDll = Path.Combine(domainServiceBins, tofind);
                if (File.Exists(findDll))
                {
                    Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                    this.AddToCache(asmToLoad, findDll);

                    return asmToLoad;
                }
            }

            return null;
        }

        private void AddToCache(Assembly assemblyToAdd, string assemblyPath)
        {
            AssemblyReference reference = new AssemblyReference(assemblyPath, assemblyToAdd);
            if (!this.assemblyCache.Contains(reference))
            {
                this.assemblyCache.Add(reference);
            }
        }
    }
}