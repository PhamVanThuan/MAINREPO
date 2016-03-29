using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Archive_Application.OnGetStageTransition
{
    [Subject("Activity => Archive_Application => OnGetStageTransition")] // AutoGenerated
    internal class when_archive_application : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Archive_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}