using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._30_Days.OnGetActivityTime
{
    [Subject("Activity => _30_Days => OnGetActivityTime")]
    internal class when_30_days : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_30_Days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_30_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(30), new TimeSpan(0, 0, 5));
        };
    }
}