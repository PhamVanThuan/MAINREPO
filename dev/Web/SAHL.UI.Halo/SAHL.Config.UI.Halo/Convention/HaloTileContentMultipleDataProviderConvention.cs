using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileContentMultipleDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var tileDataProviderConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileContentMultipleDataProvider`1"));
            if (!tileDataProviderConfigurations.Any()) { return; }

            foreach (var currentTileDataConfiguration in tileDataProviderConfigurations)
            {
                var haloTileConfiguration   = currentTileDataConfiguration.GenericTypeArguments[0];
                var halotileContentProvider = typeof(IHaloTileContentMultipleDataProvider<>);
                var genericType             = halotileContentProvider.MakeGenericType(haloTileConfiguration);

                registry.For(genericType).Use(type);
            }
        }
    }
}
