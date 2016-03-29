using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloAlternativeRootTileConfigTestConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.IsClass && !type.IsAbstract &&
                   type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloAlternativeRootTileConfiguration").Any() && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}