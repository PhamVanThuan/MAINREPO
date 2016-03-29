using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantPassportNumberRequiredRuleSpecs
{
    public class when_the_applicant_is_a_nonresident_with_a_valid_passport_number : WithFakes
    {
        private static ApplicantPassportNumberRequiredRule rule;
        private static IValidationUtils validationUtils;
        private static Applicant applicant;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            rule = new ApplicantPassportNumberRequiredRule(validationUtils);
            applicant = new Applicant();
            applicant.SAHLSACitizenType = "Non - Resident";
            applicant.PassportNo = "A07328738";
            applicant.FirstName = "John";
            applicant.Surname = "Smith";
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}