using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2ActivitySchedulerSpecs
{
    public class when_initialising_the_activity_scheduler_with_no_scheduled_activities : WithFakes
    {
        private static AutoMocker<X2ActivityScheduler> automocker = new NSubstituteAutoMocker<X2ActivityScheduler>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static IX2Engine engine;

        private Establish context = () =>
            {
                engine = An<IX2Engine>();
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetAllScheduledTimerActivities()).Return(scheduledActivities);
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.Initialise(engine);
            };

        private It should_get_a_list_of_scheduled_activities = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetAllScheduledTimerActivities());
            };
    }
}