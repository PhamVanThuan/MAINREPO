using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Projections;
using System;

namespace SAHL.Core.Events.Specs.Projections.ProjectorReflectereSpecs
{
    [Subject("SAHL.Core.Events.Projections.ProjectorReflectere.GetEventType")]
    public class whe_told_to_get_event_type : WithFakes
    {
        static IProjectorReflectere reflectere;
        static IIocContainer container;
        static Type type;
        static Type projectionBaseType;

        Establish context = () =>
        {
            projectionBaseType = typeof(IEventProjector<FakeEvent>);
            container = An<IIocContainer>();
            reflectere = new ProjectorReflectere(container);
        };

        Because of = () =>
        {
            type = reflectere.GetEventType(projectionBaseType);
        };

        It should = () =>
        {
            type.ShouldEqual(typeof(FakeEvent));
        };
    }
}