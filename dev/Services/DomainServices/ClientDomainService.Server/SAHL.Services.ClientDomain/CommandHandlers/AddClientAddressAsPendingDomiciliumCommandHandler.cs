using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddClientAddressAsPendingDomiciliumCommandHandler : IDomainServiceCommandHandler<AddClientAddressAsPendingDomiciliumCommand, ClientAddressAsPendingDomiciliumAddedEvent>
    {
        private IDomiciliumAddressDataManager DomiciliumDataManager;
        private ILinkedKeyManager LinkedKeyManager;
        private ICombGuid CombGuid;
        private IUnitOfWorkFactory uowFactory;
        private IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> DomainRuleManager;
        private IEventRaiser eventRaiser;
        private IDomainQueryServiceClient DomainQueryService;
        private IADUserManager adUserManager;
        private ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliumAddedEvent;

        public AddClientAddressAsPendingDomiciliumCommandHandler(IDomiciliumAddressDataManager domiciliumDataManager, ILinkedKeyManager linkedKeyManager,
            IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager, ICombGuid combGuid, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser,
            IDomainQueryServiceClient domainQueryService, IADUserManager adUserManager)
        {
            this.DomiciliumDataManager = domiciliumDataManager;
            this.LinkedKeyManager = linkedKeyManager;
            this.CombGuid = combGuid;
            this.uowFactory = uowFactory;
            this.DomainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;
            this.DomainQueryService = domainQueryService;
            this.adUserManager = adUserManager;

            this.DomainRuleManager.RegisterRule(new Rules.ClientAddressMustBeAStreetAddressRule<ClientAddressAsPendingDomiciliumModel>(domiciliumDataManager));
            this.DomainRuleManager.RegisterRule(new Rules.AddressMustBeAnActiveClientAddressRule(domiciliumDataManager));
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddClientAddressAsPendingDomiciliumCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                //run rules
                this.DomainRuleManager.ExecuteRules(messages, command.ClientAddressAsPendingDomiciliumModel);
                if (messages.ErrorMessages().Any())
                {
                    uow.Complete();
                    return messages;
                }
                //get userKey
                int? ADUserKey = adUserManager.GetAdUserKeyByUserName(metadata.UserName);
                if (ADUserKey == null || ADUserKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to retrieve ADUserKey.", SystemMessageSeverityEnum.Error));
                    uow.Complete();
                    return messages;
                }

                //save clientdomicilium
                var clientDomiciliumData = new LegalEntityDomiciliumDataModel(command.ClientAddressAsPendingDomiciliumModel.ClientAddresskey, (int)GeneralStatus.Pending, DateTime.Now,
                    ADUserKey.Value);

                var domiciliumAddressKey = DomiciliumDataManager.SavePendingDomiciliumAddress(clientDomiciliumData);

                if (domiciliumAddressKey > 0)
                {
                    //linkKeyToGuid
                    this.LinkedKeyManager.LinkKeyToGuid(domiciliumAddressKey, command.ClientDomiciliumGuid);

                    //raise event
                    clientAddressAsPendingDomiciliumAddedEvent = new ClientAddressAsPendingDomiciliumAddedEvent(DateTime.Now, command.ClientAddressAsPendingDomiciliumModel.ClientAddresskey,
                                        domiciliumAddressKey);
                    eventRaiser.RaiseEvent(DateTime.Now, clientAddressAsPendingDomiciliumAddedEvent, domiciliumAddressKey, (int)GenericKeyType.Address, metadata);
                }

                uow.Complete();
            }

            return messages;
        }
    }
}