using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloModuleConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IHaloModuleConfiguration)) && !type.IsAbstract)
            {
                registry.For(typeof(IHaloModuleConfiguration)).Singleton().Use(type);
            }
        }
    }
}
