using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Decline_By_Credit_Timer.OnGetStageTransition
{
    [Subject("Activity => Decline_By_Credit_Timer => OnGetStageTransition")] // AutoGenerated
    internal class when_decline_by_credit_timer : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_By_Credit_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}