using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class LinkExternalVendorToApplicationCommandHandler : IDomainServiceCommandHandler<LinkExternalVendorToApplicationCommand, ExternalVendorLinkedToApplicationEvent>
    {
        private IApplicationDataManager applicationDataManager;
        private IDomainRuleManager<VendorModel> domainRuleManager;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private ExternalVendorLinkedToApplicationEvent @event;
  

        public LinkExternalVendorToApplicationCommandHandler(IApplicationDataManager applicationDataManager, 
            IDomainRuleManager<VendorModel> domainRuleManager,
            IUnitOfWorkFactory uowFactory,
            IEventRaiser eventRaiser)            
        {
            this.applicationDataManager = applicationDataManager;
            this.domainRuleManager = domainRuleManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;      
            
            domainRuleManager.RegisterRule(new ExternalVendorMustBeActiveRule());
        }

        public ISystemMessageCollection HandleCommand(LinkExternalVendorToApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            if (command.OriginationSource != OriginationSource.Comcorp)
            {
                systemMessages.AddMessage(new SystemMessage("Origination source must be Comcorp.", SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            // 0. Get the vendor based on the vendor code
            var vendorModel = applicationDataManager.GetExternalVendorForVendorCode(command.VendorCode);

            if (vendorModel == null)
            {
                systemMessages.AddMessage(new SystemMessage("Vendor could not be found.", SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            // 1. Check if the vendor is active
            domainRuleManager.ExecuteRules(systemMessages, vendorModel);
            
            if (systemMessages.AllMessages.Any())
            {
                return systemMessages;
            }

            using (var uow = uowFactory.Build())
            {
                var dateStamp = DateTime.Now;

                var offerDataModelRole = new OfferRoleDataModel(vendorModel.LegalEntityKey,
                    command.ApplicationNumber,
                    (int)OfferRoleType.ExternalVendor,
                    vendorModel.GeneralStatusKey,
                    dateStamp);
                int offerRoleKey = applicationDataManager.SaveExternalVendorOfferRole(offerDataModelRole);

                // Fire an event to signal completion
                @event = new ExternalVendorLinkedToApplicationEvent(command.ApplicationNumber, dateStamp, vendorModel.VendorKey, vendorModel.VendorCode, vendorModel.OrganisationStructureKey, 
                    vendorModel.LegalEntityKey, vendorModel.GeneralStatusKey);

                eventRaiser.RaiseEvent(DateTime.Now, @event, offerRoleKey, (int)GenericKeyType.OfferRole, metadata);

                uow.Complete();
            }

            return systemMessages;
        }
    }
}
