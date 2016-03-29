using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2SoonestActivityScheduleSelectorSpecs
{
    public class when_getting_the_soonest_activity : WithFakes
    {
        private static AutoMocker<X2SoonestActivityScheduleSelector> automocker = new NSubstituteAutoMocker<X2SoonestActivityScheduleSelector>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static ScheduledActivityDataModel scheduledActivityDataModel;
        private static ScheduledActivityDataModel scheduledActivityDataModelLowest;
        private static ScheduledActivityDataModel selected;

        private Establish context = () =>
            {
                scheduledActivityDataModelLowest = new ScheduledActivityDataModel(12, DateTime.Now.AddSeconds(-10), 1, 1, "");
                scheduledActivities.Add(scheduledActivityDataModelLowest);
                scheduledActivityDataModel = new ScheduledActivityDataModel(12, DateTime.Now.AddSeconds(-1), 1, 1, "");
                scheduledActivities.Add(scheduledActivityDataModel);
            };

        private Because of = () =>
            {
                selected = automocker.ClassUnderTest.GetNextActivityToSchedule(scheduledActivities);
            };

        private It should_return_the_activity_with_the_lowest_activity_time = () =>
            {
                selected.ShouldEqual(scheduledActivityDataModelLowest);
            };
    }
}