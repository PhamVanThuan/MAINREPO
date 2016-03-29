using Microsoft.VisualStudio.Shell;
using SAHL.VSExtensions.Interfaces.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace SAHomeloans.SAHL_VSExtensions.Mappings
{
    internal class IOCConfig
    {
        internal static void Register(Package sAHL_VSExtensionsPackage)
        {
            Assembly currentAssembly = typeof(IOCConfig).Assembly;
            UriBuilder uri = new UriBuilder(currentAssembly.Location);
            string path = Uri.UnescapeDataString(uri.Path);
            string env = Path.GetDirectoryName(path);
            StructureMap.ObjectFactory.Configure(x =>
            {
                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.AssembliesFromPath(env, n => n.FullName.StartsWith("SAHL"));
                    y.Convention<ISAHLDialogConvention>();
                    y.ConnectImplementationsToTypesClosing(typeof(ISAHLControlForConfiguration<>));
                    y.WithDefaultConventions();
                });
                x.For<IServiceProvider>().Use(sAHL_VSExtensionsPackage);
            });
        }
    }
}