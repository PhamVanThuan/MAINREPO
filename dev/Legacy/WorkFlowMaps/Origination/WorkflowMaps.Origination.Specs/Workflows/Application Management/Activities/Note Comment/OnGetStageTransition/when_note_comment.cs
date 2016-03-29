using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Note_Comment.OnGetStageTransition
{
    [Subject("Activity => Note_Comment => OnGetStageTransition")]
    internal class when_note_comment : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Note_Comment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_note_comment = () =>
        {
            result.ShouldMatch("Note Comment");
        };
    }
}