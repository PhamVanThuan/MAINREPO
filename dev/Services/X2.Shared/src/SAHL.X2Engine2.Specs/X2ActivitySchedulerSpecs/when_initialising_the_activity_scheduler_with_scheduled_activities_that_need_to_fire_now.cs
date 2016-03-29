using System;
using System.Collections.Generic;
using System.Threading;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.X2ActivitySchedulerSpecs
{
    public class when_initialising_the_activity_scheduler_with_scheduled_activities_that_need_to_fire_now : WithFakes
    {
        private static AutoMocker<X2ActivityScheduler> automocker = new NSubstituteAutoMocker<X2ActivityScheduler>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static ScheduledActivityDataModel scheduledActivityDataModel;
        private static ActivityDataModel activityDataModel;
        private static IX2Engine engine;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long instanceID = 12;
        private static int callCount = 1;

        private Establish context = () =>
        {
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            engine = An<IX2Engine>();
            activityDataModel = new ActivityDataModel(1, "Name", 1, 1, 2, false, 1, 1, "", null, null, null, null, 1, Guid.NewGuid());
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(1)).Return(activityDataModel);
            automocker.Get<IX2EngineConfigurationProvider>().WhenToldTo(x => x.GetTimeToWaitUntilSchedulingActivities()).Return(0);

            scheduledActivityDataModel = new ScheduledActivityDataModel(instanceID, DateTime.Now.AddSeconds(-1), 1, 1, "");
            scheduledActivities.Add(scheduledActivityDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetScheduledActivity(Param.IsAny<long>(), Param.IsAny<int>())).Return(scheduledActivityDataModel);
            automocker.Get<IX2SoonestActivityScheduleSelector>().WhenToldTo(x => x.GetNextActivityToSchedule(Param.IsAny<IEnumerable<ScheduledActivityDataModel>>())).Return(() =>
            {
                if (callCount > 0)
                {
                    callCount--;
                    return scheduledActivityDataModel;
                }
                return null;
            });
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.Initialise(engine);
            Thread.Sleep(100);
        };

        private It should_return_the_activity_with_the_lowest_activity_time = () =>
        {
            automocker.Get<IX2SoonestActivityScheduleSelector>().WasToldTo(x => x.GetNextActivityToSchedule(Param.IsAny<IEnumerable<ScheduledActivityDataModel>>()));
        };

        private It should_route_the_request = () =>
        {
            engine.WasToldTo(x => x.ReceiveSystemRequest(Arg.Is<X2SystemRequestGroup>(y => y.InstanceId == instanceID)));
        };
    }
}