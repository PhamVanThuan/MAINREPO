using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_Send_Feedback.OnExit
{
    [Subject("State => Common_Send_Feedback => OnExit")] // AutoGenerated
    internal class when_common_send_feedback : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnExit_Common_Send_Feedback(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}