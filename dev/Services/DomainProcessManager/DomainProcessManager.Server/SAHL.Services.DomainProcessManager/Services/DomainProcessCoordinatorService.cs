using SAHL.Core;
using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.Shared;
using SAHL.Services.DomainProcessManager.Data;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SAHL.Services.DomainProcessManager.Services
{
    public class DomainProcessCoordinatorService : DomainProcessServiceBase, IDomainProcessCoordinatorService
    {
        private static readonly Object domainProcessLock = new object();

        private readonly IDomainProcessRepository domainProcessRepository;
        private readonly ConcurrentDictionary<Guid, IDomainProcess> domainProcessesCache;
        private readonly ConcurrentBag<Task> handleEventTasks;

        public DomainProcessCoordinatorService(IDomainProcessRepository domainProcessRepository, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (domainProcessRepository == null) { throw new ArgumentNullException("domainProcessRepository"); }

            this.domainProcessRepository = domainProcessRepository;
            this.domainProcessesCache = new ConcurrentDictionary<Guid, IDomainProcess>();
            this.handleEventTasks = new ConcurrentBag<Task>();
        }

        protected ConcurrentDictionary<Guid, IDomainProcess> DomainProcessesCache
        {
            get { return domainProcessesCache; }
        }

        public override void Start()
        {
            this.LogStartupMessage("Starting Coordinator Service");
        }

        public override void Stop()
        {
            this.LogMessage("Stopping Coordinator Service");
        }

        public void AddDomainProcess(IDomainProcess domainProcess)
        {
            if (domainProcess == null) { throw new ArgumentNullException("domainProcess"); }

            try
            {
                this.LogMessage(string.Format("Adding Domain Process - {0} - {1}", domainProcess.GetType(), domainProcess.DomainProcessId));

                lock (domainProcessLock)
                {
                    if (domainProcessesCache.ContainsKey(domainProcess.DomainProcessId))
                    {
                        return;
                    }
                    domainProcessesCache.TryAdd(domainProcess.DomainProcessId, domainProcess);
                }

                if (domainProcessRepository.Find<IDomainProcess>(domainProcess.DomainProcessId) == null)
                {
                    domainProcessRepository.Add(domainProcess);
                }

                domainProcess.PersistState += this.OnPersistDomainProcessState;
                domainProcess.Completed += this.OnDomainProcessCompleted;
                domainProcess.ErrorOccurred += this.OnDomainProcessErrorOccurred;
            }
            catch (Exception runtimeException)
            {
                this.LogMessage("Error Adding Domain Process", runtimeException);
            }
        }

        public IDomainProcess FindDomainProcess(Guid domainProcessId)
        {
            if (domainProcessId == Guid.Empty) { throw new ArgumentNullException("domainProcessId"); }

            lock (domainProcessLock)
            {
                IDomainProcess domainProcess = null;
                if (!domainProcessesCache.TryGetValue(domainProcessId, out domainProcess))
                {
                    domainProcess = domainProcessRepository.Find<IDomainProcess>(domainProcessId);
                    domainProcessesCache.TryAdd(domainProcessId, domainProcess);
                }
                return domainProcess;
            }
        }

        public void HandleEvent<T>(T domainProcessEvent) where T : class, IMessage
        {
            if (domainProcessEvent == null) { throw new ArgumentNullException("domainProcessEvent"); }
            this.LogMessage(string.Format("Busy processing event {0}.", domainProcessEvent.GetType()));

            var wrappedEvent = this.GetWrappedEvent(domainProcessEvent);
            var domainProcess = this.RetrieveDomainProcessToProcess<T>(wrappedEvent);
            domainProcess.HandleEvent(wrappedEvent.InternalEvent, wrappedEvent.ServiceRequestMetadata);

            this.domainProcessRepository.Update(domainProcess);
        }

        private IWrappedEvent<IEvent> GetWrappedEvent<T>(T domainProcessEvent) where T : class, IMessage
        {
            var wrappedEvent = domainProcessEvent as IWrappedEvent<IEvent>;
            if (wrappedEvent == null)
            {
                throw new ArgumentException(string.Format("Wrong event type receieved. Expected IWrappedEvent<IEvent> but received {0}", domainProcessEvent));
            }

            if ((wrappedEvent.ServiceRequestMetadata == null) || !wrappedEvent.ServiceRequestMetadata.ContainsKey(CoreGlobals.DomainProcessIdName))
            {
                throw new ArgumentException("No Domain Process ID received with Event");
            }
            return wrappedEvent;
        }

        private IDomainProcess RetrieveDomainProcessToProcess<T>(IWrappedEvent<IEvent> wrappedEvent) where T : class, IMessage
        {
            var domainProcessId = Guid.Parse(wrappedEvent.ServiceRequestMetadata[CoreGlobals.DomainProcessIdName]);
            var domainProcess = this.FindDomainProcess(domainProcessId);
            if (domainProcess == null) { throw new Exception("Domain Process not found"); }

            return domainProcess;
        }

        private void OnPersistDomainProcessState(object sender, EventArgs eventArgs)
        {
            var domainProcess = (IDomainProcess)sender;
            if (domainProcess == null) { return; }

            this.domainProcessRepository.Update(domainProcess);
        }

        private void OnDomainProcessCompleted(Guid domainProcessId)
        {
            IDomainProcess domainProcess = null;

            lock (domainProcessLock)
            {
                if (!domainProcessesCache.TryRemove(domainProcessId, out domainProcess)) { return; }
            }
            domainProcessRepository.Update(domainProcess);
        }

        private void OnDomainProcessErrorOccurred(Guid domainProcessId)
        {
            IDomainProcess domainProcess = null;

            lock (domainProcessLock)
            {
                if (!domainProcessesCache.TryRemove(domainProcessId, out domainProcess)) { return; }
            }
            domainProcessRepository.Update(domainProcess);
        }
    }
}