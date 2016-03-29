using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2SoonestActivityScheduleSelectorSpecs
{
    public class when_getting_the_next_activity_and_the_list_is_empty : WithFakes
    {
        private static AutoMocker<X2SoonestActivityScheduleSelector> automocker = new NSubstituteAutoMocker<X2SoonestActivityScheduleSelector>();
        private static List<ScheduledActivityDataModel> scheduledActivities;
        private static ScheduledActivityDataModel selected;

        Establish context = () =>
        {
            scheduledActivities = new List<ScheduledActivityDataModel>();
        };

        Because of = () =>
        {
            selected = automocker.ClassUnderTest.GetNextActivityToSchedule(scheduledActivities);
        };

        It should_return_null = () =>
        {
            selected.ShouldBeNull();
        };
    }
}