using SAHL.UI.Halo.Shared.Configuration;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;

namespace SAHL.Config.UI.Halo
{
    public class HaloTileModuleConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var interfaces = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloModuleTileConfiguration")).ToList();
            foreach (var configuration in interfaces)
            {
                var applicationModule = configuration.GenericTypeArguments[0];
                var rootTileConfigurationInterface = typeof(IHaloModuleTileConfiguration<>);
                var genericType = rootTileConfigurationInterface.MakeGenericType(applicationModule);

                registry.For(genericType).Use(type);
            }
        }
    }
}
