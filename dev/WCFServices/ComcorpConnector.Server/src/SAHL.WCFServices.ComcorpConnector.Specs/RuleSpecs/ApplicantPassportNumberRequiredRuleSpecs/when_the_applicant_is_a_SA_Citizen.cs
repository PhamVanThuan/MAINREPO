using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantPassportNumberRequiredRuleSpecs
{
    public class when_the_applicant_is_a_SA_Citizen : WithFakes
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
            applicant.SAHLSACitizenType = "SA Citizen";
            applicant.PassportNo = string.Empty;
            applicant.FirstName = "Zoey";
            applicant.Surname = "Speed";
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
