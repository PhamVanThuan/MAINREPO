using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Note_Comment.OnStart
{
    [Subject("Activity => Note_Comment => OnStart")] // AutoGenerated
    internal class when_note_comment : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Note_Comment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}