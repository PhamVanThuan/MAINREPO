using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Events.Projections;
using SAHL.Core.Messaging;
using SAHL.Services.EventProjection.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.ProjectionServiceSpecs
{
    [Subject("SAHL.Services.EventProjection.Services.EventProjectionService.Start")]
    public class when_event_projection_service_starts : WithFakes
    {
        private static IEventProjectionService eventProjectionService;
        private static IMessageBusAdvanced messageBus;
        private static IEnumerable<IEventProjector> projections;
        private static IProjectorReflectere projectionReflector;
        private static IEventProjector fakeProjector;
        private static Action<WrappedEvent<IEvent>> fakeAction;
        private static Type fakeType;
        private static Type fakeEvent;

        private Establish context = () =>
        {
            fakeProjector = An<IEventProjector>();

            fakeType = typeof(IEventProjector);
            fakeEvent = typeof(IEvent);
            fakeAction = new Action<WrappedEvent<IEvent>>((t) => { });

            projections = new IEventProjector[1]{
                fakeProjector
            };
            messageBus = An<IMessageBusAdvanced>();
            projectionReflector = An<IProjectorReflectere>();

            projectionReflector.WhenToldTo(x => x.GetInterfaces(fakeProjector)).Return(new Type[]{
                fakeType
            });

            projectionReflector.WhenToldTo(x => x.GetEventType(fakeType)).Return(fakeEvent);

            projectionReflector.WhenToldTo(x => x.GetProjectionDelegate(fakeProjector, fakeType, fakeEvent)).Return(fakeAction);

            eventProjectionService = new EventProjectionService(messageBus, projections, projectionReflector);
        };

        private Because of = () =>
        {
            eventProjectionService.Start();
        };

        private It should_get_interfaces_for_projector_instance = () =>
        {
            projectionReflector.WasToldTo(x => x.GetInterfaces(Param<IEventProjector>.IsAnything));
        };

        private It should_get_the_event_type_for_projector = () =>
        {
            projectionReflector.WasToldTo(x => x.GetEventType(Param<Type>.IsAnything));
        };

        private It should_get_delegate_for_projection = () =>
        {
            projectionReflector.WasToldTo(x => x.GetProjectionDelegate(fakeProjector, fakeType, fakeEvent));
        };

        private It should_subscribe = () =>
        {
            messageBus.WasToldTo(x => x.Subscribe(fakeProjector.GetType().Name, fakeAction));
        };
    }
}