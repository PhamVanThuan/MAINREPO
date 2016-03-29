using SAHL.Config.Services;
using SAHL.Config.Web.Mvc;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Core;
using SAHL.Core.Web.Mvc.Ioc;
using StructureMap;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Mvc;
using System.Web.Optimization;
using SAHL.Config.Services.Core;

namespace SAHL.Services.Web.CommandService
{
    public class WebApiApplication : HttpApplication
    {
        private const string EventLog = "Application";
        private const string EventSource = "AppStartUpLogSource";

        protected void Application_Start()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();
            var serviceManager = container.GetInstance<IServiceManager>();
            var serviceSettings = container.GetInstance<IServiceSettings>();

            if (!System.Diagnostics.EventLog.SourceExists(EventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(EventSource, EventLog);
            }

            using (EventLog _logger = new EventLog() { Source = EventSource, Log = EventLog })
            {
                try
                {
                    _logger.WriteEntry(string.Format("Starting Web Application ({0})...", serviceSettings.ServiceName), EventLogEntryType.Information);

                    RegisterConfiguration(container);

                    serviceManager.StartService();
                }
                catch (Exception ex)
                {
                    _logger.WriteEntry(string.Format("Error starting web application\r\n {0}", ex.Message), EventLogEntryType.Error);
                    throw;
                }
            }
        }

        private void RegisterConfiguration(IIocContainer container)
        {
            RegisterCommonServiceConfiguration(container);

            var serviceCorsSettings = container.GetInstance<IServiceCORSSettings>();

            RegisterDefaultServiceConfiguration(serviceCorsSettings);

            var customConfiguration = container.GetInstance<CustomHttpConfiguration>();
            if (customConfiguration == null)
            {
                //register standard handlers
                var routeRetriever = container.GetInstance<IRouteRetriever>();
                var registration = new RouteRegistration(new ApiRouteConfig(routeRetriever), new MvcRouteConfig(routeRetriever), null);
                registration.Register();
            }
            else
            {
                RegisterCustomServiceConfiguration(customConfiguration);
            }
        }

        private void RegisterCommonServiceConfiguration(IIocContainer container)
        {
            SetDependencyResolver(container);

            GlobalConfiguration.Configuration.Services.Clear(typeof(IBodyModelValidator));
        }

        private void SetDependencyResolver(IIocContainer container)
        {
            var dependencyResolver = new StructureMapDependencyResolver(container.GetInstance<IContainer>());
            DependencyResolver.SetResolver(dependencyResolver);
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }

        private void RegisterCustomServiceConfiguration(CustomHttpConfiguration customConfiguration)
        {
            customConfiguration.PerformRegistrations();
        }

        private void RegisterDefaultServiceConfiguration(IServiceCORSSettings serviceCorsSettings)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration, serviceCorsSettings);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}