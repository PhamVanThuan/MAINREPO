using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Archive_Completed_Followup.OnGetActivityTime
{
    [Subject("Activity => Archive_Completed_Followup => OnGetActivityTime")]
    internal class when_archive_completed_followup : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Archive_Completed_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_10_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(10), new TimeSpan(0, 0, 5));
        };
    }
}