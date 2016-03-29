using Microsoft.Exchange.WebServices.Data;
using SAHL.Config.Services;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager.Credentials;
using SAHL.Services.PollingManager.Handlers;
using SAHL.Services.PollingManager.Settings;
using StructureMap;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Services.PollingManager.Server.Console
{
    public class IoC
    {
        public static IContainer Initialize()
        {
            var bootstrappper = new ServiceBootstrapper();
            bootstrappper.Initialise();

            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });

                x.For<IPolledHandlerSettings>().Use<LossControlPolledHandlerSettings>()
                    .Ctor<NameValueCollection>("nameValueCollection")
                    .Is(ConfigurationManager.AppSettings);

                x.For<IExchangeProviderCredentials>().Use<LossControlExchangeProviderCredentials>()
                   .Ctor<NameValueCollection>("nameValueCollection")
                   .Is(ConfigurationManager.AppSettings);

                x.For<IExchangeProvider>()
                    .Use<ExchangeProvider>()
                    .Ctor<ExchangeService>().Is(new ExchangeService());

                x.For<IExchangeMailboxHelper>().Add<ExchangeMailboxHelper>()
                 .Ctor<int>("messageCount")
                 .Is(c => c.GetInstance<LossControlPolledHandlerSettings>().ProcessingSetSize);

                x.For<IPolledHandler>()
                 .Use<LossControlPolledExchangeHandler>();

                x.For<ILoggerSource>().Singleton().Use(new LoggerSource("Polling Manager", LogLevel.Error, false));

                x.For<IRawLogger>().Use<NullRawLogger>();
                x.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                x.For<ILogger>().Use<Logger>();
                x.For<ILoggerSource>().Singleton().Use(new LoggerSource("Polling Manager", LogLevel.Error, false));

                x.For<IPollingManagerService>().Singleton().Use<PollingManagerService>().Named("PollingManagerService");
                x.For<IStartable>().Use(y => y.GetInstance<PollingManagerService>("PollingManagerService"));
                x.For<IStoppable>().Use(y => y.GetInstance<PollingManagerService>("PollingManagerService"));

                x.For<IStartableService>().Use<HostedService>();
            });

            return ObjectFactory.Container;
        }
    }
}