using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SAHL.Tools.Workflow.Common.AppDomainManagement
{
    public class DomainManager : IDisposable
    {
        private AppDomain localDomain;
        private string rootDirectory;
        private string domainDirectory;
        private AssemblyLoader loader;
        private List<string> namespaces;

        public DomainManager(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;

            string appName = Guid.NewGuid().ToString();
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = appName;
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath = appName;

            this.domainDirectory = Path.Combine(setup.ApplicationBase, appName);

            if (!Directory.Exists(this.domainDirectory))
            {
                Directory.CreateDirectory(this.domainDirectory);
            }
            this.localDomain = AppDomain.CreateDomain(appName, null, setup);

            Type loaderType = typeof(AssemblyLoader);
            this.loader = (AssemblyLoader)localDomain.CreateInstanceAndUnwrap(
            Assembly.GetAssembly(loaderType).FullName, loaderType.FullName, false, BindingFlags.Default, null, new object[] { rootDirectory }, null, null);
        }

        public void ReflectionLoadAssembly(string assemblyPath)
        {
            this.loader.ReflectionLoadAssembly(assemblyPath);
        }

        public Dictionary<string, string> AssemblyReflectionCache
        {
            get
            {
                return this.loader.AssemblyReflectionCache;
            }
        }

        public Dictionary<string, string> GACAssemblyReflectionCache
        {
            get
            {
                return this.loader.GACAssemblyReflectionCache;
            }
        }

        public Dictionary<string, string> NotFound
        {
            get
            {
                return this.loader.AssemblyReflectionCacheNotFound;
            }
        }

        public void Dispose()
        {
            if (this.loader != null)
            {
                this.loader = null;
            }
            if (this.localDomain != null)
            {
                AppDomain.Unload(localDomain);
            }
            if (Directory.Exists(this.domainDirectory))
            {
                Directory.Delete(this.domainDirectory, true);
            }
        }
    }
}