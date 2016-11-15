using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Common_Disqualify_30_yr_term.OnEnter
{
    [Subject("State => Common_Disqualify_30_yr_term => OnEnter")] // AutoGenerated
    internal class when_common_disqualify_30_yr_term : WorkflowSpecCredit
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Common_Disqualify_30_yr_term(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}