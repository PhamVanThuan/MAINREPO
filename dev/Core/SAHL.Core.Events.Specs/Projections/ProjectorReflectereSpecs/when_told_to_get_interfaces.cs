using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Projections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Events.Specs.Projections.ProjectorReflectereSpecs
{
    [Subject("SAHL.Core.Events.Projections.ProjectorReflectere.GetInterfaces")]
    public class when_told_to_get_interfaces : WithFakes
    {
        static IProjectorReflectere reflectere;
        static IIocContainer container;
        static IEnumerable<Type> result;
        static FakeProjector fakeProjector;

        Establish context = () =>
        {
            fakeProjector = new FakeProjector();
            container = An<IIocContainer>();
            reflectere = new ProjectorReflectere(container);
        };

        Because of = () =>
        {
            result = reflectere.GetInterfaces(fakeProjector);
        };

        It should_return_results = () =>
        {
            result.Count().ShouldEqual(1);
        };

        It should_have_IEventProjector_in_results = () =>
        {
            result.Single().ShouldEqual(typeof(IEventProjector));
        };
    }
}