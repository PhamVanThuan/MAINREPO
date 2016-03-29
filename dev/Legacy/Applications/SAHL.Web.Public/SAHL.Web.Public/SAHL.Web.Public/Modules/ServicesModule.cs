using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using SAHL.Web.Public.AttorneyService;
using Ninject;
using SAHL.Common.Logging;
using SAHL.Communication;

namespace SAHL.Web.Public.Modules
{
    /// <summary>
    /// Ninject Module
    /// </summary>
    public class ServicesModule : NinjectModule
    {
        /// <summary>
        /// Load
        /// </summary>
        public override void Load()
        {
			Bind<IMessageBus>().To<EasyNetQMessageBus>();

			Bind<ILogger>().To<MessageBusLogger>();
			Bind<IMessageBusConfigurationProvider>().To<EasyNetQMessageBusConfigurationProvider>();
			Bind<MessageBusLoggerConfiguration>().ToSelf();
			SAHL.Common.Logging.LogPlugin.Logger = Kernel.Get<ILogger>();

			Bind<IMetrics>().To<MessageBusMetrics>();
			Bind<MessageBusMetricsConfiguration>().ToSelf();
			MetricsPlugin.Metrics = Kernel.Get<MessageBusMetrics>();

            Bind<IAttorney>().To<AttorneyClient>().InRequestScope();
        }
    }
}