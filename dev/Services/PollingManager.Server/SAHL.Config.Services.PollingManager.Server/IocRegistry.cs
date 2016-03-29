using Microsoft.Exchange.WebServices.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager;
using SAHL.Services.PollingManager.Credentials;
using SAHL.Services.PollingManager.Handlers;
using SAHL.Services.PollingManager.Settings;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Config.Services.ExchangeManager.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.LookForRegistries();
            });

            For<IPolledHandlerSettings>().Use<LossControlPolledHandlerSettings>()
                .Ctor<NameValueCollection>("nameValueCollection")
                .Is(ConfigurationManager.AppSettings);

            For<IExchangeProviderCredentials>().Use<LossControlExchangeProviderCredentials>()
               .Ctor<NameValueCollection>("nameValueCollection")
               .Is(ConfigurationManager.AppSettings);

            For<IExchangeProvider>()
                    .Use<ExchangeProvider>()
                    .Ctor<ExchangeService>().Is(new ExchangeService());

            For<IPolledHandler>()
               .Use<LossControlPolledExchangeHandler>()
               .Ctor<IPolledHandlerSettings>().Is<LossControlPolledHandlerSettings>();

            For<IPollingManagerService>().Singleton().Use<PollingManagerService>().Named("PollingManagerService");
            For<IStartable>().Use(x => x.GetInstance<PollingManagerService>("PollingManagerService"));
            For<IStoppable>().Use(x => x.GetInstance<PollingManagerService>("PollingManagerService"));

            this.For<IStartableService>().Use<HostedService>();
        }
    }
}