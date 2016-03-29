using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.Extensions;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.DomainProcessManager.Services
{
    public class DomainProcessEventHandlerService : DomainProcessServiceBase, IDomainProcessEventHandlerService
    {
        private readonly IDomainProcessCoordinatorService domainProcessCoordinatorService;
        private readonly IMessageBusAdvanced messageBus;
        private readonly IEnumerable<IDomainProcess> domainProcesses;

        private readonly Type openWrappedEventType = typeof(WrappedEvent<>);
        private readonly MethodInfo openHandleMethod = typeof(DomainProcessEventHandlerService).GetMethod("Handle");
        private readonly MethodInfo openSubscribeMethod = typeof(IMessageBusAdvanced).GetMethod("Subscribe", new[] { typeof(string), typeof(Action<IMessage>) });

        public DomainProcessEventHandlerService(IDomainProcessCoordinatorService domainProcessCoordinatorService,
                                                IMessageBusAdvanced messageBus, IEnumerable<IDomainProcess> domainProcesses,
                                                IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (domainProcessCoordinatorService == null) { throw new ArgumentNullException("domainProcessCoordinatorService"); }
            if (messageBus == null) { throw new ArgumentNullException("messageBus"); }

            this.domainProcessCoordinatorService = domainProcessCoordinatorService;
            this.messageBus = messageBus;
            this.domainProcesses = domainProcesses;
        }

        public override void Start()
        {
            this.LogStartupMessage("Starting Event Handler Service");

            try
            {
                foreach (var currentDomainProcess in domainProcesses)
                {
                    var processInterfaces = currentDomainProcess.GetType().GetImmediateInterfaces().Where(type => type.Name.StartsWith("IDomainProcessEvent"));

                    foreach (var currentProcessInterface in processInterfaces)
                    {
                        var eventType = currentProcessInterface.GetGenericArguments().First();
                        this.SubscribeToEvent(eventType);
                    }
                }
            }
            catch (Exception runtimeException)
            {
                this.LogStartupMessage("Error starting Event Handler Service", runtimeException);
            }
        }

        public override void Stop()
        {
            this.LogMessage("Stopping Event Handler Service");
        }

        public void Handle<T>(T @event) where T : class, IMessage
        {
            try
            {
                Console.WriteLine(@event.GetType().FullName);
                domainProcessCoordinatorService.HandleEvent(@event);
            }
            catch (Exception runtimeException)
            {
                this.LogMessage(String.Format("Failed to handle event with Id {0}", @event.Id),
                    runtimeException, String.Format("HandleEvent<{0}>", @event.GetType()));
            }
        }

        private void SubscribeToEvent(Type eventType)
        {
            Type closedEventType = openWrappedEventType.MakeGenericType(eventType);

            MethodInfo closeHandleMethod = openHandleMethod.MakeGenericMethod(closedEventType);
            MethodInfo closedSubscribeMethod = openSubscribeMethod.MakeGenericMethod(closedEventType);

            Delegate handleDelegate = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(closedEventType), this, closeHandleMethod);
            closedSubscribeMethod.Invoke(messageBus, new object[] { "DomainProcessEventHandlerService", handleDelegate });
        }
    }
}