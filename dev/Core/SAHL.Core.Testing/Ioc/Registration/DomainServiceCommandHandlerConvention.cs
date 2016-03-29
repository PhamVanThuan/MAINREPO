using SAHL.Core.IoC;
using SAHL.Core.Services;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class DomainServiceCommandHandlerConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            var implementsDomainServiceInterface
                = type.GetInterface(typeof(IDomainServiceCommandHandler).Name) != null;
            if (type.IsClass && implementsDomainServiceInterface)
            {
                registry.For(type).Use(type);
            }
        }
    }
}