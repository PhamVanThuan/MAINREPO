using SAHL.Core.IoC;
using SAHL.Core.Testing;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class LegacyEventQueryConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface("ILegacyEventGeneratorQuery") != null && type.IsClass)
            {
                registry.For(type).Use(type);
            }

        }
    }
}