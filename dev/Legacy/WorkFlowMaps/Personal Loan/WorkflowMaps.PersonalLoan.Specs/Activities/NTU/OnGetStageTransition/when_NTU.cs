using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU.OnGetStageTransition
{
    [Subject("State => NTU => OnGetStageTransition")]
    internal class when_NTU : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}