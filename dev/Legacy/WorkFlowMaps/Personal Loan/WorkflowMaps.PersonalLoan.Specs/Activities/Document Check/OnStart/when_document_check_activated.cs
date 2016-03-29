using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Document_Check.OnStart
{
    [Subject("Activity => Document_Check => OnStart")]
    public class when_document_check_activated : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Document_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}