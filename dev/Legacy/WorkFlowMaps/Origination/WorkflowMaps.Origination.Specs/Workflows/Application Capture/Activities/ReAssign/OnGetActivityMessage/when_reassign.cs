using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.ReAssign.OnGetActivityMessage
{
    [Subject("Activity => ReAssign => OnGetActivityMessage")]
    internal class when_reassign : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abc";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_ReAssign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}