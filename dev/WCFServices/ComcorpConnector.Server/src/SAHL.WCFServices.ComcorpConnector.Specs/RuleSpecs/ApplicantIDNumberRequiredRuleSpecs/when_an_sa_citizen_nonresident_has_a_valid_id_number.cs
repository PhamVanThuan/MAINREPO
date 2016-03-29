using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantIDNumberRequiredRuleSpecs
{
    public class when_an_sa_citizen_nonresident_has_a_valid_id_number : WithFakes
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
            applicant.SAHLSACitizenType = "SA Citizen – Non Resident";
            applicant.IdentificationNo = "8211045229080";
            rule = new ApplicantIDNumberRequiredRule(validationUtils);
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(Arg.Any<string>())).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicant);
        };

        private It should_validate_the_ID_number = () =>
        {
            validationUtils.Received().ValidateIDNumber(applicant.IdentificationNo);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
