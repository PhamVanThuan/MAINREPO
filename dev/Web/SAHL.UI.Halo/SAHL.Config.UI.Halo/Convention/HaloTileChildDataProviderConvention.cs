using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileChildDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var dataProviderConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileChildDataProvider`1"));
            if (!dataProviderConfigurations.Any()) { return; }

            foreach (var currentDataConfiguration in dataProviderConfigurations)
            {
                var haloTileConfiguration = currentDataConfiguration.GenericTypeArguments[0];
                var provider              = typeof(IHaloTileChildDataProvider<>);
                var genericType           = provider.MakeGenericType(haloTileConfiguration);

                registry.For(genericType).Use(type);
            }
        }
    }
}
