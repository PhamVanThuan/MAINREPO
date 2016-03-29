using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileDynamicDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }

            var interfaces = type.GetInterfaces().Where(x => x.Name == "IHaloTileDynamicDataProvider");
            if (!type.Name.Contains("Base") && interfaces.Count() > 0 && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }

        }
    }
}
