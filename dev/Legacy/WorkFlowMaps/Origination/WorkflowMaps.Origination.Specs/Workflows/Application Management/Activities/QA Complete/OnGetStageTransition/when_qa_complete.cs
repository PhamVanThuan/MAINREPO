using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.QA_Complete.OnGetStageTransition
{
    [Subject("Activity => QA_Complete => OnGetStageTransition")]
    internal class when_qa_complete : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_QA_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_qa_complete = () =>
        {
            result.ShouldBeTheSameAs("QA Complete");
        };
    }
}