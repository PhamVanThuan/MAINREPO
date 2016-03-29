using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Force_Disbursement_Timer.OnGetActivityMessage
{
    [Subject("Activity => Force_Disbursement_Timer => OnGetActivityMessage")] // AutoGenerated
    internal class when_force_disbursement_timer : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Force_Disbursement_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}