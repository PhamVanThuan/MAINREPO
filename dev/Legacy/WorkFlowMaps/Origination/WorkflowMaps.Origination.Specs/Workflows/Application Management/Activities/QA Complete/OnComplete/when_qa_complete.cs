using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.QA_Complete.OnComplete
{
    [Subject("Activity => QA_Complete => OnComplete")] // AutoGenerated
    internal class when_qa_complete : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_QA_Complete(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}