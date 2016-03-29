using System.Linq;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.ITC.Commands;

namespace SAHL.Services.ITC.Rules
{
    public class ClientShouldBeRelatedToAccountRule : IDomainRule<PerformClientITCCheckCommand>
    {
        private IApplicationDomainServiceClient ApplicationDomainClient;

        public ClientShouldBeRelatedToAccountRule(IApplicationDomainServiceClient applicationDomainClient)
        {
            this.ApplicationDomainClient = applicationDomainClient;
        }

        public void ExecuteRule(ISystemMessageCollection messages, PerformClientITCCheckCommand command)
        {
            if (command.AccountKey != null)
            {
                var query = new DoesAccountBelongToClientQuery((int)command.AccountKey, command.IdNumber);
                messages.Aggregate(ApplicationDomainClient.PerformQuery(query));
                if (query.Result == null || query.Result.Results == null || !query.Result.Results.Any() || query.Result.Results.First().OfferKey == null)
                {
                    messages.AddMessage(new SystemMessage("The client is not related to the account.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}