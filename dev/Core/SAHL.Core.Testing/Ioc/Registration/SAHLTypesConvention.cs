using SAHL.Core.IoC;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class SAHLTypesConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.Namespace.StartsWith("SAHL.") && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}