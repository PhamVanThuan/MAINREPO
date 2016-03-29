using SAHL.Core.IoC;
using StructureMap.Graph;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileContentDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var interfaces = type.GetInterfaces().Where(x => x.Name == "IHaloTileContentMultipleDataProvider" ||
                                                                     x.Name == "IHaloTileContentDataProvider");
            if (!type.Name.Contains("Base") && interfaces.Count() > 0 && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}