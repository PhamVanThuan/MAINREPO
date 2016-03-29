using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.CommandHandlers.Internal
{
    public class AddClientAddressCommandHandler : IServiceCommandHandler<AddClientAddressCommand>
    {
        private IAddressDataManager addressDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IDomainRuleManager<ClientAddressModel> domainRuleManager;

        public AddClientAddressCommandHandler(IAddressDataManager addressDataManager, ILinkedKeyManager linkedKeyManager, IDomainRuleManager<ClientAddressModel> domainRuleManager)
        {
            this.addressDataManager = addressDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.domainRuleManager = domainRuleManager;

            this.domainRuleManager.RegisterRule(new AddressCannotBeAnExistingClientAddressRule(addressDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddClientAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.domainRuleManager.ExecuteRules(messages, command.ClientAddressModel);

            if (messages.HasErrors)
            {
                return messages;
            }

            int clientAddressKey = addressDataManager.SaveClientAddress(command.ClientAddressModel);
            linkedKeyManager.LinkKeyToGuid(clientAddressKey, command.ClientAddressGuid);
            return messages;
        }
    }
}