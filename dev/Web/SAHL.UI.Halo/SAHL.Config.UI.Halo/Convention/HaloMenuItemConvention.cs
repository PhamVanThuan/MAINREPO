using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo
{
    public class HaloMenuItemConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IHaloMenuItem)) && !type.IsAbstract)
            {
                registry.For(typeof(IHaloMenuItem)).Singleton().Use(type);
            }
        }
    }
}
