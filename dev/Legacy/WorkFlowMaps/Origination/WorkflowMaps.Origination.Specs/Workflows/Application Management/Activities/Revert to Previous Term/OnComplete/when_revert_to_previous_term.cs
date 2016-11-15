using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Revert_to_Previous_Term.OnComplete
{
    [Subject("Activity => Revert_to_Previous_Term => OnComplete")] // AutoGenerated
    internal class when_revert_to_previous_term : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Revert_to_Previous_Term(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}