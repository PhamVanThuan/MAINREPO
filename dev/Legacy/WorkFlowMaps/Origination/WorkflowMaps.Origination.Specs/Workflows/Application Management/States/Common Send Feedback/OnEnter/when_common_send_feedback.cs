using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Send_Feedback.OnEnter
{
    [Subject("State => Common_Send_Feedback => OnEnter")] // AutoGenerated
    internal class when_common_send_feedback : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Common_Send_Feedback(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}