using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2SoonestActivityScheduleSelectorSpecs
{
    public class when_getting_the_next_activity_and_the_earliest_is_second_in_the_list : WithFakes
    {
        private static AutoMocker<X2SoonestActivityScheduleSelector> automocker = new NSubstituteAutoMocker<X2SoonestActivityScheduleSelector>();
        private static List<ScheduledActivityDataModel> scheduledActivities = new List<ScheduledActivityDataModel>();
        private static ScheduledActivityDataModel scheduledActivityDataModel;
        private static ScheduledActivityDataModel scheduledActivityDataModelLowest;
        private static ScheduledActivityDataModel selected;

        Establish context = () =>
            {
                scheduledActivityDataModel = new ScheduledActivityDataModel(12, DateTime.Now.AddSeconds(-1), 1, 1, "");
                scheduledActivities.Add(scheduledActivityDataModel);
                scheduledActivityDataModelLowest = new ScheduledActivityDataModel(12, DateTime.Now.AddSeconds(-10), 1, 1, "");
                scheduledActivities.Add(scheduledActivityDataModelLowest);
            };

        Because of = () =>
            {
                selected = automocker.ClassUnderTest.GetNextActivityToSchedule(scheduledActivities);
            };

        It should_return_the_activity_with_the_lowest_activity_time = () =>
            {
                selected.ShouldEqual(scheduledActivityDataModelLowest);
            };
    }
}