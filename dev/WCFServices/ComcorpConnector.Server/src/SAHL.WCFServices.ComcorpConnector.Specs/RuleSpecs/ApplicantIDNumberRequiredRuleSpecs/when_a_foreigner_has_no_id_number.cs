using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantIDNumberRequiredRuleSpecs
{
    public class when_a_foreigner_has_no_id_number : WithFakes
    {
        private static ApplicantIDNumberRequiredRule rule;
        private static IValidationUtils validationUtils;
        private static Applicant applicant;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
            {
                messages = SystemMessageCollection.Empty();
                validationUtils = An<IValidationUtils>();
                applicant = new Applicant();
                applicant.FirstName = "Bob";
                applicant.Surname = "Smith";
                applicant.SAHLSACitizenType = "Foreigner";
                applicant.IdentificationNo = null;
                rule = new ApplicantIDNumberRequiredRule(validationUtils);
            };

        private Because of = () =>
            {
                rule.ExecuteRule(messages, applicant);
            };

        private It should_not_validate_the_ID_number = () =>
            {
                validationUtils.DidNotReceive().ValidateIDNumber(Arg.Any<string>());
            };

        private It should_return_no_messages = () =>
            {
                messages.ErrorMessages().ShouldBeEmpty();
            };
    }
}