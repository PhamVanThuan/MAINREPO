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
    public class HaloTileHeaderTextProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var allConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileHeaderTextProvider`1"));
            if (allConfigurations == null) { return; }

            foreach (var currentConfiguration in allConfigurations)
            {
                var textProvider = currentConfiguration.GenericTypeArguments[0];
                var tileHeader   = typeof(IHaloTileHeaderTextProvider<>);
                var genericType  = tileHeader.MakeGenericType(textProvider);

                registry.For(genericType).Use(type);
            }
        }
    }
}
