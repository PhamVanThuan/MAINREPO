using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantMustHaveAtLeastOneContactDetailRule : IDomainRule<AddLeadApplicantToApplicationCommand>
    {
        private IDomainQueryServiceClient domainQueryServiceClient;

        public ApplicantMustHaveAtLeastOneContactDetailRule(IDomainQueryServiceClient domainQueryServiceClient)
        {
            this.domainQueryServiceClient = domainQueryServiceClient;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, AddLeadApplicantToApplicationCommand ruleModel)
        {
            var query = new GetClientDetailsQuery(ruleModel.ClientKey);
            domainQueryServiceClient.PerformQuery(query);
            if (query.Result != null)
            {
                if (query.Result.Results.Any())
                {
                    var client = query.Result.Results.First();
                    if (string.IsNullOrWhiteSpace(client.EmailAddress) && string.IsNullOrWhiteSpace(client.Cellphone)
                        && (string.IsNullOrWhiteSpace(client.HomePhoneCode) || string.IsNullOrWhiteSpace(client.HomePhone))
                        && (string.IsNullOrWhiteSpace(client.WorkPhoneCode) || string.IsNullOrWhiteSpace(client.WorkPhone)))
                    {
                        messages.AddMessage(new SystemMessage("An applicant requires at least one valid contact detail (An Email Address, Home, Work or Cell Number).", 
                            SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}