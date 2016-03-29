using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Note_Comment.OnComplete
{
    [Subject("Activity => Note_Comment => OnComplete")]
    internal class when_note_comment_when_is_further_lending : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Note_Comment(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}