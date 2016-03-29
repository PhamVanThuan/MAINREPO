using System;
using SAHL.Core.Events;
using StructureMap.Graph;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class EventConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(IEvent).Name) != null && type.IsClass)
            {
                registry.For(type).Use(type);
            }
        }
    }
}