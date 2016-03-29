using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Reinstate_NTU.OnComplete
{
    [Subject("Activity => Reinstate NTU => OnComplete")]
    internal class when_reinstate_ntu : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}