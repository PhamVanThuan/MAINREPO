using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.QA_Query.OnGetStageTransition
{
    [Subject("Activity => QA_Query => OnGetStageTransition")]
    internal class when_qa_query : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_QA_Query(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_qa_query = () =>
        {
            result.ShouldBeTheSameAs("QA Query");
        };
    }
}