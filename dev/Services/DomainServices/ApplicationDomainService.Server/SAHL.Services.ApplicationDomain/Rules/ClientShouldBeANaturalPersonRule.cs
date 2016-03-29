using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientShouldBeANaturalPersonRule : IDomainRule<ApplicantDeclarationsModel>
    {
        private IDomainQueryServiceClient domainQueryService;

        public ClientShouldBeANaturalPersonRule(IDomainQueryServiceClient domainQueryService)
        {
            this.domainQueryService = domainQueryService;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ApplicantDeclarationsModel applicantDeclarationsModel)
        {
            var isClientANaturalPersonQuery = new IsClientANaturalPersonQuery(applicantDeclarationsModel.ClientKey);
            domainQueryService.PerformQuery(isClientANaturalPersonQuery);
            if (isClientANaturalPersonQuery.Result.Results.Any())
            {
                if (!isClientANaturalPersonQuery.Result.Results.First().ClientIsANaturalPerson)
                {
                    messages.AddMessage(new SystemMessage("Declarations can only be provided for an application on a client that is a Natural Person.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}