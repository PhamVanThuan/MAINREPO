using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.not_EA_Application.OnGetStageTransition
{
    [Subject("Activity => not_EA_Application => OnGetStageTransition")] // AutoGenerated
    internal class when_not_ea_application : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_not_EA_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}