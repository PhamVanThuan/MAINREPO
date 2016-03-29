using SAHL.Config.Core.Conventions;
using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.IoC;
using SAHL.Services.EventProjection.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.EventProjection.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                scanner.WithDefaultConventions();
                scanner.AddAllTypesOf(typeof(IEventProjector)).NameBy(x => x.Name);
                scanner.AddAllTypesOf(typeof(IEventProjector<>)).NameBy(x => x.Name);
                scanner.AddAllTypesOf(typeof(IUIStatementsProvider));
                scanner.Convention<TableProjectorHandlerDecoratorConvention>();
                scanner.WithDefaultConventions();
            });

            For<EventProjectionService>().Singleton().Use<EventProjectionService>().Named("EventProjectionService");
            For<IStartableService>().Use(x=>x.GetInstance<EventProjectionService>("EventProjectionService"));
            For<IStoppableService>().Use(x => x.GetInstance<EventProjectionService>("EventProjectionService"));
        }
    }
}