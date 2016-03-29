using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class SendDisabilityClaimManualApprovalLetterCommandHandler : IServiceCommandHandler<SendDisabilityClaimManualApprovalLetterCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private ICommunicationsServiceClient communicationsServiceClient;
        private ICombGuid combGuidGenerator;
        private IServiceQueryRouter serviceQueryRouter;
        private IEventRaiser eventRaiser;

        public SendDisabilityClaimManualApprovalLetterCommandHandler(ILifeDomainDataManager lifeDomainDataManager, ICommunicationsServiceClient communicationsServiceClient, 
            ICombGuid combGuidGenerator, IServiceQueryRouter serviceQueryRouter, IEventRaiser eventRaiser)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.communicationsServiceClient = communicationsServiceClient;
            this.combGuidGenerator = combGuidGenerator;
            this.serviceQueryRouter = serviceQueryRouter;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(SendDisabilityClaimManualApprovalLetterCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var emailAddress = this.lifeDomainDataManager.GetEmailAddressForUserWhoApprovedDisabilityClaim(command.DisabilityClaimKey);

            var getDisabilityClaimByKeyQuery = new GetDisabilityClaimByKeyQuery(command.DisabilityClaimKey);
            messages.Aggregate(this.serviceQueryRouter.HandleQuery(getDisabilityClaimByKeyQuery));

            var getFurtherLendingExclusionsByDisabilityClaimKeyQuery = new GetFurtherLendingExclusionsByDisabilityClaimKeyQuery(command.DisabilityClaimKey);
            messages.Aggregate(this.serviceQueryRouter.HandleQuery(getFurtherLendingExclusionsByDisabilityClaimKeyQuery));

            string subject = string.Format("Manual Approval Letter required for Disability Claim on Life Policy: {0}", getDisabilityClaimByKeyQuery.Result.Results.FirstOrDefault().LifeAccountKey);
            string messageBody = "The following exclusions apply:- \n";

            foreach (var exclusion in getFurtherLendingExclusionsByDisabilityClaimKeyQuery.Result.Results)
            {
                messageBody += string.Format("\n Loan Account {0} had a {1} on {2} for {3}", exclusion.AccountKey, exclusion.Description, exclusion.Date.ToSAHLDateString(), 
                    exclusion.Amount.ToSAHLCurrencyFormat());   
            }

            var emailModel = new StandardEmailModel(emailAddress, subject, messageBody, MailPriority.Normal);
            IEmailTemplate<IEmailModel> email = new StandardEmailTemplate(emailModel);
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata();
            var sendEmailCorrelationId = combGuidGenerator.Generate();
            var sendEmailCommand = new SendInternalEmailCommand(sendEmailCorrelationId, email);
            messages.Aggregate(communicationsServiceClient.PerformCommand(sendEmailCommand, serviceRequestMetadata));

            var descriptions = getFurtherLendingExclusionsByDisabilityClaimKeyQuery.Result.Results.Select(x => x.Description).ToList();

            eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimManualApprovalLetterSentEvent(DateTime.Now, command.DisabilityClaimKey, descriptions, emailAddress), 
                command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata); 

            return messages;
        }
    }
}
