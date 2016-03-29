using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Require_Arrear_Comment.OnGetStageTransition
{
    [Subject("Activity => Require_Arrear_Comment => OnGetStageTransition")]
    internal class when_require_arrear_comment : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Require_Arrear_Comment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_require_arrear_comment = () =>
        {
            result.ShouldEqual("Require Arrear Comment");
        };
    }
}