using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.Services;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.X2ActivitySchedulerSpecs
{
    public class when_recieving_a_notification_of_a_new_scheduled_activity_to_execute_in_the_future : WithFakes
    {
        private static AutoMocker<X2ActivityScheduler> automocker = new NSubstituteAutoMocker<X2ActivityScheduler>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static ScheduledActivityDataModel scheduledActivityDataModel;
        private static ActivityDataModel activityDataModel;
        private static IX2ScheduledActivityTimer timer;
        private static IX2Engine engine;
        private static X2NotificationOfNewScheduledActivityRequest message;
        private static long instanceId = 12;
        private static int activityId = 13;
        private static ScheduledActivityDataModel newScheduledActivityDataModel;
        private static ActivityDataModel newActivityDataModel;

        private Establish context = () =>
        {
            engine = An<IX2Engine>();
            activityDataModel = new ActivityDataModel(1, "Name", 1, 1, 2, false, 1, 1, "", null, null, null, null, 1, Guid.NewGuid());
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(1)).Return(activityDataModel);
            scheduledActivityDataModel = new ScheduledActivityDataModel(12, DateTime.Now.AddSeconds(10), 1, 1, "");
            scheduledActivities.Add(scheduledActivityDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetAllScheduledTimerActivities()).Return(scheduledActivities);
            automocker.Get<IX2SoonestActivityScheduleSelector>().WhenToldTo(x => x.GetNextActivityToSchedule(Param.IsAny<IEnumerable<ScheduledActivityDataModel>>())).Return(scheduledActivityDataModel);
            timer = An<IX2ScheduledActivityTimer>();
            automocker.Get<IX2ScheduledActivityTimerFactory>().WhenToldTo(x => x.CreateTimer()).Return(timer);

            automocker.ClassUnderTest.Initialise(engine);
            message = new X2NotificationOfNewScheduledActivityRequest(instanceId, activityId);
            newScheduledActivityDataModel = new ScheduledActivityDataModel(instanceId, DateTime.Now.AddSeconds(100), activityId, 1, "");
            newActivityDataModel = new ActivityDataModel(activityId, 1, "Name", 1, 1, 2, false, 1, 1, "", null, null, null, null, 1, Guid.NewGuid());
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(activityId)).Return(newActivityDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetScheduledActivity(instanceId, activityId)).Return(newScheduledActivityDataModel);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(message);
        };

        private It should_recalculate_when_the_next_scheduled_activity_should_fire = () =>
        {
            automocker.Get<IX2ScheduledActivityTimerFactory>().WasToldTo(x => x.CreateTimer()).Times(1);
        };

        private It should_setup_the_timer_for_the_next_scheduled_activity = () =>
        {
            timer.WasToldTo(x => x.Start(Param.IsAny<int>(), Param.IsAny<Action>()));
        };
    }
}