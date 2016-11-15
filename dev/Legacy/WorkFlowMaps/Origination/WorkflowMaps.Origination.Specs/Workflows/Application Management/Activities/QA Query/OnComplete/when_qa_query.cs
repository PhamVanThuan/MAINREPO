using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.QA_Query.OnComplete
{
    [Subject("Activity => QA_Query => OnComplete")] // AutoGenerated
    internal class when_qa_query : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_QA_Query(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}