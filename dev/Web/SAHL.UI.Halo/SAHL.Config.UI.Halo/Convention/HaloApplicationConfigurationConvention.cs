using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloApplicationConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IHaloApplicationConfiguration)) && !type.IsAbstract)
            {
                registry.For(typeof(IHaloApplicationConfiguration)).Singleton().Use(type);
            }
        }
    }
}
