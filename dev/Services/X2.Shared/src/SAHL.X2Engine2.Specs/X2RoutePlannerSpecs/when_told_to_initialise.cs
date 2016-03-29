using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Specs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2RoutePlannerSpecs
{
    public class when_told_to_initialise : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RoutePlanner> autoMocker;

        Establish context = () =>
        {
            
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RoutePlanner>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        It should_initialise_queue_manager = () =>
        {
            autoMocker.Get<IX2QueueManager>().WasToldTo(x => x.Initialise());
        };

        It should_initialise_consumer_monitor = () =>
        {
            autoMocker.Get<IX2ConsumerMonitor>().WasToldTo(x => x.Initialise());
        };
    }
}
