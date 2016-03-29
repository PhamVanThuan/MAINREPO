using SAHL.Core.IoC;
using SAHL.Core.Services;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class ServiceCommandHandlerConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var serviceCommandInterface = type.GetInterface(typeof(IServiceCommandHandler<>).Name);
            if (type.IsClass && serviceCommandInterface != null)
            {
                registry.For(type).Use(type);
            }
        }
    }
}