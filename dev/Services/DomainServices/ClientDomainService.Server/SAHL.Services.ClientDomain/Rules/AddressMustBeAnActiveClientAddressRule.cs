﻿using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class AddressMustBeAnActiveClientAddressRule : IDomainRule<ClientAddressAsPendingDomiciliumModel>
    {
        private IDomiciliumAddressDataManager domiciliumDataManager;

        public AddressMustBeAnActiveClientAddressRule(IDomiciliumAddressDataManager domiciliumDataManager)
        {
            this.domiciliumDataManager = domiciliumDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ClientAddressAsPendingDomiciliumModel ruleModel)
        {
            var clientAddress = domiciliumDataManager.FindExistingActiveClientAddress(ruleModel.ClientAddresskey);
            if (!clientAddress.Any())
            {
                messages.AddMessage(new SystemMessage("An active client address must be provided.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}