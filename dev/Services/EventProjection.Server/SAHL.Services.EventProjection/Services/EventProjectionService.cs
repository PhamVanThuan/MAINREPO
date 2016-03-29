using SAHL.Core.Events;
using SAHL.Core.Events.Projections;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.Shared;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Services.EventProjection.Services
{
    public interface IEventProjectionService : IHostedService
    {
    }

    public class EventProjectionService : HostedService, IEventProjectionService
    {
        private IEnumerable<IEventProjector> projectors;
        private IMessageBusAdvanced messageBus;
        private IProjectorReflectere projectorReflectere;

        public EventProjectionService(IMessageBusAdvanced messageBus, IEnumerable<IEventProjector> projectors, IProjectorReflectere projectorReflectere)
        {
            this.projectors = projectors;
            this.messageBus = messageBus;
            this.projectorReflectere = projectorReflectere;
        }

        public override void Start()
        {
            base.Start();
            var openSubscribeMethod = typeof(IMessageBusAdvanced).GetMethod("Subscribe", new[] { typeof(string), typeof(Action<IMessage>) });
            foreach (IEventProjector projectorInstance in projectors)
            {
                IEnumerable<Type> projectorInterfaces = projectorReflectere.GetInterfaces(projectorInstance);
                foreach (Type projectorInterface in projectorInterfaces)
                {
                    Type eventType = projectorReflectere.GetEventType(projectorInterface);
                    MethodInfo closedGenericSubscribeMethod = openSubscribeMethod.MakeGenericMethod(typeof(WrappedEvent<>).MakeGenericType(eventType));
                    Delegate handle = projectorReflectere.GetProjectionDelegate(projectorInstance, projectorInterface, eventType);
                    if (handle != null)
                    {
                        closedGenericSubscribeMethod.Invoke(messageBus, new object[] { projectorInstance.GetType().Name, handle });
                    }
                }
            }
        }
    }
}