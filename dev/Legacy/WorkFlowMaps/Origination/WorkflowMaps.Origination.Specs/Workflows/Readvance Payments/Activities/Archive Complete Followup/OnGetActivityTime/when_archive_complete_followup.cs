using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Archive_Complete_Followup.OnGetActivityTime
{
    [Subject("Activity => Archive_Complete_Followup => OnGetActivityTime")]
    internal class when_archive_Complete_Followup : WorkflowSpecReadvancePayments
    {
        private static System.DateTime result;
        private static System.DateTime tenDays;

        private Establish context = () =>
        {
            result = System.DateTime.Now;
            tenDays = System.DateTime.Now.AddDays(10);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Archive_Complete_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_be_10_days_from_today = () =>
        {
            result.Date.ShouldEqual(tenDays.Date);
        };
    }
}