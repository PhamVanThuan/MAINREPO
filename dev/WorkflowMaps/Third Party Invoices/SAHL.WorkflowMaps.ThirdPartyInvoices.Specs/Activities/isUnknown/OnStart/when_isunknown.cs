using Machine.Specifications;

namespace WorkflowMaps.ThirdPartyInvoices.Specs.Activities.isUnknown.OnStart
{
    [Subject("Activity => isUnknown => OnStart")] // AutoGenerated
    internal class when_isunknown : WorkflowSpecThirdPartyInvoices
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_isUnknown(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}