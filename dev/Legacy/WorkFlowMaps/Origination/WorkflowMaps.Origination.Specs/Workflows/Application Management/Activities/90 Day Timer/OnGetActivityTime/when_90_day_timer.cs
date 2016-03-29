using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._90_Day_Timer.OnGetActivityTime
{
    [Subject("Activity => _90_Day_Timer => OnGetActivityTime")]
    internal class when_90_day_timer : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_90_Day_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_90_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(90), new TimeSpan(0, 0, 5));
        };
    }
}