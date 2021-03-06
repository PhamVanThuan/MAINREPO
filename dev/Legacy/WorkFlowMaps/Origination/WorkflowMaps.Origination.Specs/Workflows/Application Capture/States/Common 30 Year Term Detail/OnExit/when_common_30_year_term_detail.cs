using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_30_Year_Term_Detail.OnExit
{
    [Subject("State => Common_30_Year_Term_Detail => OnExit")] // AutoGenerated
    internal class when_common_30_year_term_detail : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnExit_Common_30_Year_Term_Detail(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}