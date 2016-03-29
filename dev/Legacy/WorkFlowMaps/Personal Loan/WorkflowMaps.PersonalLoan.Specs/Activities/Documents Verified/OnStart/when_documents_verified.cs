using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Documents_Verified.OnStart
{
    [Subject("Activity => Documents_Verified => OnStart")]
    internal class when_documents_verified : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Documents_Verified(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}