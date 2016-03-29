using SAHL.Core.IoC;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class DomainDataManagerConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
                if (type.Name.EndsWith("DataManager") && type.IsClass && type.Assembly.GetName().Name.Contains("DomainService"))
                {
                    registry.For(type).Use(type);
                }
        }
    }
}