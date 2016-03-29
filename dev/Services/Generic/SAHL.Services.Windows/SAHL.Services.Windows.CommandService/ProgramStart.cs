using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Topshelf;
using SAHL.Config.Services.Core;

using SAHL.Core;
using SAHL.Config.Services;
using SAHL.Config.Services.Windows;

[assembly: InternalsVisibleTo("SAHL.Services.Windows.CommandService.Tests")]

namespace SAHL.Services.Windows.CommandService
{
    internal class ProgramStart
    {
        private const string EventLog    = "Application";
        private const string EventSource = "AppStartUpLogSource";

        private static IServiceBootstrapper serviceBootstrapper;
        private static IIocContainer iocContainer;
        private static IWindowsServiceSettings windowsServiceSettings;

        public ProgramStart(IServiceBootstrapper serviceBootstrapper)
        {
            if (serviceBootstrapper == null) { throw new ArgumentNullException("serviceBootstrapper"); }
            ProgramStart.serviceBootstrapper = serviceBootstrapper;
        }

        private static bool EnableFirstChanceException
        {
            get { return windowsServiceSettings.EnableFirstChanceException; }
        }

        private static bool EnableUnhandledException
        {
            get { return windowsServiceSettings.EnableUnhandledException; }
        }

        public static void Main(string[] args)
        {
            ProgramStart.InitialiseService();

            using (var eventLog = new EventLog() { Source = EventSource, Log = EventLog })
            {
                var windowsServiceManager = GetServiceManager();

                eventLog.WriteEntry(string.Format("Starting Windows Service Application ({0})...", windowsServiceSettings.ServiceName), EventLogEntryType.Information);

                HostFactory.Run(hostConfigurator =>
                    {
                        hostConfigurator.Service<IServiceManager>(serviceConfigurator =>
                            {
                                serviceConfigurator.ConstructUsing(() => windowsServiceManager);
                                serviceConfigurator.WhenStarted(serviceManager => serviceManager.StartService());
                                serviceConfigurator.WhenStopped(serviceManager => serviceManager.StopService());
                            });

                        hostConfigurator.RunAsLocalSystem();

                        hostConfigurator.SetDisplayName(windowsServiceSettings.DisplayName);
                        hostConfigurator.SetDescription(windowsServiceSettings.Description);
                        hostConfigurator.SetServiceName(windowsServiceSettings.ServiceName);
                    });

                eventLog.WriteEntry(string.Format("Starting Windows Service Application ({0})...Done", windowsServiceSettings.ServiceName), EventLogEntryType.Information);
            }
        }

        private static void InitialiseService()
        {
            if (!System.Diagnostics.EventLog.SourceExists(EventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(EventSource, EventLog);
            }

            if (serviceBootstrapper == null)
            {
                serviceBootstrapper = new ServiceBootstrapper();
            }

            iocContainer           = serviceBootstrapper.Initialise();
            windowsServiceSettings = ProgramStart.GetServiceSettings();

            if (ProgramStart.EnableUnhandledException)
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            }

            if (ProgramStart.EnableFirstChanceException)
            {
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomainFirstChanceException;
            }
        }

        private static IWindowsServiceSettings GetServiceSettings()
        {
            var serviceSettings = iocContainer.GetInstance<IWindowsServiceSettings>();
            if (serviceSettings == null)
            {
                throw new NullReferenceException("Windows Service Settings not found");
            }

            return serviceSettings;
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

        private static void CurrentDomainFirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            var source = string.Format("{0} - {1}", "SAHL.Services.Windows.CommandService", "FirstChanceException");

            var eventInfo = new StringBuilder();
            eventInfo.AppendLine(e.Exception.ToString());
            eventInfo.AppendLine(Environment.NewLine);
            eventInfo.AppendLine(new StackTrace(true).ToString());

            WriteEventLogEntry(source, eventInfo.ToString());
        }

        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var source = string.Format("{0} - {1}", "SAHL.Services.Windows.CommandService", "UnhandledException");
            var eventInfo = new StringBuilder();
            eventInfo.AppendLine(e.ExceptionObject.ToString());
            eventInfo.AppendLine(Environment.NewLine);
            eventInfo.AppendLine(new StackTrace(true).ToString());

            WriteEventLogEntry(source, eventInfo.ToString());
        }

        private static void WriteEventLogEntry(string source, string eventInfo)
        {
            try 
            { 
                if (!System.Diagnostics.EventLog.SourceExists(source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(source, "Application");
                }

                System.Diagnostics.EventLog.WriteEntry(source, eventInfo, EventLogEntryType.Error);
            }
            catch (Exception)
            {
                // Do nothing .. Swallow exception silently
            }
        }
    }
}
