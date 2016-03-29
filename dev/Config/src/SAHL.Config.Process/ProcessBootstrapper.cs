using SAHL.Config.Services.Core;
using SAHL.Core;
using StructureMap;

namespace SAHL.Config.Process
{
    public class ProcessBootstrapper : IServiceBootstrapper
    {
        public IIocContainer Initialise()
        {
            var processPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var processFolder = System.IO.Path.GetDirectoryName(processPath);

            ObjectFactory.Initialize(x => x.Scan(scan =>
                {
                    scan.AssembliesFromPath(processFolder, y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                }));

            return ObjectFactory.Container.GetInstance<IIocContainer>();
        }
    }
}