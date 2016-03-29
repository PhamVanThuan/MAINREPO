using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Credit_Score_Decline.OnStart
{
    [Subject("Activity => Credit_Score_Decline => OnStart")] // AutoGenerated
    internal class when_credit_score_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Credit_Score_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}