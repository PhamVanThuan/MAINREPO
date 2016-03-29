using SAHL.Core.Data;
using SAHL.Core.IoC;
using StructureMap.Graph;
using System;
using System.Linq;

namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloRootTileConfigTestConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.IsClass && !type.IsAbstract &&
                   type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloRootTileConfiguration").Any() && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}