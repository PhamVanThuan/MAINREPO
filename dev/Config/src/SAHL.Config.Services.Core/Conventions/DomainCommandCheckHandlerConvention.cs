using System;
using System.Linq;
using SAHL.Core.Services;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SAHL.Config.Services.Core.Conventions
{
    public class DomainCommandCheckHandlerConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var checkHandlers = type.GetInterfaces().Where(x => x.Name.StartsWith("IDomainCommandCheckHandler"));
            foreach (var currentCheckHandler in checkHandlers)
            {
                var domainCommandCheckType = currentCheckHandler.GetGenericArguments().FirstOrDefault();
                var domainCommandCheckHandlerType = typeof(IDomainCommandCheckHandler<>);
                var genericType = domainCommandCheckHandlerType.MakeGenericType(domainCommandCheckType);

                registry.For(genericType).Use(type).Named(domainCommandCheckType.Name);
            }
        }
    }
}