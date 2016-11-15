using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.MoveToCreate.OnComplete
{
    [Subject("Activity => MoveToCreate => OnComplete")] // AutoGenerated
    internal class when_movetocreate : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_MoveToCreate(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}