using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorFactorySpecs
{
    public class when_initialising : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitorFactory> autoMocker;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitorFactory>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        It should_set_up_the_response_handler = () =>
        {
            autoMocker.Get<IX2ResponseSubscriber>().WasToldTo(x => x.Subscribe<X2Response>(Param.IsAny<Action<X2Response>>()));
        };

    }
}
