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
    public class when_no_projections_are_available : WithFakes
    {
        private static IEventProjectionService eventProjectionService;
        private static IMessageBusAdvanced messageBus;
        private static IEnumerable<IEventProjector> projections;
        private static IProjectorReflectere projectionReflector;

        private Establish context = () =>
        {
            projections = new IEventProjector[0];

            messageBus = An<IMessageBusAdvanced>();
            projectionReflector = An<IProjectorReflectere>();

            eventProjectionService = new EventProjectionService(messageBus, projections, projectionReflector);
        };

        private Because of = () =>
        {
            eventProjectionService.Start();
        };

        private It should_not_get_interfaces_for_projector_instance = () =>
        {
            projectionReflector.WasNotToldTo(x => x.GetInterfaces(Param<IEventProjector>.IsAnything));
        };

        private It should_not_get_the_event_type_for_projector = () =>
        {
            projectionReflector.WasNotToldTo(x => x.GetEventType(Param<Type>.IsAnything));
        };

        private It should_not_get_delegate_for_projection = () =>
        {
            projectionReflector.WasNotToldTo(x => x.GetProjectionDelegate(Param<IEventProjector>.IsAnything, Param<Type>.IsAnything, Param<Type>.IsAnything));
        };

        private It should_not_subscribe = () =>
        {
            messageBus.WasNotToldTo(x => x.Subscribe(Param<string>.IsAnything, Param<Action<IEvent>>.IsAnything));
        };
    }
}