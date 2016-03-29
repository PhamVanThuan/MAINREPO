using SAHL.Core.IoC;
using SAHL.Core.Services;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class InternalServiceCommandHandlerForDomainConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var serviceCommandInterface = type.GetInterface(typeof(IServiceCommandHandler<>).Name);
            var domainServiceCommandInterface = type.GetInterface(typeof(IDomainServiceCommandHandler<,>).Name);
            var isInDomainService = type.Namespace.EndsWith("Domain");
            if (type.IsClass && serviceCommandInterface != null && domainServiceCommandInterface == null && isInDomainService)
            {
                registry.For(type).Use(type);
            }
        }
    }
}