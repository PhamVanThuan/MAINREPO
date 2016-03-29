using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Convention
{
    public class HaloTileHeaderIconProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var allConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileHeaderIconProvider`1"));
            if (allConfigurations == null) { return; }

            foreach (var currentConfiguration in allConfigurations)
            {
                var iconProvider = currentConfiguration.GenericTypeArguments[0];
                var tileHeader   = typeof (IHaloTileHeaderIconProvider<>);
                var genericType  = tileHeader.MakeGenericType(iconProvider);

                registry.For(genericType).Use(type);
            }
        }
    }
}
