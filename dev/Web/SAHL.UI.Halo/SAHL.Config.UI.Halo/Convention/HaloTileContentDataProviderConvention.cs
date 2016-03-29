using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileContentDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var tileDataProviderConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileContentDataProvider`1"));
            if (!tileDataProviderConfigurations.Any()) { return; }

            foreach (var currentTileDataConfiguration in tileDataProviderConfigurations)
            {
                var haloTileConfiguration   = currentTileDataConfiguration.GenericTypeArguments[0];
                var halotileContentProvider = typeof(IHaloTileContentDataProvider<>);
                var genericType             = halotileContentProvider.MakeGenericType(haloTileConfiguration);

                registry.For(genericType).Use(type);
            }
        }
    }
}
