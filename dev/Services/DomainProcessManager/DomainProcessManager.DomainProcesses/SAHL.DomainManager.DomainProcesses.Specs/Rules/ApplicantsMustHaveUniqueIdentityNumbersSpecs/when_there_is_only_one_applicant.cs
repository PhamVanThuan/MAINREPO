using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.ApplicantsMustHaveUniqueIdentityNumbersSpecs
{
    public class when_there_is_only_one_applicant : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static ApplicantsMustHaveUniqueIdentityNumbersRule rule;
        private static ApplicationCreationModel model;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);
            rule = new ApplicantsMustHaveUniqueIdentityNumbersRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}