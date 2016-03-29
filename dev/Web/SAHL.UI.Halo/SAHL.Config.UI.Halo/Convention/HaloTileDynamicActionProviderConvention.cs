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
    public class HaloTileDynamicActionProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTileDynamicActionProvider`1"));
            if (configuration == null) { return; }

            var applicationModule     = configuration.GenericTypeArguments[0];
            var dynamicActionProvider = typeof(IHaloTileDynamicActionProvider<>);
            var genericType           = dynamicActionProvider.MakeGenericType(applicationModule);

            registry.For(genericType).Use(type);
        }
    }
}
