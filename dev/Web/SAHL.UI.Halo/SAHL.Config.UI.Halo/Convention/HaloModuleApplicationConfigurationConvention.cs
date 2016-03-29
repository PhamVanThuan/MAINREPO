using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloModuleApplicationConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloModuleApplicationConfiguration"));
            if (configuration == null) { return; }

            var haloApplication                    = configuration.GenericTypeArguments[0];
            var haloApplicationModuleConfiguration = typeof(IHaloModuleApplicationConfiguration<>);
            var genericType                        = haloApplicationModuleConfiguration.MakeGenericType(haloApplication);

            registry.For(genericType).Use(type);
        }
    }
}
