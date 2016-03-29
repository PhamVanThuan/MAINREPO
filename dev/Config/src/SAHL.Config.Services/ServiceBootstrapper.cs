using SAHL.Config.Services.Core;
using SAHL.Core;
using StructureMap;

namespace SAHL.Config.Services
{
    public class ServiceBootstrapper : IServiceBootstrapper
    {
        public IIocContainer Initialise()
        {

            ObjectFactory.Initialize(x => x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                }));

            return ObjectFactory.Container.GetInstance<IIocContainer>();
        }
    }
}