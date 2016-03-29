using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using System.Linq;

namespace SAHL.Services.AddressDomain.CommandHandlers.Internal
{
    public class AddPostalAddressCommandHandler : IServiceCommandHandler<AddPostalAddressCommand>
    {
        private IDomainRuleManager<AddressDataModel> domainRuleManager;
        private IAddressDataManager addressDataManager;
        private ILinkedKeyManager linkedKeyManager;

        public AddPostalAddressCommandHandler(IDomainRuleManager<AddressDataModel> domainRuleManager, IAddressDataManager addressDataService, ILinkedKeyManager linkedKeyManager)
        {
            this.domainRuleManager = domainRuleManager;
            this.addressDataManager = addressDataService;
            this.linkedKeyManager = linkedKeyManager;
            domainRuleManager.RegisterRule(new PostNetSuiteRequiresASuiteNumberRule());
        }

        public ISystemMessageCollection HandleCommand(AddPostalAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var postOffices = addressDataManager.GetPostOfficeForModelData(command.PostalAddressModel.Province, command.PostalAddressModel.City, command.PostalAddressModel.PostalCode);
            if(!postOffices.Any())
            {
                messages.AddMessage(new SystemMessage(
                    string.Format(@"No Post Office could be found for - City: {0}; Province: {1}; PostalCode: {2}", command.PostalAddressModel.City, command.PostalAddressModel.Province,
                    command.PostalAddressModel.PostalCode), SystemMessageSeverityEnum.Error)
                    );
                return messages;
            }
            int postOfficeKey = postOffices.First().PostOfficeKey;
            AddressDataModel addressModel = new AddressDataModel((int)command.PostalAddressModel.AddressFormat,
                                                    command.PostalAddressModel.BoxNumber
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , postOfficeKey
                                                    , null
                                                    , command.PostalAddressModel.Province
                                                    , command.PostalAddressModel.City
                                                    , command.PostalAddressModel.City
                                                    , command.PostalAddressModel.PostalCode
                                                    , null
                                                    , System.DateTime.Now
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null);

            // Remove magic number and use some enum to define contexts
            this.domainRuleManager.ExecuteRules(messages, addressModel);
            if (!messages.HasErrors)
            {
                int addressKey = addressDataManager.SaveAddress(addressModel);
                linkedKeyManager.LinkKeyToGuid(addressKey, command.AddressId);
            }

            return messages;
        }
    }
}