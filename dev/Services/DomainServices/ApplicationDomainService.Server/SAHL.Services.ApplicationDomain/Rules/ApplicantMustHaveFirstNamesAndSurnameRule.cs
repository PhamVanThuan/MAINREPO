using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantMustHaveFirstNamesAndSurnameRule : IDomainRule<AddLeadApplicantToApplicationCommand>
    {
        private IDomainQueryServiceClient domainQueryClient;

        public ApplicantMustHaveFirstNamesAndSurnameRule(IDomainQueryServiceClient domainQueryClient)
        {
            this.domainQueryClient = domainQueryClient;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, AddLeadApplicantToApplicationCommand ruleModel)
        {
            var query = new GetClientDetailsQuery(ruleModel.ClientKey);
            domainQueryClient.PerformQuery(query);
            if (query.Result != null)
            {
                if (query.Result.Results.Any())
                {
                    var client = query.Result.Results.First();
                    if (client.LegalEntityType != (int)LegalEntityType.NaturalPerson)
                    {
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(client.FirstNames))
                    {
                        messages.AddMessage(new SystemMessage("A natural person applicant must have a first name.", SystemMessageSeverityEnum.Error));
                    }

                    if (string.IsNullOrWhiteSpace(client.Surname))
                    {
                        messages.AddMessage(new SystemMessage("A natural person applicant must have a surname.", SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}