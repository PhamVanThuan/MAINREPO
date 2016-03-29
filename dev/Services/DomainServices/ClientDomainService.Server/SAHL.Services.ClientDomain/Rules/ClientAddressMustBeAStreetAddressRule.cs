using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Rules
{
    public class ClientAddressMustBeAStreetAddressRule<T> : IDomainRule<T> where T : ClientAddressAsPendingDomiciliumModel
    {
        private IDomiciliumAddressDataManager domiciliumAddressDataManager;

        public ClientAddressMustBeAStreetAddressRule(IDomiciliumAddressDataManager domiciliumAddressDataManager)
        {
            this.domiciliumAddressDataManager = domiciliumAddressDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, T ruleModel)
        {
            bool addressIsResidentialAddress = domiciliumAddressDataManager.CheckIsAddressTypeAResidentialAddress(ruleModel.ClientAddresskey);
            if (!addressIsResidentialAddress)
            {
                messages.AddMessage(new SystemMessage("Client address must be a street address.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}