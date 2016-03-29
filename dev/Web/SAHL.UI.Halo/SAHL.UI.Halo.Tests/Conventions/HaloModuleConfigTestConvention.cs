using SAHL.Core.IoC;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloModuleConfigTestConvention :  IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloModuleConfiguration").Any() && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}
