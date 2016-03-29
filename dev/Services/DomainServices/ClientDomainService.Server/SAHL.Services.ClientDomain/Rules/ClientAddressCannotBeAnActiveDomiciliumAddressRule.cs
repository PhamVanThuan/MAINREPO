﻿using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Rules
{
    public class ClientAddressCannotBeAnActiveDomiciliumAddressRule<T> : IDomainRule<T> where T : ClientAddressAsPendingDomiciliumModel
    {
        private IDomiciliumAddressDataManager domiciliumAddressDataManager;

        public ClientAddressCannotBeAnActiveDomiciliumAddressRule(IDomiciliumAddressDataManager domiciliumAddressDataManager)
        {
            this.domiciliumAddressDataManager = domiciliumAddressDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            bool IsClientAddressActiveDomicilium = domiciliumAddressDataManager.IsClientAddressActiveDomicilium(ruleModel.ClientAddresskey, ruleModel.ClientKey);

            if (IsClientAddressActiveDomicilium)
            {
                messages.AddMessage(new SystemMessage("This Address is already set as the Client's active Domicilum Address.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}