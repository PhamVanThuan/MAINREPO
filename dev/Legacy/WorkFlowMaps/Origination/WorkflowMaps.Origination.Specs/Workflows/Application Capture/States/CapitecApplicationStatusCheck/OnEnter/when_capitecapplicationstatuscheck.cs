using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.CapitecApplicationStatusCheck.OnEnter
{
    [Subject("State => CapitecApplicationStatusCheck => OnEnter")] // AutoGenerated
    internal class when_capitecapplicationstatuscheck : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_CapitecApplicationStatusCheck(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}