using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.Services;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2ActivitySchedulerSpecs
{
    public class when_initialising_the_activity_scheduler_with_scheduled_activities_that_need_to_fire_in_the_distant_future : WithFakes
    {
        private static AutoMocker<X2ActivityScheduler> automocker = new NSubstituteAutoMocker<X2ActivityScheduler>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static ScheduledActivityDataModel scheduledActivityDataModel;
        private static ActivityDataModel activityDataModel;
        private static IX2ScheduledActivityTimer timer;
        private static IX2Engine engine;

        private Establish context = () =>
        {
            engine = An<IX2Engine>();
            activityDataModel = new ActivityDataModel(1, "Name", 1, 1, 2, false, 1, 1, "", null, null, null, null, 1, Guid.NewGuid());
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(1)).Return(activityDataModel);
            scheduledActivityDataModel = new ScheduledActivityDataModel(12, DateTime.Now.AddMonths(1), 1, 1, "");
            scheduledActivities.Add(scheduledActivityDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetAllScheduledTimerActivities()).Return(scheduledActivities);
            automocker.Get<IX2SoonestActivityScheduleSelector>().WhenToldTo(x => x.GetNextActivityToSchedule(Param.IsAny<IEnumerable<ScheduledActivityDataModel>>())).Return(scheduledActivityDataModel);
            timer = new X2ScheduledActivityTimer();
            automocker.Get<IX2ScheduledActivityTimerFactory>().WhenToldTo(x => x.CreateTimer()).Return(timer);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.Initialise(engine);
        };

        private It should_return_the_activity_with_the_lowest_activity_time = () =>
        {
            automocker.Get<IX2SoonestActivityScheduleSelector>().WasToldTo(x => x.GetNextActivityToSchedule(Param.IsAny<IEnumerable<ScheduledActivityDataModel>>()));
        };
    }
}