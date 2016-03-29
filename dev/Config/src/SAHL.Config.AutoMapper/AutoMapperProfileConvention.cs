using System;
using System.Linq;

using SAHL.Core;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.AutoMapper
{
    public class AutoMapperProfileConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IAutoMapperProfile)) && !type.IsAbstract)
            {
                registry.For(typeof(IAutoMapperProfile)).Use(type);
            }
        }
    }
}
