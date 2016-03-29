using SAHL.Config.Services;
using SAHL.Config.Services.Core;
using SAHL.Config.Services.Windows.Web;
using SAHL.Core;
using StructureMap;
using System;
using System.Diagnostics;
using Topshelf;

namespace SAHL.Services.WindowsWeb.CommandService
{
    public class ProgramStart
    {
        private const string EventLog = "Application";
        private const string EventSource = "AppStartUpLogSource";

        private static IServiceBootstrapper serviceBootstrapper;
        private static IIocContainer iocContainer;

        public ProgramStart(IServiceBootstrapper serviceBootstrapper)
        {
            if (serviceBootstrapper == null) { throw new ArgumentNullException("serviceBootstrapper"); }
            ProgramStart.serviceBootstrapper = serviceBootstrapper;
        }

        public static void Main(string[] args)
        {
            if (serviceBootstrapper == null)
            {
                serviceBootstrapper = new ServiceBootstrapper();
            }

            if (!System.Diagnostics.EventLog.SourceExists(EventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(EventSource, EventLog);
            }

            using (var _logger = new EventLog() { Source = EventSource, Log = EventLog })
            {
                try
                {
                    iocContainer = serviceBootstrapper.Initialise();

                    var serviceSettings = GetServiceSettings();
                    var serviceManager = GetServiceManager();

                    _logger.WriteEntry(string.Format("Starting Windows Service Application ({0})...", serviceSettings.ServiceName), EventLogEntryType.Information);

                    HostFactory.Run(hostConfigurator =>
                        {
                            hostConfigurator.Service<IServiceManager>(serviceConfigurator =>
                                {
                                    serviceConfigurator.ConstructUsing(() => serviceManager);
                                    serviceConfigurator.WhenStarted(manager => manager.StartService());
                                    serviceConfigurator.WhenStopped(manager => manager.StopService());
                                });

                            hostConfigurator.RunAsLocalSystem();

                            hostConfigurator.SetDisplayName(serviceSettings.DisplayName);
                            hostConfigurator.SetDescription(serviceSettings.Description);
                            hostConfigurator.SetServiceName(serviceSettings.ServiceName);
                        });

                    _logger.WriteEntry(string.Format("Starting Windows Service Application ({0})...Done", serviceSettings.ServiceName), EventLogEntryType.Information);
                }
                catch (Exception runtimeException)
                {
                    _logger.WriteEntry(string.Format("Runtime exception occurred\n{0}", runtimeException), EventLogEntryType.Error);
                }
            }
        }

        private static IServiceManager GetServiceManager()
        {
            var serviceManager = iocContainer.GetInstance<IServiceManager>();
            if (serviceManager == null)
            {
                throw new NullReferenceException("Windows Service Manager not found");
            }

            return serviceManager;
        }

        private static IWebSelfHostedServiceSettings GetServiceSettings()
        {
            var serviceSettings = iocContainer.GetInstance<IWebSelfHostedServiceSettings>();
            if (serviceSettings == null)
            {
                throw new NullReferenceException("Windows Service Settings not found");
            }

            return serviceSettings;
        }
    }
}