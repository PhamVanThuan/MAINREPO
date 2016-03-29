using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SAHL.Tools.Workflow.Common.Compiler;

namespace SAHL.Tools.Workflow.Common.AppDomainManagement
{
    public class AssemblyLoader : MarshalByRefObject
    {
        private List<Assembly> assemblyCache;
        private List<Assembly> assemblyReflectionCache;
        private Dictionary<string, string> assemblies;
        private Dictionary<string, string> gacAssemblies;
        private Dictionary<string, string> assembliesNotFound;
        private string rootDirectory;

        public AssemblyLoader()
            : this("")
        {
        }

        public AssemblyLoader(string rootDirectory)
        {
            this.assemblyCache = new List<Assembly>();
            this.assemblyReflectionCache = new List<Assembly>();
            this.rootDirectory = rootDirectory;
            this.assemblies = new Dictionary<string, string>();
            this.assembliesNotFound = new Dictionary<string, string>();
            this.gacAssemblies = new Dictionary<string, string>();
        }

        public void LoadReferencedAssembly(string assemblyPath, string privateBinPath)
        {
            Assembly asmToLoad = this.LoadAssemblyInternal(assemblyPath);
        }

        public void ReflectionLoadAssembly(string assemblyPath)
        {
            try
            {
                if (assemblyPath.StartsWith("mscorlib"))
                    return;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
                Assembly asmToLoad = null;
				asmToLoad = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
				assemblyPath = asmToLoad.FullName == assemblyPath ? "System" : assemblyPath;
				this.AddToReflectionCache(asmToLoad, assemblyPath);

                // check the assemblies own references
                foreach (AssemblyName asmName in asmToLoad.GetReferencedAssemblies())
                {
                    if (asmName.FullName.StartsWith("mscorlib"))
                    {
                        continue;
                    }
                    string referencedAssemblyPath = GetAssemblyPath(asmName);
                    if (!string.IsNullOrEmpty(referencedAssemblyPath))
                    {
                        if (!this.assemblies.ContainsKey(asmName.FullName) && !this.gacAssemblies.ContainsKey(asmName.FullName))
                        {
                            ReflectionLoadAssembly(referencedAssemblyPath);
                        }
                    }
                    else
                    {
                        int a = 0;
                    }
                }
            }
            catch (System.IO.FileLoadException)
            {
                int a = 0;
            }
            catch (System.IO.FileNotFoundException)
            {
                if (!assembliesNotFound.ContainsKey(assemblyPath))
                {
                    this.assembliesNotFound.Add(assemblyPath, "NOT FOUND");
                }
            }
            catch
            {
                if (!assemblyPath.Contains("Castle.ActiveRecord"))
                {
                    throw;
                }
            }
            finally
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
            }
        }

        private string GetAssemblyPath(AssemblyName asmName)
        {
            string tofind = asmName.FullName.Substring(0, asmName.FullName.IndexOf(",")) + ".dll";

            if (tofind.ToLower().Contains("lucene"))
            {
                int a = 0;
            }

			if (Directory.Exists(rootDirectory))
            {
				string findDll = Path.Combine(rootDirectory, tofind);
                if (File.Exists(findDll))
                {
                    return findDll;
                }
            }

            return asmName.FullName;
        }

        public void LoadAssembly(string assemblyPath)
        {
            Assembly asmToLoad = this.LoadAssemblyInternal(assemblyPath);

            if (!this.assemblyCache.Contains(asmToLoad))
            {
                this.assemblyCache.Add(asmToLoad);
            }
        }

        public IAppDomainCompiler GetCompiler(string buildDirectory, string outputDirectory)
        {
            MarshalByRefObject result = this.CreateInstance(typeof(AppDomainCompiler).FullName, BindingFlags.CreateInstance, new object[] { buildDirectory, outputDirectory });
            IAppDomainCompiler compiler = result as IAppDomainCompiler;
            return compiler;
        }

        public MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags,
            object[] constructorParams)
        {
            Assembly owningAssembly = null;
            foreach (Assembly assembly in assemblyCache)
            {
                if (assembly.GetType(typeName) != null)
                {
                    owningAssembly = assembly;
                }
            }
            if (owningAssembly == null)
            {
                throw new InvalidOperationException("Could not find owning assembly for type " + typeName);
            }
            MarshalByRefObject createdInstance = owningAssembly.CreateInstance(typeName, false, bindingFlags, null,
                constructorParams, null, null) as MarshalByRefObject;
            if (createdInstance == null)
            {
                throw new ArgumentException("typeName must specify a Type that derives from MarshalByRefObject");
            }
            return createdInstance;
        }

        internal Dictionary<string, string> AssemblyReflectionCache
        {
            get
            {
                return this.assemblies;
            }
        }

        internal Dictionary<string, string> GACAssemblyReflectionCache
        {
            get
            {
                return this.gacAssemblies;
            }
        }

        internal Dictionary<string, string> AssemblyReflectionCacheNotFound
        {
            get
            {
                return this.assemblies;
            }
        }

        private Assembly LoadAssemblyInternal(string pathToAssembly)
        {
            return Assembly.Load(Path.GetFileNameWithoutExtension(pathToAssembly));
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            if (args.Name.StartsWith("System"))
            {
                try
                {
                    Assembly asmToLoad = Assembly.ReflectionOnlyLoad(args.Name);
                    this.AddToReflectionCache(asmToLoad, "System");
                    return asmToLoad;
                }
                catch
                {
                    throw;
                }
            }

            string tofind = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";

            // search the known directories
            string internalBins = Path.Combine(this.rootDirectory, "Internal Binaries");
            string externalBins = Path.Combine(this.rootDirectory, "External Binaries");
            string domainServiceBins = Path.Combine(this.rootDirectory, "DomainService\\DomainServiceBinaries");

            if (Directory.Exists(internalBins))
            {
                string findDll = Path.Combine(internalBins, tofind);
                if (File.Exists(findDll))
                {
                    try
                    {
                        Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                        string version = asmToLoad.ImageRuntimeVersion;
                        this.AddToReflectionCache(asmToLoad, "Internal Binaries");

                        return asmToLoad;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            if (Directory.Exists(externalBins))
            {
                string findDll = Path.Combine(externalBins, tofind);
                if (File.Exists(findDll))
                {
                    try
                    {
                        Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                        string version = asmToLoad.ImageRuntimeVersion;
                        this.AddToReflectionCache(asmToLoad, "External Binaries");

                        return asmToLoad;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            if (Directory.Exists(domainServiceBins))
            {
                string findDll = Path.Combine(domainServiceBins, tofind);
                if (File.Exists(findDll))
                {
                    try
                    {
                        Assembly asmToLoad = Assembly.ReflectionOnlyLoadFrom(findDll);
                        string version = asmToLoad.ImageRuntimeVersion;
                        this.AddToReflectionCache(asmToLoad, "Domain Service Binaries");

                        return asmToLoad;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return null;
        }

        private void AddToReflectionCache(Assembly assemblyToAdd, string assemblyPath)
        {
            if (assemblyToAdd.GlobalAssemblyCache)
            {
                if (!this.gacAssemblies.ContainsKey(assemblyToAdd.FullName))
                {
                    this.gacAssemblies.Add(assemblyToAdd.FullName, assemblyPath);
                }
            }
            else
            {
                if (!this.assemblies.ContainsKey(assemblyToAdd.FullName))
                {
                    this.assemblies.Add(assemblyToAdd.FullName, assemblyPath);
                }
            }
        }
    }
}