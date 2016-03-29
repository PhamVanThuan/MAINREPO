using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileDynamicDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var dataProviderConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileDynamicDataProvider`1"));
            if (!dataProviderConfigurations.Any()) { return; }

            foreach (var currentDataConfiguration in dataProviderConfigurations)
            {
                var haloTileConfiguration = currentDataConfiguration.GenericTypeArguments[0];
                var provider              = typeof(IHaloTileDynamicDataProvider<>);
                var genericType           = provider.MakeGenericType(haloTileConfiguration);

                registry.For(genericType).Use(type);
            }
        }
    }
}
