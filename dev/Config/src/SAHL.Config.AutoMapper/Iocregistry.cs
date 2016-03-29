using System.Collections.Generic;

using AutoMapper;
using AutoMapper.Mappers;

using StructureMap.Configuration.DSL;

namespace SAHL.Config.AutoMapper
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));

                    scanner.Convention<AutoMapperProfileConvention>();
                    scanner.WithDefaultConventions();
                });

            this.For<ConfigurationStore>().Singleton().Use<ConfigurationStore>().Ctor<IEnumerable<IObjectMapper>>().Is(MapperRegistry.Mappers);
            this.For<IConfigurationProvider>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            this.For<IConfiguration>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            this.For<ITypeMapFactory>().Use<TypeMapFactory>();

            this.For<IMappingEngine>().Singleton().Use<MappingEngine>();
            this.SelectConstructor<MappingEngine>(() => new MappingEngine(null));
        }
    }
}
