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
    public class HaloModuleDynamicActionProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloModuleDynamicActionProvider`1"));
            if (configuration == null) { return; }

            var applicationModule     = configuration.GenericTypeArguments[0];
            var dynamicActionProvider = typeof(IHaloModuleDynamicActionProvider<>);
            var genericType           = dynamicActionProvider.MakeGenericType(applicationModule);

            registry.For(genericType).Use(type);
        }
    }
}
