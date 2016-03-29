using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantContactDetails
{
    public sealed class when_all_contact_details_are_sent : WithCoreFakes
    {
        private static IDomainRuleManager<Applicant> domainRuleManager;
        private static Applicant applicant;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<Applicant>();
            messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new ApplicantContactDetailsValidation());

            applicant = new Applicant();
            applicant.EmailAddress = "me@test.com";
            applicant.Cellphone = "2321321";
            applicant.WorkPhoneCode = "031";
            applicant.WorkPhone = "5555555";
            applicant.HomePhoneCode = "031";
            applicant.HomePhone = "5555555";
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
