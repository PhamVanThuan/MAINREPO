using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientCannotBeAnExistingApplicant
{
    public class when_the_client_is_not_an_applicant : WithFakes
    {
        private static ClientCannotBeAnExistingApplicantRule rule;
        private static IApplicantDataManager applicantDataManager;
        private static AddLeadApplicantToApplicationCommand command;
        private static int clientKey;
        private static int applicationNumber;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            clientKey = 121212;
            applicationNumber = 1408282;
            applicantDataManager = An<IApplicantDataManager>();
            rule = new ClientCannotBeAnExistingApplicantRule(applicantDataManager);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), clientKey, applicationNumber, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            applicantDataManager.WhenToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(clientKey, applicationNumber)).Return(false);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}