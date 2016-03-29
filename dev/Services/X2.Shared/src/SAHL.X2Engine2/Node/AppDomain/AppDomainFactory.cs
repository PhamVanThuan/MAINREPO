using System;
using System.IO.Abstractions;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.AppDomain;

namespace SAHL.X2Engine2.Node.AppDomain
{
    public class AppDomainFactory : IAppDomainFactory
    {
        private IFileSystem fileSystem;

        public AppDomainFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public System.AppDomain Create(ProcessDataModel processDataModel, string directory, string directoryFullPath)
        {
            string configFileName = string.Format("{0}.config", processDataModel.Name);
            string configFileFullPath = fileSystem.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, configFileName);
            string configFileNewFullPath = fileSystem.Path.Combine(directoryFullPath, configFileName);

            string appName = string.Format("X2ProxyLoader-{0}", processDataModel.ID);
            AppDomainSetup setup = new AppDomainSetup
            {
                ApplicationName = appName,
                ApplicationBase = System.AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = directory,
                ConfigurationFile = configFileNewFullPath,
                PrivateBinPathProbe = "noprobe",
            };

            return System.AppDomain.CreateDomain(setup.ApplicationName, null, setup);
        }
    }
}