using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantAge
{
    public sealed class when_applicant_is_too_old : WithCoreFakes
    {
        private static Applicant applicant;
        private static IValidationUtils _validationUtils;
        private static ApplicantAgeValidation rule;
        private static IRuleHelper ruleHelper;

        private Establish context = () =>
        {
            applicant = new Applicant();
            applicant.DateOfBirth = new System.DateTime(1900, 1, 1);
            messages = SystemMessageCollection.Empty();
            _validationUtils = An<IValidationUtils>();
            ruleHelper = An<IRuleHelper>();
            _validationUtils.WhenToldTo(x => x.GetAgeFromDateOfBirth(applicant.DateOfBirth)).Return(66);
            ruleHelper.WhenToldTo(x => x.GetApplicantNameForErrorMessage(applicant)).Return("Applicant : Craig Fraser");
            rule = new ApplicantAgeValidation(_validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
        };

        private It should_have_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEndWith("Applicant should be younger than 65.");
        };
    }
}