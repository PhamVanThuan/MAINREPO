using SAHL.Core.IoC;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.LegacyEventGenerator.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
                {
                    x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    x.AddAllTypesOf<ILegacyEventGeneratorQuery>();
                });

            For<ILegacyEventQueryMappingService>().Singleton().Use<LegacyEventQueryMappingService>();
            For<IStartable>().Use(x => x.GetInstance<ILegacyEventQueryMappingService>());
            For<IStoppable>().Use(x => x.GetInstance<ILegacyEventQueryMappingService>());
        }
    }
}