using SAHL.Config.Core.Conventions;
using SAHL.Config.Services.Core.Conventions;
using SAHL.Core.Caching;
using StructureMap;

namespace SAHL.X2Engine2.Console
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
                    scan.WithDefaultConventions();
                    scan.Convention<CommandHandlerDecoratorConvention>();
                    scan.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));
                    scan.LookForRegistries();
                });
            });
            return ObjectFactory.Container;
        }
    }
}