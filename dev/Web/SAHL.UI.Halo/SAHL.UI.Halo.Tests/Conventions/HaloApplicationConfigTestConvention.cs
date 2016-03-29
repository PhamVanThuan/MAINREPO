using SAHL.Core.Testing.Ioc.Registration;
using StructureMap.Graph;
using System;
using System.Linq;
namespace SAHL.UI.Halo.Tests.Conventions
{
    public class HaloApplicationConfigTestConvention :  IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.IsClass && type.GetInterfaces().Where(x => x.IsInterface && x.Name == "IHaloApplicationConfiguration").Any() && !type.IsAbstract && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}
