using SAHL.Core.IoC;
using SAHL.Core.Services;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class ServiceCommandConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(IServiceCommand).Name) != null && type.IsClass && type.GetInterface(typeof(IServiceQuery).Name) == null)
            {
                registry.For(type).Use(type);
            }
        }
    }
}
