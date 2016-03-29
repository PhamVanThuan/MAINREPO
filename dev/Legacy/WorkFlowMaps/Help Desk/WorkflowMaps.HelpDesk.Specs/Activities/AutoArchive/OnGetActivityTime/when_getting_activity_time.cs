using Machine.Specifications;
using System;

namespace WorkflowMaps.HelpDesk.Specs.Activities.AutoArchive.OnGetActivityTime
{
    [Subject("Activity => AutoArchive => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecHelpDesk
    {
        private static int expectedMonths;
        private static int expectedDay;
        private static int expectedYear;
        private static DateTime timerValue;

        private Establish context = () =>
        {
            expectedMonths = DateTime.Now.AddMonths(3).Month;
            expectedYear = DateTime.Now.AddMonths(3).Year;
            expectedDay = DateTime.Now.AddMonths(3).Day;
        };

        private Because of = () =>
        {
            timerValue = workflow.GetActivityTime_AutoArchive(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_the_timer_to_three_months_from_today = () =>
        {
            timerValue.Month.ShouldEqual(expectedMonths);
            timerValue.Day.ShouldEqual(expectedDay);
            timerValue.Year.ShouldEqual(expectedYear);
        };
    }
}