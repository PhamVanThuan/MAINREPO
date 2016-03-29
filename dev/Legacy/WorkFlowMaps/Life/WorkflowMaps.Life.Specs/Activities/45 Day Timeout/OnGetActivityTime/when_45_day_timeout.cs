using Machine.Specifications;
using System;

namespace WorkflowMaps.Life.Specs.Activities._45_Day_Timeout.OnGetActivityTime
{
    [Subject("Activity => _45_Day_Timeout => OnGetActivityTime")]
    internal class when_45_day_timeout : WorkflowSpecLife
    {
        private static DateTime result;
        private static DateTime expectedResult;

        private Establish context = () =>
        {
            expectedResult = DateTime.Now.AddDays(45);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_45_Day_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_45_days_from_now = () =>
        {
            result.ShouldBeCloseTo(expectedResult, new TimeSpan(0, 0, 1));
        };
    }
}