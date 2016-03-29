using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._2_Months.OnGetActivityTime
{
    [Subject("Activity => _2_Months => OnGetActivityTime")]
    internal class when_2_months : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_2_Months(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_2_month_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddMonths(2), new TimeSpan(0, 0, 5));
        };
    }
}