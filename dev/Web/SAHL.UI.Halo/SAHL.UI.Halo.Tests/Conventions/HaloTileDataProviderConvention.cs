using SAHL.Core.IoC;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var interfaces = type.GetInterfaces().Where(x => x.Name == "IHaloTileDataProvider");
            if (type.IsClass && !type.Name.Contains("Base") && interfaces.Count() > 0)
            {
                registry.For(type).Use(type);
            }

        }
    }
}