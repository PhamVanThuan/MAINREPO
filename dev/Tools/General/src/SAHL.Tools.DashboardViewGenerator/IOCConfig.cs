using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public class IOCConfig
    {
        internal static IContainer Register(Options options)
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
                    y.WithDefaultConventions();
                });
                x.For<Options>().Singleton().Use(options);
            });
            return ObjectFactory.Container;
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
}
