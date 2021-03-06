using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Reinstate_NTU.OnComplete
{
    [Subject("Activity => Reinstate_NTU => OnComplete")] // AutoGenerated
    internal class when_reinstate_ntu : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}