using System;
using System.Diagnostics;

using SAHL.Core;
using SAHL.Core.IoC;
using SAHL.Core.Services.Metrics;

namespace SAHL.Config.Services.Core
{
    public class ServiceManager : IServiceManager
    {
        private readonly IIocContainer container;
        private const string EventLog = "Application";
        private const string EventSource = "AppStartUpLogSource";

        public ServiceManager(IIocContainer container)
        {
            if (container == null) { throw new ArgumentNullException("container"); }
            this.container = container;
        }

        public void StartService()
        {
            this.MakeSureEventSourceExists();

            using (var eventLogger = new EventLog() { Source = EventSource, Log = EventLog })
            {
                this.StartStartables(eventLogger);
                this.StartStartableService(eventLogger);
            }
        }

        public void StopService()
        {
            this.MakeSureEventSourceExists();

            using (var eventLogger = new EventLog() { Source = EventSource, Log = EventLog })
            {
                this.StopStoppableService(eventLogger);
            }

            var stopables = this.container.GetAllInstances<IStoppable>();
            foreach (var stopable in stopables)
            {
                stopable.Stop();
            }
        }

        private void MakeSureEventSourceExists()
        {
            if (System.Diagnostics.EventLog.SourceExists(EventSource)) { return; }
            System.Diagnostics.EventLog.CreateEventSource(EventSource, EventLog);
        }

        private void StartStartables(EventLog eventLogger)
        {
            try
            {
                var startables = this.container.GetAllInstances<IStartable>();
                foreach (var startable in startables)
                {
                    startable.Start();
                }
            }
            catch (Exception ex)
            {
                eventLogger.WriteEntry(string.Format("Error while starting service startables\r\n {0}", ex.Message), EventLogEntryType.Error);
                throw;
            }
        }

        private void StartStartableService(EventLog eventLogger)
        {
            try
            {
                var startableService = this.container.GetInstance<IStartableService>();
                if (startableService == null) { return; }

                Console.WriteLine("Starting ... {0}", startableService);
                startableService.Start();

                var commandServiceRequestMetrics = this.container.GetInstance<ICommandServiceRequestMetrics>();
                if (commandServiceRequestMetrics != null)
                {
                    commandServiceRequestMetrics.SetServiceStartDateTime(startableService.GetType().Name, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                eventLogger.WriteEntry(string.Format("Error while starting service\r\n {0}", ex.Message), EventLogEntryType.Error);
                throw;
            }
        }

        private void StopStoppableService(EventLog eventLogger)
        {
            try
            {
                var stoppableService = this.container.GetInstance<IStoppableService>();
                if (stoppableService == null) { return; }

                Console.WriteLine("Stopping ... {0}", stoppableService);
                stoppableService.Stop();
            }
            catch (Exception ex)
            {
                eventLogger.WriteEntry(string.Format("Error while stopping service\r\n {0}", ex.Message), EventLogEntryType.Error);
                throw;
            }
        }
    }
}