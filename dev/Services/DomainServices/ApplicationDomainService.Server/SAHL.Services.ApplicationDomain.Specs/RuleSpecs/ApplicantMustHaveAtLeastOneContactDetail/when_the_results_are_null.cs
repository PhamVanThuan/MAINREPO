using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantMustHaveAtLeastOneContactDetail
{
    public class when_the_results_are_null : WithFakes
    {
        private static ApplicantMustHaveAtLeastOneContactDetailRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static AddLeadApplicantToApplicationCommand command;
        private static ISystemMessageCollection messages;
        private static IEnumerable<GetClientDetailsQueryResult> results;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            results = Enumerable.Empty<GetClientDetailsQueryResult>();
            domainQueryClient = An<IDomainQueryServiceClient>();
            rule = new ApplicantMustHaveAtLeastOneContactDetailRule(domainQueryClient);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = null;
                return SystemMessageCollection.Empty();
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}