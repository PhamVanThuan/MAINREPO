using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.ReAssign.OnGetStageTransition
{
    [Subject("Activity => ReAssign => OnGetStageTransition")] // AutoGenerated
    internal class when_reassign : WorkflowSpecApplicationCapture
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_ReAssign(messages, workflowData, instanceData, paramsData);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}