using System.Text;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendEmailToConsultantForValuationDoneCommandHandler : IHandlesDomainServiceCommand<SendEmailToConsultantForValuationDoneCommand>
    {
        IApplicationRepository applicationRepository;
        IPropertyRepository propertyRepository;
        IMessageService messageService;

        public SendEmailToConsultantForValuationDoneCommandHandler(IApplicationRepository applicationRepository, IPropertyRepository propertyRepository, IMessageService messageService)
        {
            this.applicationRepository = applicationRepository;
            this.propertyRepository = propertyRepository;
            this.messageService = messageService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendEmailToConsultantForValuationDoneCommand command)
        {
            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendFormat("Valuation Complete for Offer {0}", command.ApplicationKey);
            sbBody.AppendLine();
            IApplicationMortgageLoan iaml = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;
            IProperty p = iaml.Property;
            /// Scenario: Person A and B are wanting to buy the same house. SAHL only wants ONE valuation.
            /// We create a val record for the prop. Second case comes along but there is already a valuation
            /// in progress for this propertykey. OnValuationComplete notify both A and B. The Val case's
            /// parent links to person A but NOT person B so we need to look for all offers that relate
            /// to the property.
            IEventList<IApplication> Apps = propertyRepository.GetApplicationsForProperty(iaml.Property.Key);

            foreach (IApplication app in Apps.Where(x => x.IsOpen))
            {
                foreach (IApplicationRole role in app.ApplicationRoles)
                {
                    if ((role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.ApplicationRoleType.Key == (int)OfferRoleTypes.NewBusinessProcessorD) ||
                        (role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.ApplicationRoleType.Key == (int)OfferRoleTypes.FLProcessorD) ||
                        (role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.ApplicationRoleType.Key == (int)OfferRoleTypes.BranchConsultantD) ||
                        (role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.ApplicationRoleType.Key == (int)OfferRoleTypes.BranchAdminD))
                    {
                        string fromAddress = SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo;
                        string toAddress = role.LegalEntity.EmailAddress;
                        string cc = string.Empty;
                        string bcc = string.Empty;
                        string subject = "Valuation Complete from Halo";
                        messageService.SendEmailInternal(fromAddress, toAddress, cc, bcc, subject, sbBody.ToString());
                    }
                }
            }
        }
    }
}