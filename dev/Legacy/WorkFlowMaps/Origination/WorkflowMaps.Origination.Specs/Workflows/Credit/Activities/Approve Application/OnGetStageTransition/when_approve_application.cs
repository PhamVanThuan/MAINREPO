using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Approve_Application.OnGetStageTransition
{
    [Subject("Activity => Approve_Application => OnGetStageTransition")]
    internal class when_approve_application : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Approve_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_approve_application = () =>
        {
            result.ShouldBeTheSameAs("Approve Application");
        };
    }
}