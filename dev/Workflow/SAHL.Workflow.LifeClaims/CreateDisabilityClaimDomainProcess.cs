using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SAHL.Workflow.LifeClaims
{
    public class CreateDisabilityClaimDomainProcess : ICreateDisabilityClaimDomainProcess
    {
        private ILifeDomainServiceClient lifeDomainServiceClient;

        public CreateDisabilityClaimDomainProcess(ILifeDomainServiceClient lifeDomainServiceClient)
        {
            this.lifeDomainServiceClient = lifeDomainServiceClient;
        }

        public string GetDisabilityClaimInstanceSubject(ISystemMessageCollection messages, int disabilityClaimKey)
        {
            GetDisabilityClaimInstanceSubjectQuery query = new GetDisabilityClaimInstanceSubjectQuery(disabilityClaimKey);
            ISystemMessageCollection serviceMessages = lifeDomainServiceClient.PerformQuery(query);
            return query.Result.Results.FirstOrDefault();
        }

        public bool CheckIfDisabilityClaimExclusionsExist(ISystemMessageCollection messages, int disabilityClaimKey)
        {
            var exclusionsExistQuery = new GetFurtherLendingExclusionsByDisabilityClaimKeyQuery(disabilityClaimKey);
            ISystemMessageCollection serviceMessages = lifeDomainServiceClient.PerformQuery(exclusionsExistQuery);
            return (exclusionsExistQuery.Result.Results.Count() > 0);
        }

        public void DisabilityClaimSendManualApprovalLetter(ISystemMessageCollection messages, int disabilityClaimKey)
        {
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata();
            var command = new SendDisabilityClaimManualApprovalLetterCommand(disabilityClaimKey);
            messages.Aggregate(lifeDomainServiceClient.PerformCommand(command, serviceRequestMetadata));
        }
        
    }
}