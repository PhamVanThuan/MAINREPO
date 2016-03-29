using SAHL.Core;
using StructureMap;

namespace SAHL.Config.Services.Client
{
    public class ClientBootstrapper : IClientBootstrapper
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