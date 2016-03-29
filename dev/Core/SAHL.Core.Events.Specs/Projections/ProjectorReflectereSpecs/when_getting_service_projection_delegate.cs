using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Specs.Projections.ProjectorReflectereSpecs
{
    [Subject("SAHL.Core.Events.Projections.ProjectorReflectere.GetProjectionDelegate")]
    public class when_getting_service_projection_delegate : WithFakes
    {
        static IProjectorReflectere reflectere;
        static IIocContainer container;
        static Delegate result;
        static FakeServiceProjector fakeProjector;
        static FakeService service;
        static Type projectionType;
        static Type eventType;

        static FakeEvent innerFakeEvent;
        static WrappedEvent<FakeEvent> fakeEvent;

        static Type expectedActionType;

        Establish context = () =>
        {
            fakeProjector = new FakeServiceProjector();
            container = An<IIocContainer>();
            projectionType = typeof(IServiceProjector<FakeEvent, FakeService>);
            innerFakeEvent = new FakeEvent();
            fakeEvent = new WrappedEvent<FakeEvent>(Guid.NewGuid(), innerFakeEvent, An<IServiceRequestMetadata>());
            eventType = typeof(FakeEvent);

            service = new FakeService();

            expectedActionType = typeof(Action<WrappedEvent<FakeEvent>>);
            container.WhenToldTo(x => x.GetInstance<FakeService>()).Return(service);

            reflectere = new ProjectorReflectere(container);
        };

        Because of = () =>
        {
            result = reflectere.GetProjectionDelegate(fakeProjector, projectionType, eventType);
            result.DynamicInvoke(fakeEvent);
        };

        It should_return_expected_type_of_action = () =>
        {
            result.ShouldBeAssignableTo(expectedActionType);
        };

        It should_pass_handle_to_fake_service_projector = () =>
        {
            fakeProjector.Handled.ShouldBeTrue();
        };

        It should_send_expected_service_client = () =>
        {
            fakeProjector.Client.ShouldEqual(service);
        };

        It should_send_expected_event_to_handle = () =>
        {
            fakeProjector.Event.ShouldEqual(innerFakeEvent);
        };
    }
}