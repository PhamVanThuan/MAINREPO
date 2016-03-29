using SAHL.Core.IoC;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloDynamicRootTileConfigTestConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var interfaces = type.GetInterfaces().Where(x => x.Name == "IHaloDynamicRootTileConfiguration");
            if (!type.Name.Contains("Base") && interfaces.Count() > 0 && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}
