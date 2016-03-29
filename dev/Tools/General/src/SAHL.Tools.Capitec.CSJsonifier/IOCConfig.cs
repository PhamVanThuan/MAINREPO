using StructureMap.Graph;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    internal class IOCConfig
    {
        internal static void Register(Options options)
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
                    y.Convention<ScanConventionRegistration>();
                    y.ConnectImplementationsToTypesClosing(typeof(ITemplateForModel<>));
                    y.WithDefaultConventions();
                });

                x.For<INamespaceProvider>().Singleton().Use(new NamespaceProvider(options.Namespace,options.Prefix));
            });
        }
    }

    public class ScanConventionRegistration : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IScanConvention)))
            {
                registry.For(typeof(IScanConvention)).Use(type);
            }
        }
    }
}