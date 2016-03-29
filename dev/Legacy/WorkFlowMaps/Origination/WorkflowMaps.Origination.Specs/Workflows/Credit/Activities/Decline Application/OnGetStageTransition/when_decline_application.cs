using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline_Application.OnGetStageTransition
{
    [Subject("Activity => Decline_Application => OnGetStageTransition")]
    internal class when_decline_application : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline_application = () =>
        {
            result.ShouldBeTheSameAs("Decline Application");
        };
    }
}