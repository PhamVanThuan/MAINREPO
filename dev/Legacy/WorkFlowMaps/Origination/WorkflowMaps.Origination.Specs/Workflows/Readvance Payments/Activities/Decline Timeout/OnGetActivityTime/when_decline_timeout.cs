using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Decline_Timeout.OnGetActivityTime
{
    [Subject("Activity => Decline_Timeout => OnGetActivityTime")]
    internal class when_decline_timeout : WorkflowSpecReadvancePayments
    {
        private static System.DateTime result;
        private static System.DateTime thirtyDays;

        private Establish context = () =>
        {
            result = System.DateTime.Now;
            thirtyDays = System.DateTime.Now.AddDays(30);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Decline_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_be_30_days_from_today = () =>
        {
            result.Date.ShouldEqual(thirtyDays.Date);
        };
    }
}