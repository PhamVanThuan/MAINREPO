using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Email_Disbursed_Letter.OnComplete
{
    [Subject("Activity => Email_Disbursed_Letter => OnComplete")]
    internal class when_email_disbursed_letter : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan personalLoan;
        private static string adUserName;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            personalLoan = An<IPersonalLoan>();
            adUserName = @"SAHL\PLCUser";
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(personalLoan);
            personalLoan.WhenToldTo(x => x.GetADUserNameByWorkflowRoleType((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD)).Return(adUserName);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Email_Disbursed_Letter(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_email_corresponding_report_to_mailing_address = () =>
        {
            personalLoan.WasToldTo(x => x.EmailCorrespondenceReportToApplicationMailingAddress((IDomainMessageCollection)messages, workflowData.ApplicationKey, adUserName, SAHL.Common.Constants.Reports.DisbursementLetter, SAHL.Common.Globals.CorrespondenceTemplates.EmailCorrespondenceGeneric));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}