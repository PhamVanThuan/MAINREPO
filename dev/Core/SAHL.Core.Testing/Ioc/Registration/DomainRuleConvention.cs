using SAHL.Core.IoC;
using SAHL.Core.Rules;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class DomainRuleConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(IDomainRule<>).Name) != null && type.IsClass && !type.IsAbstract)
            {
                registry.For(type).Use(type);
            }
        }
    }
}