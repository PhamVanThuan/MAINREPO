using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.NTU_PipeLine.OnComplete
{
    [Subject("Activity => NTU_PipeLine => OnComplete")] // AutoGenerated
    internal class when_ntu_pipeline : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_NTU_PipeLine(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}