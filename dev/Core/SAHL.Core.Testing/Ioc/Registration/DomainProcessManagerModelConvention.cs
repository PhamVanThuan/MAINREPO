using SAHL.Core.Data;
using SAHL.Core.IoC;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class DomainProcessManagerModelConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(IDataModel).Name) != null && type.IsClass && type.Namespace.Equals("SAHL.DomainProcessManager.Models"))
            {
                registry.For(type).Use(type);
            }
        }
    }
}