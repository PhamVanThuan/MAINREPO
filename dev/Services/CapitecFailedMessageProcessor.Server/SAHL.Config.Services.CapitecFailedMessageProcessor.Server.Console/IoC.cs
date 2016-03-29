using SAHL.Config.Core;
using SAHL.Config.Core.Conventions;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using StructureMap;

namespace SAHL.Config.Services.X2.NodeServer.Console
{
    public class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
                x.For<IIocContainer>().Use<StructureMapIocContainer>();
            });

            return ObjectFactory.Container;
        }
    }
}