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
    public class when_an_sa_citizen_has_an_invalid_number : WithFakes
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
            applicant.SAHLSACitizenType = "SA Citizen";
            applicant.IdentificationNo = "8211045329082";
            rule = new ApplicantIDNumberRequiredRule(validationUtils);
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(Arg.Any<string>())).Return(false);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
        };

        private It should_validate_the_ID_number = () =>
        {
            validationUtils.Received().ValidateIDNumber(applicant.IdentificationNo);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Bob Smith : Applicant ID Number is invalid.");
        };
    }
}