using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Escalate_to_Mgr_.OnGetStageTransition
{
    [Subject("Activity => Escalate_to_Mgr_ => OnGetStageTransition")] // AutoGenerated
    internal class when_escalate_to_mgr_ : WorkflowSpecCredit
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_Escalate_to_Mgr_(messages, workflowData, instanceData, paramsData);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}