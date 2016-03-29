using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileLinkedConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloRootTileLinkedConfiguration`1"));
            if (configuration == null) { return; }

            var rootTileConfiguration            = configuration.GenericTypeArguments[0];
            var linkedTileConfigurationInterface = typeof(IHaloRootTileLinkedConfiguration<>);
            var genericType                      = linkedTileConfigurationInterface.MakeGenericType(rootTileConfiguration);

            registry.For(genericType).Use(type);
        }
    }
}
