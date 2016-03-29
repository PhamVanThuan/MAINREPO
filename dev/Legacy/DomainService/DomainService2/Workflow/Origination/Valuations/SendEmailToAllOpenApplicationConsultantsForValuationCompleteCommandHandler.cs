using System.Collections.Generic;
using System.Text;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler : IHandlesDomainServiceCommand<SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand>
    {
        private IOrganisationStructureRepository orgStructureRepository;
        private IPropertyRepository propertyRepository;
        private IMessageService messageService;

        public SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler(IOrganisationStructureRepository orgStructureRepository, IPropertyRepository propertyRepository, IMessageService messageService)
        {
            this.orgStructureRepository = orgStructureRepository;
            this.propertyRepository = propertyRepository;
            this.messageService = messageService;
        }

        public void Handle(IDomainMessageCollection messages, SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand command)
        {
            if (command.ValuationKey == 0)
            {
                // go get the property key from the application
                SendCompletedValuationEmailsForApp(command.ApplicationKey);
            }
            else
            {
                IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);

                IEventList<IApplication> applications = propertyRepository.GetApplicationsForProperty(valuation.Property.Key);

                foreach (var application in applications.Where(x=>x.IsOpen))
                {
                    SendCompletedValuationEmailsForApp(application.Key);
                }
            }
        }

        private void SendCompletedValuationEmailsForApp(int appKey)
        {
            List<string> emailAddresses = new List<string>();
            // Branch Consultant - 101
            IApplicationRole role = orgStructureRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(appKey, (int)SAHL.Common.Globals.OfferRoleTypes.BranchConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
            if (role != null)
                emailAddresses.Add(role.LegalEntity.EmailAddress);
            // NBP - 694
            role = orgStructureRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(appKey, (int)SAHL.Common.Globals.OfferRoleTypes.NewBusinessProcessorD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
            if (role != null)
                emailAddresses.Add(role.LegalEntity.EmailAddress);
            // FL - 857
            role = orgStructureRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(appKey, (int)SAHL.Common.Globals.OfferRoleTypes.FLProcessorD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
            if (role != null)
                emailAddresses.Add(role.LegalEntity.EmailAddress);
            // Branch Admin - 102
            role = orgStructureRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(appKey, (int)SAHL.Common.Globals.OfferRoleTypes.BranchAdminD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
            if (role != null)
                emailAddresses.Add(role.LegalEntity.EmailAddress);

            StringBuilder toEmailAddresses = new StringBuilder();
			toEmailAddresses.Append(String.Join(",", emailAddresses));

            StringBuilder body = new StringBuilder();
            body.AppendFormat("Valuation for application {0} has been completed", appKey);
            body.AppendLine();

			string emailsToSendTo = toEmailAddresses.ToString();
			if (!string.IsNullOrEmpty(emailsToSendTo))
			{
				messageService.SendEmailInternal(SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo, emailsToSendTo, "", "", "Valuation Complete for Application: " + appKey, body.ToString());
			}
        }
    }
}