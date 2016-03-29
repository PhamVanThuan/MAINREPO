using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.Rules
{
    public class AddressCannotBeAnExistingClientAddressRule : IDomainRule<ClientAddressModel>
    {
        private IAddressDataManager addressDataManager;

        public AddressCannotBeAnExistingClientAddressRule(IAddressDataManager addressDataManager)
        {
            this.addressDataManager = addressDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ClientAddressModel ruleModel)
        {
            LegalEntityAddressDataModel existingClientAddress = this.addressDataManager.GetExistingActiveClientAddress(ruleModel.ClientKey, ruleModel.AddressKey, ruleModel.AddressType);
            if (existingClientAddress != null)
            {
                messages.AddMessage(new SystemMessage(string.Format("An active {2} address for ClientKey: {0} and AddressKey: {1} already exists.", ruleModel.ClientKey, ruleModel.AddressKey,
                ruleModel.AddressType.ToString()), SystemMessageSeverityEnum.Error));
            }

        }
    }
}