using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantContactDetails
{
    public sealed class when_no_contact_details_are_sent : WithFakes
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
            messages.ErrorMessages().First().Message.ShouldEndWith("At least one contact detail (Email, Home, Work or Cell Number) is required.");
        };
    }
}
