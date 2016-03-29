using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantAge
{
    public sealed class when_applicant_is_too_young : WithCoreFakes
    {
        private static IDomainRuleManager<Applicant> domainRuleManager;
        private static Applicant applicant;
        private static IValidationUtils validationUtils;
        private static IRuleHelper ruleHelper;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            ruleHelper = An<IRuleHelper>();
            validationUtils.WhenToldTo(x => x.GetAgeFromDateOfBirth(Param.IsAny<DateTime>())).Return(17);
            ruleHelper.WhenToldTo(x => x.GetApplicantNameForErrorMessage(applicant)).Return("Applicant : Craig Fraser");
            domainRuleManager = new DomainRuleManager<Applicant>();
            messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new ApplicantAgeValidation(validationUtils));

            applicant = new Applicant();
            applicant.DateOfBirth = System.DateTime.Now;

        };

        private Because of = () =>
        {
            domainRuleManager.ExecuteRules(messages, applicant);
        };

        private It should_have_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEndWith("Applicant should be older than 18.");
        };
    }
}
