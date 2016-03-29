using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.ITC.Commands;
using System.Collections.Generic;
using rules = SAHL.Services.ITC.Rules;

namespace SAHL.Services.ITC.Server.Specs.Rules.ClientRelatedToAccount
{
    class when_client_not_related_to_account : WithFakes
    {
        private static rules.ClientShouldBeRelatedToAccountRule rule;
        private static ISystemMessageCollection messages;
        private static IApplicationDomainServiceClient applicationDomainClient;
        private static PerformClientITCCheckCommand command;
        private static DoesAccountBelongToClientQuery query;

        private Establish context = () =>
        {
            command = new PerformClientITCCheckCommand("8507125499086", 12122, "X2");
            applicationDomainClient = An<IApplicationDomainServiceClient>();
            query = new DoesAccountBelongToClientQuery(10001, "8507125499086");
            rule = new rules.ClientShouldBeRelatedToAccountRule(applicationDomainClient);
            messages = SystemMessageCollection.Empty();

            applicationDomainClient.WhenToldTo(x => x.PerformQuery(Param<DoesAccountBelongToClientQuery>.Matches(y => y.IdNumber == command.IdNumber))).Return<DoesAccountBelongToClientQuery>(y =>
            {
                y.Result = new ServiceQueryResult<DoesAccountBelongToClientQueryResult>(
                    new List<DoesAccountBelongToClientQueryResult> { new DoesAccountBelongToClientQueryResult() });
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_chech_if_client_is_related_to_account = () =>
        {
            applicationDomainClient.WasToldTo(x => x.PerformQuery(Param<DoesAccountBelongToClientQuery>.Matches(y => y.IdNumber == command.IdNumber)));
        };

        private It should_return_a_message = () =>
        {
            messages.AllMessages.ShouldContain<ISystemMessage>(x => x.Message == "The client is not related to the account." && x.Severity == SystemMessageSeverityEnum.Error);
        };
    }
}