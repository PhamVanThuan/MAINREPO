using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantMustHaveFirstNamesAndSurname
{
    public class when_no_results_are_returned : WithFakes
    {
        private static ApplicantMustHaveFirstNamesAndSurnameRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static AddLeadApplicantToApplicationCommand command;
        private static ISystemMessageCollection messages;
        private static IEnumerable<GetClientDetailsQueryResult> results;
        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            results = Enumerable.Empty<GetClientDetailsQueryResult>();
            domainQueryClient = An<IDomainQueryServiceClient>();
            rule = new ApplicantMustHaveFirstNamesAndSurnameRule(domainQueryClient);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientDetailsQueryResult>(results);
                return SystemMessageCollection.Empty();
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_use_the_domain_query_service_to_get_the_client = () =>
        {
            domainQueryClient.WasToldTo(c => c.PerformQuery(Arg.Is<GetClientDetailsQuery>(y => y.ClientKey == command.ClientKey)));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}