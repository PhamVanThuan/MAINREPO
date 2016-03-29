using SAHL.Core.IoC;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileModelConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloTileModel").Any() && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}