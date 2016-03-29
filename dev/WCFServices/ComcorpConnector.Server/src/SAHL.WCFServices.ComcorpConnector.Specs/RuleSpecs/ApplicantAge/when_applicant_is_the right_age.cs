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
    public sealed class when_applicant_is_the_right_age : WithCoreFakes
    {
        private static ApplicantAgeValidation rule;
        private static Applicant applicant;
        private static IValidationUtils validationUtils;
        private static IRuleHelper ruleHelper;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            ruleHelper = An<IRuleHelper>();
            applicant = new Applicant();
            applicant.DateOfBirth = System.DateTime.Now.AddYears(-25);
            validationUtils.WhenToldTo(x => x.GetAgeFromDateOfBirth(applicant.DateOfBirth)).Return(25);
            ruleHelper.WhenToldTo(x => x.GetApplicantNameForErrorMessage(applicant)).Return("Applicant : Craig Fraser");
            rule = new ApplicantAgeValidation(validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
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