using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantMustHaveAtLeastOneContactDetail
{
    public class when_only_the_work_phone_code_and_number_are_provided : WithFakes
    {
        private static ApplicantMustHaveAtLeastOneContactDetailRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static ISystemMessageCollection messages;
        private static AddLeadApplicantToApplicationCommand command;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), 1234, 5678, Core.BusinessModel.Enums.LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            domainQueryClient = An<IDomainQueryServiceClient>();
            rule = new ApplicantMustHaveAtLeastOneContactDetailRule(domainQueryClient);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}