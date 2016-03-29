using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline_By_Credit_Timer.OnGetActivityTime
{
    [Subject("Activity => Decline_By_Credit_Timer => OnGetActivityTime")]
    internal class when_decline_by_credit_timer : WorkflowSpecApplicationManagement
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Decline_By_Credit_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_30_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(30), new TimeSpan(0, 0, 5));
        };
    }
}