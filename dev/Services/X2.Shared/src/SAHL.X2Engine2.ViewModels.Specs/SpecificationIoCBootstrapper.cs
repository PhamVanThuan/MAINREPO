using SAHL.Config.Services.Core.Conventions;
using SAHL.Core.Messaging;
using StructureMap;

namespace SAHL.X2Engine2.ViewModels.Specs
{
    public static class SpecificationIoCBootstrapper
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.Convention<CommandHandlerDecoratorConvention>();
                    scan.LookForRegistries();
                });
                x.For<IMessageBusAdvanced>().Use<InMemoryMessageBus>();
            });
        }
    }
}