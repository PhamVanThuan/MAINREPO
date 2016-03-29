using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Disbursement_Timer.OnGetActivityTime
{
    [Subject("Activity => Disbursement_Timer => OnGetActivityTime")]
    internal class when_disbursement_timer : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Disbursement_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_6_hours_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddHours(6), new TimeSpan(0, 0, 5));
        };
    }
}