using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantContactDetails
{
    public sealed class when_at_least_one_contact_detail_is_sent : WithFakes
    {
        private static IDomainRuleManager<Applicant> domainRuleManager;
        private static ISystemMessageCollection messages;
        private static Applicant applicant;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<Applicant>();
            messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new ApplicantContactDetailsValidation());

            applicant = new Applicant();
            applicant.EmailAddress = "me@test.com";
        };

        private Because of = () =>
        {
            domainRuleManager.ExecuteRules(messages, applicant);
        };

        private It should_have_no_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}