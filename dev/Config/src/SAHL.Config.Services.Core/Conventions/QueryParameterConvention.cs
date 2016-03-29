using System;
using SAHL.Core.Services;
using StructureMap;
using StructureMap.Graph;

namespace SAHL.Config.Services.Core.Conventions
{
    public class QueryParameterConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (typeof(IQueryParameter).IsAssignableFrom(type))
            {
                if (!type.IsAbstract && !type.IsInterface)
                {
                    registry.For(type).LifecycleIs(InstanceScope.Hybrid).Use(type);
                }
            }
        }
    }
}