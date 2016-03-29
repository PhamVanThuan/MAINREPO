using SAHL.Core.Data;
using SAHL.Core.IoC;
using StructureMap.Graph;
using System;
using System.Linq;

namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloTileActionConfigTestConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloTileActionEdit").Any() && !type.IsAbstract && type.IsClass && !type.Name.Contains("Base"))
            {
                registry.For(type).Use(type);
            }
        }
    }
}