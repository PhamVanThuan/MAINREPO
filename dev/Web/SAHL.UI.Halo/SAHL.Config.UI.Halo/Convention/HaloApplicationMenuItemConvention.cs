using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloApplicationMenuItemConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var applicationMenuItem = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloApplicationMenuItem"));
            if (applicationMenuItem == null) { return; }

            var haloApplication = applicationMenuItem.GenericTypeArguments[0];
            var menuItemType    = typeof(IHaloApplicationMenuItem<>);
            var genericType     = menuItemType.MakeGenericType(haloApplication);

            registry.For(genericType).Use(type);
        }
    }
}
