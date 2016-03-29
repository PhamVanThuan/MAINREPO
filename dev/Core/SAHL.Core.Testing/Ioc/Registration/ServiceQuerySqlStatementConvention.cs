using SAHL.Core.IoC;
using SAHL.Core.Services;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class ServiceQuerySqlStatementConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(IServiceQuerySqlStatement<,>).Name) != null && type.IsClass)
            {
                var defaultExplicitArguments = new DefaultExplicitArguments();
                var configuredInstance = registry.For(type).Use(type);
                defaultExplicitArguments.Configure(configuredInstance, type);
            }
        }
    }
}