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
    public class HaloTileActionEditConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().FirstOrDefault(x => x.Name.StartsWith("IHaloTileActionEdit`1"));
            if (configuration == null) { return; }

            var childTileConfiguration = configuration.GenericTypeArguments[0];
            var tileActionDrilldown = typeof(IHaloTileActionEdit<>);
            var genericType = tileActionDrilldown.MakeGenericType(childTileConfiguration);

            registry.For(genericType).Use(type);
        }
    }
}
