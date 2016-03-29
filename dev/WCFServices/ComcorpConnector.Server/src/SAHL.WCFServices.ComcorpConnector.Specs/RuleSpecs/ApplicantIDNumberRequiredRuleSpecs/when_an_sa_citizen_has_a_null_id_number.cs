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
    public class when_an_sa_citizen_has_a_null_id_number : WithFakes
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
            applicant.IdentificationNo = null;
            rule = new ApplicantIDNumberRequiredRule(validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
        };

        private It should_not_validate_the_ID_number = () =>
        {
            validationUtils.DidNotReceive().ValidateIDNumber(applicant.IdentificationNo);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Bob Smith : Applicant must have an ID Number.");
        };
    }
}
