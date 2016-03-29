using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._1_month_timer.OnGetActivityTime
{
    [Subject("Activity => _1_month_timer => OnGetActivityTime")]
    internal class when_1_month_timer : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_1_month_timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_1_month_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddMonths(1), new TimeSpan(0, 0, 5));
        };
    }
}