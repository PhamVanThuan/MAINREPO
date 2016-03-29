using SAHL.Core.IoC;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class LegacyEventQueryHandlerConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var serviceQueryInterface = type.GetInterface(typeof(IServiceQueryHandler<>).Name);
            if (type.IsClass && serviceQueryInterface != null && type.Name.EndsWith("LegacyEventQueryHandler"))
            {
                registry.For(type).Use(type);

            }

        }
    }
}