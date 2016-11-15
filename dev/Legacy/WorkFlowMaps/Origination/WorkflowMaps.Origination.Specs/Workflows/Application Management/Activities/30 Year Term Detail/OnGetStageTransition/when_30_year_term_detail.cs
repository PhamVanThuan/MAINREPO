using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities._30_Year_Term_Detail.OnGetStageTransition
{
    [Subject("Activity => 30_Year_Term_Detail => OnGetStageTransition")] // AutoGenerated
    internal class when_30_year_term_detail : WorkflowSpecApplicationManagement
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_30_Year_Term_Detail(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}