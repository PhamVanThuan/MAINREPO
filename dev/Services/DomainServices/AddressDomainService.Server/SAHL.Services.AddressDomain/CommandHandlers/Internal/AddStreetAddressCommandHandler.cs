using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.AddressDomain.CommandHandlers.Internal
{
    public class AddStreetAddressCommandHandler : IServiceCommandHandler<AddStreetAddressCommand>
    {
        private IDomainRuleManager<AddressDataModel> domainRuleContext;
        private IAddressDataManager addressDataManager;
        private ILinkedKeyManager linkedKeyManager;

        public AddStreetAddressCommandHandler(IDomainRuleManager<AddressDataModel> domainRuleContext, IAddressDataManager addressDataService, ILinkedKeyManager linkedKeyManager)
        {
            this.domainRuleContext = domainRuleContext;
            this.addressDataManager = addressDataService;
            this.linkedKeyManager = linkedKeyManager;
            this.domainRuleContext.RegisterRule(new StreetAddressRequiresAValidSuburbRule());
        }

        public ISystemMessageCollection HandleCommand(AddStreetAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            const int addressFormatKey = (int)AddressFormat.Street;

            var suburb = addressDataManager.GetSuburbForModelData(command.StreetAddressModel.Suburb, command.StreetAddressModel.City,  command.StreetAddressModel.PostalCode, 
                command.StreetAddressModel.Province);
            if (!suburb.Any())
            {
                messages.AddMessage(new SystemMessage(string.Format("No Suburb could be found for - Suburb: {0}; City: {1}; Province: {2}; PostalCode: {3}", command.StreetAddressModel.Suburb,
                    command.StreetAddressModel.City, command.StreetAddressModel.Province, command.StreetAddressModel.PostalCode),
                    SystemMessageSeverityEnum.Error));
                return messages;
            }
            int suburbKey = suburb.Any() ? suburb.First().SuburbKey : 0;
            AddressDataModel addressDataModel = new AddressDataModel(
                                                      addressFormatKey
                                                    , null
                                                    , command.StreetAddressModel.UnitNumber
                                                    , command.StreetAddressModel.BuildingNumber
                                                    , command.StreetAddressModel.BuildingName
                                                    , command.StreetAddressModel.StreetNumber
                                                    , command.StreetAddressModel.StreetName
                                                    , suburbKey
                                                    , null
                                                    , ""
                                                    , command.StreetAddressModel.Province
                                                    , command.StreetAddressModel.City
                                                    , command.StreetAddressModel.Suburb
                                                    , command.StreetAddressModel.PostalCode
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null);

            this.domainRuleContext.ExecuteRules(messages, addressDataModel);
            if (!messages.HasErrors)
            {
                int addressKey = addressDataManager.SaveAddress(addressDataModel);
                linkedKeyManager.LinkKeyToGuid(addressKey, command.AddressId);
            }

            return messages;
        }
    }
}