using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileEditorConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTileEditorConfiguration`1"));
            if (configuration == null) { return; }

            var applicationModule                = configuration.GenericTypeArguments[0];
            var tileEditorConfigurationInterface = typeof(IHaloTileEditorConfiguration<>);
            var genericType                      = tileEditorConfigurationInterface.MakeGenericType(applicationModule);

            registry.For(genericType).Use(type);
        }
    }
}
