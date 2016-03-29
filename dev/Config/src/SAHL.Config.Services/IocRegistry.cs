using SAHL.Config.Services.Core;
using SAHL.Config.Services.Core.Conventions;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Config.Services
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                scan.WithDefaultConventions();

                scan.Convention<CommandHandlerDecoratorConvention>();
                scan.Convention<QueryHandlerDecoratorConvention>();
                scan.Convention<QueryParameterConvention>();
                scan.Convention<DomainCommandCheckHandlerConvention>();

                scan.ConnectImplementationsToTypesClosing(typeof(IServiceQuerySqlStatement<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IServiceQueryRule<>));
                scan.ConnectImplementationsToTypesClosing(typeof(IServiceCommandRule<>));
                scan.ConnectImplementationsToTypesClosing(typeof(IDomainRule<>));

                scan.LookForRegistries();
            });

            For<IHostedService>().Singleton().Use<HostedService>();

            For<NameValueCollection>().Use(ConfigurationManager.AppSettings);

            // command persistence
            For<SAHL.Core.Services.CommandPersistence.ICommandRetryPolicy>().Use<SAHL.Core.Services.CommandPersistence.CommandRetryPolicy.CommandRetryPolicyThree>();

            // logging
            For<ILoggerAppSource>().Singleton().Use<LoggerAppSourceFromServiceName>();

            // command handlers
            For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

            // query handlers
            For<IServiceQueryHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

            // metrics
            For<ICommandServiceRequestMetrics>().Use<CommandServiceRequestMetrics>()
                .Ctor<string>().Is(ctx =>
                {
                    var startableService = ctx.TryGetInstance<SAHL.Core.IoC.IStartableService>();
                    string startableServiceName = string.Empty;
                    if (startableService != null)
                    {
                        startableServiceName = startableService.GetType().Name;
                    }
                    return startableServiceName;
                });
        }
    }
}