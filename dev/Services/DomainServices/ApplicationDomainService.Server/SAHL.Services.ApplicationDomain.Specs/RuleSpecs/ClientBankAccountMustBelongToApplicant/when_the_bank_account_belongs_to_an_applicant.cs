using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientBankAccountMustBelongToApplicant
{
    public class when_the_bank_account_belongs_to_an_applicant : WithFakes
    {
        private static ClientBankAccountMustBelongToApplicantOnApplicationRule rule;
        private static ISystemMessageCollection messages;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static ApplicationDebitOrderModel ruleModel;

        private Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            ruleModel = new ApplicationDebitOrderModel(1234657, 28, 123456);
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            rule = new ClientBankAccountMustBelongToApplicantOnApplicationRule(applicantDataManager);
            applicantDataManager.WhenToldTo(x => x.DoesBankAccountBelongToApplicantOnApplication(ruleModel.ApplicationNumber, ruleModel.ClientBankAccountKey)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}