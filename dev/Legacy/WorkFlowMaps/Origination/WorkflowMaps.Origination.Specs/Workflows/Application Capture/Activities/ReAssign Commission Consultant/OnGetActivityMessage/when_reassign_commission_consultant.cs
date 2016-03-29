using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.ReAssign_Commission_Consultant.OnGetActivityMessage
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnGetActivityMessage")]
    internal class when_reassign_commission_consultant : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abc";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}