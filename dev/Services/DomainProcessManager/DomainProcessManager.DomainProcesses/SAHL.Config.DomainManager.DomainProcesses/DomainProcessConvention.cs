using System;
using System.Linq;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.Core.Extensions;
using SAHL.Core.DomainProcess;

namespace SAHL.Config.DomainManager.DomainProcesses
{
    public class DomainProcessConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var immediateInterfaces = type.GetImmediateInterfaces();
            var domainProcessInterface = immediateInterfaces.SingleOrDefault(x => x.Name.StartsWith("IDomainProcess`1"));
            if (domainProcessInterface == null) { return; }

            var dataModelType            = domainProcessInterface.GenericTypeArguments[0];
            var domainProcessGenericType = typeof(IDomainProcess<>);
            var genericType              = domainProcessGenericType.MakeGenericType(dataModelType);

            var eventType = domainProcessInterface.GenericTypeArguments[0];
            registry.For(genericType).Use(type).Named(eventType.Name);
        }
    }
}
