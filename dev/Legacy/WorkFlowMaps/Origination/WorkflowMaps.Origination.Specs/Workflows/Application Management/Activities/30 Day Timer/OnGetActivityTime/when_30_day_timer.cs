using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._45_days.OnGetActivityTime
{
    [Subject("Activity => _30_day_timer => OnGetActivityTime")]
    internal class when_45_days : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_30_Day_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_45_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(30), new TimeSpan(0, 0, 5));
        };
    }
}