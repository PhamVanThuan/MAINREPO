using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Specs.Projections.ProjectorReflectereSpecs
{
    [Subject("SAHL.Core.Events.Projections.ProjectorReflectere.GetProjectionDelegate")]
    public class when_getting_table_projection_delegate : WithFakes
    {
        static IProjectorReflectere reflectere;
        static IIocContainer container;
        static Delegate result;
        static FakeTableProjector fakeProjector;
        static Type projectionType;
        static Type eventType;

        static FakeEvent innerFakeEvent;
        static WrappedEvent<FakeEvent> fakeEvent;

        static Type expectedActionType;

        Establish context = () =>
        {
            fakeProjector = new FakeTableProjector();
            container = An<IIocContainer>();
            projectionType = typeof(ITableProjector<FakeEvent, IDataModel>);
            innerFakeEvent = new FakeEvent();
            fakeEvent = new WrappedEvent<FakeEvent>(Guid.NewGuid(), innerFakeEvent, An<IServiceRequestMetadata>());
            eventType = typeof(FakeEvent);

            expectedActionType = typeof(Action<WrappedEvent<FakeEvent>>);

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

        It should_pass_handle_to_fake_table_projector = () =>
        {
            fakeProjector.Handled.ShouldBeTrue();
        };

        It should_send_expected_event_to_handle = () =>
        {
            fakeProjector.Event.ShouldEqual(innerFakeEvent);
        };
    }
}