using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SAHL.X2Designer.AppDomainManagement
{
    public class AppDomainManager : IDisposable
    {
        private AppDomain localDomain;
        private string rootDirectory;
        private string domainDirectory;
        private AssemblyLoader loader;
        private List<string> namespaces;

        public AppDomainManager()
        {
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
            Assembly.GetAssembly(loaderType).FullName, loaderType.FullName);//, false, BindingFlags.Default, null, new object[] { rootDirectory }, null, null
        }

        public string RootDirectory
        {
            get
            {
                return this.rootDirectory;
            }
            set
            {
                this.rootDirectory = value;
                this.loader.RootDirectory = value;
            }
        }

        public List<string> Namespaces
        {
            get
            {
                return this.namespaces;
            }
        }

        public string AddReferencedAssembly(string assemblyPath)
        {
            return loader.AddReferencedAssembly(assemblyPath, this.namespaces);
        }

        public void Dispose()
        {
            if (loader != null)
            {
                loader = null;
            }
            if (localDomain != null)
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