using SAHL.Core.Data;
using SAHL.Core.IoC;
using SAHL.Core.Services;
using SAHL.Core.Testing.Providers;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class SqlStatementConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface(typeof(ISqlStatement<>).Name) != null
                && type.IsClass
                && !type.IsAbstract
                && !type.Namespace.Contains("_template")
                && type.GetInterface(typeof(IServiceQuerySqlStatement<,>).Name) == null)
            {
                var defaultExplicitArguments = new DefaultExplicitArguments();
                var configuredInstance = registry.For(type).Use(type);
                defaultExplicitArguments.Configure(configuredInstance,type);
            }
        }
    }
}