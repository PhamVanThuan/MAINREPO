using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using System;

namespace SAHL.Services.AddressDomain.CommandHandlers.Internal
{
    public class AddFreeTextAddressCommandHandler : IServiceCommandHandler<AddFreeTextAddressCommand>
    {
        private IAddressDataManager addressDataManager;
        private ILinkedKeyManager linkedKeyManager;

        public AddFreeTextAddressCommandHandler(IAddressDataManager addressDataManager, ILinkedKeyManager linkedKeyManager)
        {
            this.addressDataManager = addressDataManager;
            this.linkedKeyManager = linkedKeyManager;
        }

        public ISystemMessageCollection HandleCommand(AddFreeTextAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            int? postOfficeKey = null;
            if (!String.Equals(command.FreeTextAddress.Country, "South Africa", StringComparison.OrdinalIgnoreCase))
            {
                postOfficeKey = addressDataManager.GetPostOfficeKeyForCountry(command.FreeTextAddress.Country);
                if (postOfficeKey == null)
                {
                    messages.AddMessage(new SystemMessage(String.Format("Post Office not found for country : {0}", command.FreeTextAddress.Country), SystemMessageSeverityEnum.Error));
                    return messages;
                }
            }
            var addressDataModel = new AddressDataModel((int)command.FreeTextAddress.AddressFormat, null, null, null, null, null, null, null, postOfficeKey,
                null, null, null, null, null, null, DateTime.Now,  null, command.FreeTextAddress.FreeText1, command.FreeTextAddress.FreeText2,
                command.FreeTextAddress.FreeText3, command.FreeTextAddress.FreeText4, command.FreeTextAddress.FreeText5);

            int addressKey = addressDataManager.SaveAddress(addressDataModel);
            linkedKeyManager.LinkKeyToGuid(addressKey, command.AddressId);

            return messages;
        }
    }
}