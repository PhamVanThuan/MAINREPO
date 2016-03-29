using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Credit_Score_Decline.OnExit
{
    [Subject("State => Common_Credit_Score_Decline => OnExit")] // AutoGenerated
    internal class when_common_credit_score_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Credit_Score_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}