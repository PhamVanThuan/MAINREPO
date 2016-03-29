using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Email_Disbursed_Letter.OnStart
{
    [Subject("Activity => Email_Disbursed_Letter => OnStart")]
    internal class when_cannot_email_disbursement_letter : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan personalLoan;

        private Establish context = () =>
        {
            result = false;
            personalLoan = An<IPersonalLoan>();
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(personalLoan);
            personalLoan.WhenToldTo(x => x.CheckCanEmailPersonalLoanApplicationRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, false)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Email_Disbursed_Letter(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}