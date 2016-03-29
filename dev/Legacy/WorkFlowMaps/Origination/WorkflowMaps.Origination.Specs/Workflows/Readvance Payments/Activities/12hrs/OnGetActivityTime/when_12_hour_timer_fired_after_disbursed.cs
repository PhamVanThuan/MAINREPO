using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities._12hrs.OnGetActivityTime
{
    [Subject("Activity => _12hrs => OnGetActivityTime")]
    internal class when_12_hour_timer_fired_after_disbursed : WorkflowSpecReadvancePayments
    {
        private static System.DateTime result;
        private static System.DateTime twelveHours;

        private Establish context = () =>
        {
            result = System.DateTime.Now;
            twelveHours = System.DateTime.Now.AddHours(12);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_12hrs(instanceData, workflowData, paramsData, messages);
        };

        private It should_be_10_days_from_today = () =>
        {
            result.Hour.ShouldEqual(twelveHours.Hour);
        };
    }
}