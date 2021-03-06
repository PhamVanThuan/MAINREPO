using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.CapitecDeclinedApplication.OnGetStageTransition
{
    [Subject("Activity => CapitecDeclinedApplication => OnGetStageTransition")] // AutoGenerated
    internal class when_capitecdeclinedapplication : WorkflowSpecApplicationCapture
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_CapitecDeclinedApplication(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}