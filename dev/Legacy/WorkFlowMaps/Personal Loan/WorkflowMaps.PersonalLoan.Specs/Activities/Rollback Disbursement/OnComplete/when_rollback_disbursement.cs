using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rollback_Disbursement.OnComplete
{
    [Subject("Activity => Rollback_Disbursement => OnComplete")]
    internal class when_rollback_disbursement : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static ICommon commonClient;
        private static int expectedApplicationKey;

        private Establish context = () =>
            {
                workflowData.ApplicationKey = 1234567;
                result = false;
                client = An<IPersonalLoan>();
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Rollback_Disbursement(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_disbursed_personal_loan_account_to_application = () =>
        {
            client.WasToldTo(x => x.ReturnDisbursedPersonalLoanToApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}