﻿using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Rules
{
    public class ClientAddressCannotBeAPendingDomiciliumAddressRule<T> : IDomainRule<T> where T : ClientAddressAsPendingDomiciliumModel
    {
        private IDomiciliumAddressDataManager domiciliumAddressDataManager;

        public ClientAddressCannotBeAPendingDomiciliumAddressRule(IDomiciliumAddressDataManager domiciliumAddressDataManager)
        {
            this.domiciliumAddressDataManager = domiciliumAddressDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            bool IsClientAddressActiveDomicilium = domiciliumAddressDataManager.IsClientAddressPendingDomicilium(ruleModel.ClientAddresskey);

            if (IsClientAddressActiveDomicilium)
            {
                messages.AddMessage(new SystemMessage("This client address is already marked as a pending domicilium.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}