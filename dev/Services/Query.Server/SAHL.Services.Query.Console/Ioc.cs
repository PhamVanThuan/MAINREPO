using SAHL.Config.Services;
using SAHL.Config.Services.Query.Server;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using StructureMap;

namespace SAHL.Services.Query.Console
{
    public static class Ioc
    {

        public static IContainer Initialize()
        {

            var bootstrappper = new ServiceBootstrapper();
            bootstrappper.Initialise();

            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });

                x.For<IRawLogger>().Use<NullRawLogger>();
                x.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                x.For<IStartable>().Use<LookupTypeStartable>();
                
            });
            
            return ObjectFactory.Container;

        }
    }
}