using SAHL.Core.IoC;
using StructureMap.Graph;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileSqlDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {

            if (type.Namespace == null)
            {
                return;
            }
            if (type.BaseType != null && type.BaseType.Name.Contains("HaloTileBaseContentDataProvider"))
            {
                registry.For(type).Use(type);
            }

        }
    }
}
